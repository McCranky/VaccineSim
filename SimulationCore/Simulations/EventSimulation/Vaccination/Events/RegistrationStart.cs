namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public class RegistrationStart : VaccinationEvent
    {
        public override void Execute()
        {
            var sim = Simulation as VaccinationSimulation;

            // naplánovanie konca registrácie
            sim.Timeline.Enqueue(new RegistrationEnd
            {
                Simulation = Simulation,
                Patient = Patient,
                Time = Time + sim.RegistrationGen.Next(),
                Worker = Worker
            });
        }
    }
}
