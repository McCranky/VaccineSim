namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public class VaccinationEnd : VaccinationEvent
    {
        public override void Execute()
        {
            var sim = Simulation as VaccinationSimulation;

            // uvolnenie pracovníka
            Worker.StopWorking(Time);

            // naplánovanie začiatku čakania pacienta v miestnosti
            ++sim.WaitingRoom;
            sim.WaitingRoomLength.Add(sim.WaitingRoom, Time);
            sim.Timeline.Enqueue(new WaitingRoomStart
            {
                Simulation = Simulation,
                Time = Time,
                Patient = Patient
            });

            // ak je niekto vo fronte na očkovanie a je volny pracovnik tak vytovrí event na začiatok očkovania a poznači štatistiky
            if (sim.VaccinationFront.Count > 0 && sim.VaccinationRoom.AvaliableWorkers > 0)
            {
                var patient = sim.VaccinationFront.Dequeue();
                sim.VaccinationLength.Add(sim.VaccinationFront.Count, Time);
                sim.VaccinationWaiting.Add(Time - patient.VaccinationEnqueue);

                sim.Timeline.Enqueue(new VaccinationStart
                {
                    Patient = patient,
                    Simulation = Simulation,
                    Time = Time,
                    Worker = sim.VaccinationRoom.AssignWork(Time)
                });
            }
        }
    }
}
