using System.Collections.Generic;

namespace SimulationLib.Simulations.EventSimulation.Vaccination.Models
{
    public class AfterEventValues
    {
        public double SimulationTime { get; set; }

        public int RegistrationFrontLength { get; set; }
        public int ExaminationFrontLength { get; set; }
        public int VaccinationFrontLength { get; set; }

        public List<WorkerStats> RegistrationWorkersStats { get; set; }
        public List<WorkerStats> ExaminationWorkersStats { get; set; }
        public List<WorkerStats> VaccinationWorkersStats { get; set; }

        public double RegistrationAverageLength { get; set; }
        public double RegistrationAverageTime { get; set; }

        public double ExaminationAverageLength { get; set; }
        public double ExaminationAverageTime { get; set; }

        public double VaccinationAverageLength { get; set; }
        public double VaccinationAverageTime { get; set; }

        public int WaitingRoomPatients { get; set; }
        public double WaitingRoomAverageLength { get; set; }
    }
}
