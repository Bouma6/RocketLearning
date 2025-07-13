namespace RocketLearning.Game;

public class Terrain
{
    private const double Screenwidth = 1600;
    private const double Screenheight = 900;

    // Position and size of the landing pad
    public double PlatformX { get;} = 600;
    public double PlatformY { get;} = 210;
    public double PlatformWidth { get;} = 250;
    public double PlatformHeight { get;} = 10;
    
    // For collision
    public RocketStates CheckLanding(Rocket rocket)
    {
        var rocketBottom = rocket.PositionY - 18;
        var rocketLeft = rocket.PositionX-40;
        var rocketRight = rocket.PositionX+40;
        
        var verticallyAligned = rocketBottom >= PlatformY && rocketBottom <= PlatformY + PlatformHeight;
        var horizontallyAligned = rocketRight >= PlatformX && rocketLeft <= PlatformX + PlatformWidth;

        if (verticallyAligned && horizontallyAligned) {return RocketStates.LandedInbound;}

        if ((rocketBottom<PlatformY-40 &&rocketLeft<150)||(rocketBottom<PlatformY-20)||(rocketBottom<PlatformY-40 &&rocketLeft>1200))
        {
            return RocketStates.LandedOutbound;
        }
        if (rocketLeft <= 20 || rocketRight >= Screenwidth + 20 || rocketBottom >= Screenheight) {return RocketStates.OutOfBounds;}

        return RocketStates.Flying;
    }
}