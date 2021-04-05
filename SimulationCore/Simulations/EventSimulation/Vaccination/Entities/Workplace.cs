using SimulationLib.Generators;
using SimulationLib.Simulations.EventSimulation.Vaccination.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimulationLib.Simulations.EventSimulation.Vaccination
{
    public class Workplace
    {
        private List<Worker> _workers;
        private RandomGenFactory _random;
        public Workplace(int count)
        {
            _workers = new List<Worker>(count);
            for (int i = 0; i < count; i++)
            {
                _workers.Add(new Worker());
            }

            _random = new RandomGenFactory(count);
        }

        public int AvaliableWorkers => _workers.Where(w => w.CanWork).Count();

        public double Workload => _workers.Select(w => w.Workload.Average).Sum() / _workers.Count;

        public Worker AssignWork(double actualTime)
        {
            var workers = _workers.Where(w => w.CanWork).ToArray();
            if (workers.Length == 0) return null;

            var worker = workers[0];
            if (workers.Length > 1)
            {
                worker = workers[_random.Next(workers.Length)];
            }

            worker.StartWorking(actualTime);
            return worker;
        }

        public List<WorkerStats> GetWorkersStats()
        {
            var stats = new List<WorkerStats>();
            foreach (var worker in _workers)
            {
                stats.Add(new WorkerStats { 
                    Id = worker.Id,
                    IsWorking = !worker.CanWork,
                    Workload = worker.Workload.Average
                });
            }
            return stats;
        }

        public void Restart()
        {
            foreach (var worker in _workers)
            {
                worker.Clear();
            }
        }
    }
}
