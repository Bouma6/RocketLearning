namespace RocketLearning.Game;

public class Terrain
{
    private const double Screenwidth = 1600;
    private const double Screenheight = 900;

    // Position and size of the landing pad
    public double PlatformX { get;} = 700;
    public double PlatformY { get;} = 100;
    public double PlatformWidth { get;} = 200;
    public double PlatformHeight { get;} = 10;

    // For drawing
    public (double X, double Y, double Width, double Height) GetPlatformRect()
        => (PlatformX, PlatformY, PlatformWidth, PlatformHeight);

    // For collision
    public RocketStates CheckLanding(Rocket rocket)
    {
        var rocketBottom = rocket.PositionY - 18;
        var rocketLeft = rocket.PositionX-40;
        var rocketRight = rocket.PositionX+40;
        
        var verticallyAligned = rocketBottom >= PlatformY && rocketBottom <= PlatformY + PlatformHeight;
        var horizontallyAligned = rocketRight >= PlatformX && rocketLeft <= PlatformX + PlatformWidth;

        if (verticallyAligned && horizontallyAligned) {return RocketStates.LandedInbound;}
        if (verticallyAligned){return RocketStates.LandedOutbound;}
        if (rocketLeft <= 20 || rocketRight >= Screenwidth + 20 || rocketBottom >= Screenheight) {return RocketStates.OutOfBounds;}

        return RocketStates.Flying;
    }
}