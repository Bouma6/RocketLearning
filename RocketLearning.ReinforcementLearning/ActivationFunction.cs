namespace RocketLearning.ReinforcementLearning;
public static class ActivationFunction
{
    public static double Sigmoid(double x) => 1.0 / (1.0 + Math.Exp(-x));
    public static double Tanh(double x) => Math.Tanh(x);
    public static double RelU(double x) => Math.Max(0.0, x);
}