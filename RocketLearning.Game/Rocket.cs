
namespace RocketLearning.Game;

public class Rocket
{
    private const double AngleChange = 3;
    private const double Speed = 10;

    public double PositionY { get; set; } = 100;
    public double PositionX { get; set; } = 100;
    public double Angle { get; set; }               

    private void Motor(bool left)
    {
        Angle = left ? (Angle + AngleChange)%360 : (Angle - AngleChange)%360;
        var radians = Angle * (Math.PI / 180);
        
        PositionX += Math.Sin(radians) * Speed;
        PositionY += Math.Cos(radians) * Speed;
    }
    public void LeftMotor() => Motor(true);
    public void RightMotor() => Motor(false);
}