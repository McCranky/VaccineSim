using System.Threading;

namespace SimulationLib.Simulations.EventSimulation.Core
{
    public class PauseEvent : Event
    {
        public override void Execute()
        {
            Thread.Sleep(Simulation.Settings.PauseDelay);

            if (Simulation.Settings.IsWatching)
            {
                Time += Simulation.Settings.PauseEventGap;
                Simulation.Timeline.Enqueue(this);
            }
            else
            {
                Simulation.IsPauseEventInTimeline = false;
            }
        }
    }
}
