using System;

namespace RocketLearning.Game;

public class GameState
{
    public Rocket Rocket { get; private set; } = new();
    public Terrain Terrain { get; private set; } = new();
    public double DeltaTime { get; } = 1.0 / 60;
    public double Score { get; private set; } = 0;
    public double Time { get; private set; } = 0;

    public bool Crashed { get; private set; } = false;
    public bool Landed { get; private set; } = false;
    public bool GameOver { get; private set; } = false;

    public event Action? OnStateChanged;

    private const int VelocityPunishment = 20;
    private const int AnglePunishment = 50;
    private const int TimePunishment = 3;

    public void Tick(RocketInput input, double deltaTime)
    {
        if (GameOver)
            return;

        ApplyInput(input);
        Rocket.Tick();
        Time += deltaTime;

        CheckLandingOrCrash();
        ApplyConstantPunishment();

        OnStateChanged?.Invoke();
    }

    public void Reset()
    {
        Rocket = new Rocket();
        Terrain = new Terrain();
        Time = 0;
        Score = 0;
        Crashed = false;
        Landed = false;
        GameOver = false;

        OnStateChanged?.Invoke();
    }

    private void ApplyInput(RocketInput input)
    {
        switch (input)
        {
            case RocketInput.Left:
                Rocket.LeftMotor();
                break;
            case RocketInput.Right:
                Rocket.RightMotor();
                break;
        }
    }

    private void ApplyConstantPunishment()
    {
        double dx = Math.Abs(Rocket.PositionX - Terrain.PlatformX);
        double dy = Math.Abs(Rocket.PositionY - Terrain.PlatformY);
        double punishment = TimePunishment * dx * dy / 600000;
        Score -= punishment;
    }

    private void CheckLandingOrCrash()
    {
        var result = Terrain.CheckLanding(Rocket);

        if (result == RocketStates.Flying)
            return;

        GameOver = true;

        if (result == RocketStates.LandedInbound)
        {
            Landed = true;
            Score += 1000;
        }
        else if (result == RocketStates.LandedOutbound)
        {
            Landed = true;
            Score += 250 - Math.Abs(Rocket.PositionX - 900);
        }
        else
        {
            Crashed = true;
            Score -= 800;
        }

        Score -= Math.Abs(Rocket.VelocityX) * VelocityPunishment;
        Score -= Math.Abs(Rocket.VelocityY) * VelocityPunishment;
        Score -= Math.Abs(Rocket.Angle) * AnglePunishment;
        Score = Math.Max(Score, 10);
    }
}
