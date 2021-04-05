namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public class WaitingRoomEnd : VaccinationEvent
    {
        public override void Execute()
        {
            var sim = Simulation as VaccinationSimulation;

            --sim.WaitingRoom;
            sim.WaitingRoomLength.Add(sim.WaitingRoom, Time);
        }
    }
}
