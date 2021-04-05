namespace SimulationLib.Statistics
{
    public class DiscreetStatistic : Statistic
    {
        public void Add(double value = 0)
        {
            ++Count;

            Minimum = value < Minimum ? value : Minimum;
            Maximum = value < Maximum ? Maximum : value;

            Sum += value;
            SumSquared += System.Math.Pow(value, 2);
        }
    }
}
