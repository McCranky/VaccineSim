using System;

namespace SimulationLib.Statistics
{
    public abstract class Statistic
    {
        public double Sum { get; protected set; }
        public double SumSquared { get; protected set; }
        public double Count { get; protected set; }
        public double Minimum { get; protected set; } = double.MaxValue;
        public double Maximum { get; protected set; } = double.MinValue;
        public double Average => Count != 0 ? Sum / Count : 0;
        public double LowerInterval => Count == 0 ? 0 : Average - (StandardDeviation() * 1.96d / Math.Sqrt(Count));
        public double HigherInterval => Count == 0 ? 0 : Average + (StandardDeviation() * 1.96d / Math.Sqrt(Count));

        public double StandardDeviation()
        {
            if (Count == 0) return 0;

            var firstPart = 1d / (Count - 1) * SumSquared;
            var secondPart = 1d / (Count - 1) * Sum;

            return Math.Sqrt(Math.Abs(firstPart - Math.Pow(secondPart, 2)));
        }

        public virtual void Clear()
        {
            Sum = 0;
            SumSquared = 0;
            Count = 0;
            Minimum = double.MaxValue;
            Maximum = double.MinValue;
        }
    }
}
