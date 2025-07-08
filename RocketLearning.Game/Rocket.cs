namespace RocketLearning.Game;
//TO DO: Game object from which Rocket inherits and Games physics which apply to all game objects
public class Rocket
{
    private const double AngleChange = 3;
    private const double EnginePower = 0.75;//0.75
    private const double Gravity = 0.04;//0.04

    public double PositionX { get; private set; } = 200.0 + new Random().NextDouble() * 1200.0;
    public double PositionY { get; private set; } = 700.0 + new Random().NextDouble() * 100.0;
    public double VelocityX { get; private set; }
    public double VelocityY { get; private set; }
    public double Angle { get; private set; }
    
    public void LeftMotor()
    {
        ApplyThrust(true);
    }

    public void RightMotor()
    {        
        ApplyThrust(false);
    }

    public void Motor()
    {
        var radians = Angle * (Math.PI / 180);
        VelocityX += Math.Sin(radians) * EnginePower;
        VelocityY += Math.Cos(radians) * EnginePower;
    }
    private void ApplyThrust(bool left)
    {
        Angle = left ? Angle-AngleChange : Angle + AngleChange;
        var radians = Angle * (Math.PI / 180);
        VelocityX += Math.Sin(radians) * EnginePower;
        VelocityY += Math.Cos(radians) * EnginePower;
    }
    public void Tick()
    {
        VelocityY -= Gravity;
        PositionX += VelocityX;
        PositionY += VelocityY;
    }
}