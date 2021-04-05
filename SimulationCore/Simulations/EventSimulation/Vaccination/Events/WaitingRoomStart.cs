namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public class WaitingRoomStart : VaccinationEvent
    {
        public override void Execute()
        {
            var sim = Simulation as VaccinationSimulation;

            var waitingTime = sim.WaitingGen.NextDouble() < 0.95 ? 15 * 60 : 30 * 60;
            sim.Timeline.Enqueue(new WaitingRoomEnd
            {
                Simulation = Simulation,
                Patient = Patient,
                Time = Time + waitingTime
            });
        }
    }
}
