namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public class RegistrationEnd : VaccinationEvent
    {
        public override void Execute()
        {
            var sim = Simulation as VaccinationSimulation;

            // uvolnenie pracovníka
            Worker.StopWorking(Time);

            // naplánovanie vyšetrenia ak je volny lekár
            var worker = sim.ExaminationRoom.AssignWork(Time);
            if (worker != null)
            {
                sim.ExaminationWaiting.Add();

                sim.Timeline.Enqueue(new ExaminationStart
                {
                    Simulation = Simulation,
                    Time = Time,
                    Patient = Patient,
                    Worker = worker
                });
            }
            else // zaradenie do frontu na vyšetrenie a poznačenie štatistik
            {
                sim.ExaminationFront.Enqueue(Patient);
                sim.ExaminationLength.Add(sim.ExaminationFront.Count, Time);

                Patient.ExaminationEnqueue = Time;
            }

            // ak je niekto vo fronte na registraciu a je volny pracovnik tak vytovrí event na začiatok registracie a poznači štatistiky
            if (sim.RegistrationFront.Count > 0 && sim.RegistrationRoom.AvaliableWorkers > 0)
            {
                var patient = sim.RegistrationFront.Dequeue();
                sim.RegistrationLength.Add(sim.RegistrationFront.Count, Time);
                sim.RegistrationWaiting.Add(Time - patient.RegistrationEnqueue);


                sim.Timeline.Enqueue(new RegistrationStart
                {
                    Patient = patient,
                    Simulation = Simulation,
                    Time = Time,
                    Worker = sim.RegistrationRoom.AssignWork(Time)
                });
            }
        }
    }
}
