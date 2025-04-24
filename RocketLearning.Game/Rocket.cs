namespace RocketLearning.Game;

public class Rocket
{
    public double PositionY { get; set; } = 100;
    public double PositionX { get; set; } = 100;
    public double Angle { get; set; } = 0;
    public void RotateLeft() => Angle -= 6;
    public void RotateRight() => Angle += 6;
    public void LeftMotor() => PositionY -= 10;
    public void RightMotor() => PositionY += 10;
}