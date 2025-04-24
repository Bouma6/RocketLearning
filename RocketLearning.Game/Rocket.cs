namespace RocketLearning.Game;
//TO DO: Game object from which Rocket inherits and Games physics which apply to all game objects
public class Rocket
{
    private const double AngleChange = 4;
    private const double EnginePower = 0.75;
    private const double Gravity = 0.03;

    public double PositionX { get; private set; } = 400;
    public double PositionY { get; private set; } = 100;
    private double VelocityX { get; set; }
    private double VelocityY { get; set; }
    public double Angle { get; private set; }
    
    public void LeftMotor()
    {
        ApplyThrust(true);
    }

    public void RightMotor()
    {        
        ApplyThrust(false);
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