namespace SimulationLib.Simulations.EventSimulation.Vaccination.Events
{
    public class PatientArrival : VaccinationEvent
    {
        public override void Execute()
        {
            var sim = Simulation as VaccinationSimulation;

            var arrive = sim.ArrivalGen.NextDouble();
            if (arrive > sim.NotArriveProbability)
            {
                var worker = sim.RegistrationRoom.AssignWork(Time);
                if (worker != null)
                {
                    // poznačenie štatistik a naplanovanie začiatku registracie
                    sim.RegistrationWaiting.Add();

                    sim.Timeline.Enqueue(new RegistrationStart
                    {
                        Patient = Patient,
                        Simulation = Simulation,
                        Time = Time,
                        Worker = worker
                    });
                }
                else
                {
                    // zaradenie do frontu a poznačenie štatistik
                    sim.RegistrationFront.Enqueue(Patient);
                    sim.RegistrationLength.Add(sim.RegistrationFront.Count, Time);

                    Patient.RegistrationEnqueue = Time;
                }
            }
            else
            {
                ++sim.NotArrivedPatients;
            }

            if (sim.ActualSimulationTime >= sim.Settings.SimulationTime) return;

            Time += sim.Settings.SimulationTime / sim.VaccinSettings.Patients;
            Patient = new Patient();
            sim.Timeline.Enqueue(this);
        }
    }
}
