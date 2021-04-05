namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public class ExaminationEnd : VaccinationEvent
    {
        public override void Execute()
        {
            var sim = Simulation as VaccinationSimulation;

            // uvolnenie pracovníka
            Worker.StopWorking(Time);

            // naplánovanie vakcinácie ak je volna sestrička
            var worker = sim.VaccinationRoom.AssignWork(Time);
            if (worker != null)
            {
                sim.VaccinationWaiting.Add();

                sim.Timeline.Enqueue(new VaccinationStart
                {
                    Simulation = Simulation,
                    Time = Time,
                    Patient = Patient,
                    Worker = worker
                });
            }
            else // zaradenie do frontu na očkovanie a poznačenie štatistik
            {
                sim.VaccinationFront.Enqueue(Patient);
                sim.VaccinationLength.Add(sim.VaccinationFront.Count, Time);

                Patient.VaccinationEnqueue = Time;
            }

            // ak je niekto vo fronte na vyšetrenie a je volny pracovnik tak vytovrí event na začiatok vyšetrenia a poznači štatistiky
            if (sim.ExaminationFront.Count > 0 && sim.ExaminationRoom.AvaliableWorkers > 0)
            {
                var patient = sim.ExaminationFront.Dequeue();
                sim.ExaminationLength.Add(sim.ExaminationFront.Count, Time);
                sim.ExaminationWaiting.Add(Time - patient.ExaminationEnqueue);

                sim.Timeline.Enqueue(new ExaminationStart
                {
                    Patient = patient,
                    Simulation = Simulation,
                    Time = Time,
                    Worker = sim.ExaminationRoom.AssignWork(Time)
                });
            }
        }
    }
}
