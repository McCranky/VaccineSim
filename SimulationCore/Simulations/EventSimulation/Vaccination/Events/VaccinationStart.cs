namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public class VaccinationStart : VaccinationEvent
    {
        public override void Execute()
        {
            var sim = Simulation as VaccinationSimulation;

            // naplánovanie konca vakcinácie
            sim.Timeline.Enqueue(new VaccinationEnd
            {
                Simulation = Simulation,
                Patient = Patient,
                Time = Time + sim.VaccinationGen.Next(),
                Worker = Worker
            });
        }
    }
}
