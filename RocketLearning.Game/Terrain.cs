namespace RocketLearning.Game;

public class Terrain
{
    // Position and size of the landing pad
    public double PlatformX { get;} = 100;
    public double PlatformY { get;} = 600;
    public double PlatformWidth { get;} = 200;
    public double PlatformHeight { get;} = 10;

    // For drawing
    public (double X, double Y, double Width, double Height) GetPlatformRect()
        => (PlatformX, PlatformY, PlatformWidth, PlatformHeight);

    // For collision
    public bool CheckLanding(Rocket rocket)
    {
        double rocketBottom = rocket.PositionY + 80;
        double rocketLeft = rocket.PositionX - 80;
        double rocketRight = rocket.PositionX + 80;

        bool verticallyAligned = rocketBottom >= PlatformY && rocketBottom <= PlatformY + PlatformHeight;
        bool horizontallyAligned = rocketRight >= PlatformX && rocketLeft <= PlatformX + PlatformWidth;

        return verticallyAligned && horizontallyAligned;
    }
}