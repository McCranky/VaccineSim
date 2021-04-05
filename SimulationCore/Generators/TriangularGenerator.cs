using System;
using System.Collections.Generic;
using System.Text;

namespace SimulationLib.Generators
{
    public class TriangularGenerator
    {
        private readonly double _min;
        private readonly double _max;
        private readonly double _med;
        private readonly Random _rnd;
        public TriangularGenerator(double min, double max, double med, int seed = default)
        {
            _min = min;
            _max = max;
            _med = med;
            _rnd = seed == default ? new Random() : new Random(seed);
        }

        public double Next()
        {
            var triangularDist = (_med - _min) / (_max - _min);
            var randomNum = _rnd.NextDouble();
            if (randomNum > 0 && randomNum < triangularDist)
            {
                return _min + Math.Sqrt(randomNum * (_max - _min) * (_med - _min));
            }
            else
            {
                return _max - Math.Sqrt((1 - randomNum) * (_max - _min) * (_max - _med));
            }
        }
    }
}
