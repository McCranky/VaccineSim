using SimulationLib.Statistics;

namespace SimulationLib.Simulations.EventSimulation.Vaccination
{
    public class Worker
    {
        private static int _id = 0;
        public int Id { get; set; }
        public bool CanWork { get; protected set; } = true;
        public ContinuousStatistic Workload { get; protected set; }

        public Worker()
        {
            Id = _id++;
            Workload = new ContinuousStatistic();
        }

        public void StartWorking(double actualTime)
        {
            CanWork = false;
            Workload.Add(1, actualTime);
        }

        public void StopWorking(double actualTime)
        {
            CanWork = true;
            Workload.Add(0, actualTime);
        }

        public static void ResetId()
        {
            _id = 0;
        }

        public void Clear()
        {
            CanWork = true;
            Workload.Clear();
        }
    }
}
