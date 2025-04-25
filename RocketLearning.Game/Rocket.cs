namespace RocketLearning.Game;
//TO DO: Game state will have the rocket and GameViewModel will have gameState
//TO DO: Game object from which Rocket inherits and Games physics which apply to all game objects
//TO DO: Terrain 
//TO DO: Crashing when outside the bounds
//TO DO: Score when landing 
//TO DO: Improve physics of the rocket when landing such that it can flip or fall on its side 
public class Rocket
{
    private const double AngleChange = 4;
    private const double EnginePower = 0.75;
    private const double Gravity = 0.03;

    public double PositionX { get; private set; } = 400;
    public double PositionY { get; private set; } = 100;
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