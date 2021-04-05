namespace SimulationLib.Simulations.EventSimulation.Vaccination.Models
{
    public class VaccinationSettings
    {
        public int RegistrationWorkers { get; set; }
        public int ExaminationWorkers { get; set; }
        public int VaccinationWorkers { get; set; }

        public int Patients { get; set; }
        public int NotCommingLowerBoundry { get; set; }
        public int NotCommingHigherBoundry { get; set; }

        public bool DoctorsExperimentEnabled { get; set; }
        public int DoctorsMin { get; set; }
        public int DoctorsMax { get; set; }
        public int ExperimentReplications { get; set; }
    }
}
