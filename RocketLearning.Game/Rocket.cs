namespace RocketLearning.Game;
public class Rocket(double spawnX)
{
    private const double AngleChange = 3;
    private const double EnginePower = 0.75;//0.75
    private const double Gravity = 0.04;//0.04

    public double PositionX { get; private set; } = spawnX;
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

    //If you want to also add the option of going straight up and not having to use left or right motor - just using main motor in middle not changing the angle
    public void Motor()
    {
        var radians = Angle * (Math.PI / 180);
        VelocityX += Math.Sin(radians) * EnginePower;
        VelocityY += Math.Cos(radians) * EnginePower;
    }
    //Usage of the left or right motor - the rocket always tilts one way or another 
    private void ApplyThrust(bool left)
    {
        Angle = left ? Angle-AngleChange : Angle + AngleChange;
        var radians = Angle * (Math.PI / 180);
        VelocityX += Math.Sin(radians) * EnginePower;
        VelocityY += Math.Cos(radians) * EnginePower;
    }
    // Real world physics effect on the rocket. 
    // Rocket starts to fall and lose speed 
    public void Tick()
    {
        VelocityY -= Gravity;
        PositionX += VelocityX;
        PositionY += VelocityY;
    }
}