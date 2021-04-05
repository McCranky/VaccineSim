using System;

namespace SimulationLib.Generators
{
    public class PoissonGenerator
    {
        private readonly double _lambda;
        private readonly Random _rnd;

        public PoissonGenerator(double lambda, int seed = default)
        {
            _lambda = lambda;
            _rnd = seed == default ? new Random() : new Random(seed);
        }

        public double Next()
        {
            var L = Math.Pow(Math.E, -_lambda);
            var k = 0;
            var p = 1d;

            do
            {
                ++k;
                var u = _rnd.NextDouble();
                p *= u;

            } while (p > L);
            return k - 1;
        }
    }
}
