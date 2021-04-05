namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public class ExaminationStart : VaccinationEvent
    {
        public override void Execute()
        {
            var sim = Simulation as VaccinationSimulation;

            // naplánovanie konca vyšetrenia
            sim.Timeline.Enqueue(new ExaminationEnd
            {
                Simulation = Simulation,
                Patient = Patient,
                Time = Time + sim.ExaminationGen.Next(),
                Worker = Worker
            });
        }
    }
}
