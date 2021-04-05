namespace SimulationLib.Statistics
{
    public class ContinuousStatistic : Statistic
    {
        public double _lastWeight;
        public double _lastValue;
        public void Add(double value, double weight)
        {
            var currentWeight = weight - _lastWeight;

            Count += currentWeight;
            _lastWeight = weight;

            Sum += _lastValue * currentWeight;
            SumSquared += System.Math.Pow(_lastValue * currentWeight, 2);
            _lastValue = value;


            Minimum = value < Minimum ? value : Minimum;
            Maximum = value < Maximum ? Maximum : value;

        }

        public override void Clear()
        {
            base.Clear();
            _lastValue = 0;
            _lastWeight = 0;
        }
    }
}
