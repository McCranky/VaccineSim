using System;
using System.Collections.Generic;
using System.Text;

namespace SimulationLib.Generators
{
    public class UniformGenerator
    {
        private readonly double _min;
        private readonly double _max;
        private readonly Random _rnd;

        public UniformGenerator(double min, double max, int seed = default)
        {
            _min = min;
            _max = max;

            var guidSeed = Guid.NewGuid().GetHashCode();
            _rnd = seed == default ? new Random(guidSeed) : new Random(seed);
        }

        public double Next()
        {
            return (_rnd.NextDouble() * (_max - _min)) + _min;
        }
    }
}
