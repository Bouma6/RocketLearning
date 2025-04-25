using System;
using Avalonia.Threading;

namespace RocketLearning.Game;

public class GameState
{
    public Rocket Rocket { get; private set; } = new Rocket();
    public Terrain Terrain { get; private set; } = new Terrain();
    const int VelocityPunishment = 20;
    const int AnglePunishment = 50;
    const int TimePunishment = 3; 
    public double Score { get; private set; } = 0;
    public double Time { get; private set; } = 0.0;
    public bool Crashed { get; private set; } = false;
    public bool Landed { get; private set; } = false;
    public bool GameOver { get; private set; } = false;

    public event Action? OnStateChanged;

    private readonly DispatcherTimer _timer;

    public GameState()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
        };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();
    }

    private void Tick()
    {
        if (GameOver) return;

        Rocket.Tick();
        Time += 1.0 / 60.0;

        CheckLandingOrCrash();
        ConstantPunishment();
        
        OnStateChanged?.Invoke();
    }

    private void ConstantPunishment()
    {
        var punish = TimePunishment * (Math.Abs(Rocket.PositionX - Terrain.PlatformX) ) *
            (Math.Abs(Rocket.PositionY - Terrain.PlatformY) ) / 1000000;
        Score -= punish;
        Console.WriteLine($"Punishment: {punish}");
    }
    private void CheckLandingOrCrash()
    {
        var landing = Terrain.CheckLanding(Rocket);
        
        if (landing == RocketStates.Flying) return;
        GameOver = true;
        if (landing == RocketStates.LandedInbound)
        {
            Landed = true;
            Crashed = false;
            Score += 1000;
        }
        else if (landing == RocketStates.LandedOutbound)
        {
            Landed = true;
            Crashed = false;
            Score += 250 - Math.Abs(Rocket.PositionX-900);
        }
        else
        {
            Crashed = true;
            Landed = false;
            Score -= 800;
        }

        Score -= Math.Abs(Rocket.VelocityX) * VelocityPunishment;
        Score -= Math.Abs(Rocket.VelocityY) * VelocityPunishment;
        Score -= Math.Abs(Rocket.Angle) * AnglePunishment;
        
        Console.WriteLine($"Final score:{Score}");
    }


}