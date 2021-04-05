namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public abstract class VaccinationEvent : Event
    {
        public Patient Patient { get; set; }
        public Worker Worker { get; set; }
    }
}
