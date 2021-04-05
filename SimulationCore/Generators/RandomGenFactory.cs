using System;
using System.Collections.Generic;

namespace SimulationLib.Generators
{
    public class RandomGenFactory
    {
        private readonly List<Random> _generators;

        public RandomGenFactory(int count)
        {
            --count;
            _generators = new List<Random>(count);
            Console.WriteLine($"Initialising {count} generators");
            for (int i = 0; i < count; i++)
            {
                var seed = Guid.NewGuid().GetHashCode();
                Console.WriteLine($"Seed {i}: {seed}");
                _generators.Add(new Random(seed));
            }
        }

        public int Next(int count)
        {
            return _generators[count - 2].Next(count);
        }
    }
}
