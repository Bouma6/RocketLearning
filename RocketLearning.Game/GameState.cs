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
    private double _timeScale  = 1;
    public double Time { get; private set; } = 0.0;
    public bool Crashed { get; private set; } = false;
    public bool Landed { get; private set; } = false;
    public bool GameOver { get; private set; } = false;
    public event Action? OnStateChanged;
    private readonly DispatcherTimer _timer;
    private RocketInput _pendingInput = RocketInput.None;



    public GameState()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16/_timeScale) // ~60 FPS
        };
        _timer.Tick += (_, _) => TickRealTime();
        _timer.Start();
    }
    private void StepOnce(RocketInput input)
    {
        if (GameOver) return;

        switch (input)
        {
            case RocketInput.Left:
                Rocket.LeftMotor();
                break;
            case RocketInput.Right:
                Rocket.RightMotor();
                break;
            case RocketInput.None:
                break;
        }
        Rocket.Tick();
        Time += 1.0 / 60.0;

        CheckLandingOrCrash();
        ConstantPunishment();
        
        OnStateChanged?.Invoke();
    }
    public void SetInput(RocketInput input)
    {
        _pendingInput = input;
    }
    public void Reset()
    {
        Rocket = new Rocket();
        Terrain = new Terrain();
        Time = 0;
        GameOver = false;
        Landed = false;
        Crashed = false;
        Score = 0;
    }

    public void TickRealTime()
    {
        if (GameOver) return;

        StepOnce(_pendingInput);
        _pendingInput = RocketInput.None;
    }

    private void ConstantPunishment()
    {
        var punish = TimePunishment * (Math.Abs(Rocket.PositionX - Terrain.PlatformX) ) *
            (Math.Abs(Rocket.PositionY - Terrain.PlatformY) ) / 600000;
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
        Score = Math.Max(Score,10);
        Console.WriteLine($"Final score:{Score}");
    }


}