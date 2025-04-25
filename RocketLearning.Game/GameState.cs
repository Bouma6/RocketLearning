using System;
using Avalonia.Threading;

namespace RocketLearning.Game;

public class GameState
{
    public Rocket Rocket { get; private set; } = new Rocket();
    public Terrain Terrain { get; private set; } = new Terrain();

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

        UpdateScore();

        OnStateChanged?.Invoke();
    }

    private void CheckLandingOrCrash()
    {
        if (!Terrain.CheckLanding(Rocket)) return;

        GameOver = true;

        bool safeLanding = Math.Abs(Rocket.VelocityY) < 1.5 &&
                           Math.Abs(Rocket.VelocityX) < 1.5 &&
                           Math.Abs(NormalizeAngle(Rocket.Angle)) < 10;

        if (safeLanding)
        {
            Landed = true;
            Crashed = false;
        }
        else
        {
            Crashed = true;
            Landed = false;
        }
    }

    private void UpdateScore()
    {
        if (GameOver && Landed)
        {
            Score = Math.Max(1000 - Time * 10, 0);
        }
    }

    private double NormalizeAngle(double angle)
    {
        angle %= 360;
        return angle < 0 ? angle + 360 : angle;
    }
}