using System;

namespace SimulationLib.Generators
{
    public class ExponentialGenerator
    {
        private readonly double _lambda;
        private readonly Random _rnd;

        public ExponentialGenerator(double lambda, int seed = default)
        {
            _lambda = lambda;
            var guidSeed = Guid.NewGuid().GetHashCode();
            Console.WriteLine(guidSeed);
            _rnd = seed == default ? new Random(guidSeed) : new Random(seed);
        }

        public double Next()
        {
            return (-Math.Log(1 - _rnd.NextDouble())) / _lambda;
        }
    }
}
