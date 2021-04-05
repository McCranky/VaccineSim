using SimulationLib.Simulations.Structures;

namespace SimulationLib.Simulations.EventSimulation
{
    public abstract class Event : IPrioritizable
    {
        public EventSimulationCore Simulation { get; set; }
        public double Time { get; set; }
        public double Priority => Time;
        public abstract void Execute();
    }
}
