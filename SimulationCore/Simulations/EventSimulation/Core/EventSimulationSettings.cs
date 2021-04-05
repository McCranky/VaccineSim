namespace SimulationLib.Simulations.EventSimulation
{
    public class EventSimulationSettings
    {
        public double SimulationTime { get; set; }
        public bool IsPause { get; set; }
        public bool IsWatching { get; set; }
        public int PauseDelay { get; set; } = 500;
        public double PauseEventGap { get; set; } = 0.5d;
        public int SpeedModification { get; set; } = 1;
    }
}
