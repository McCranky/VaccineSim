using SimulationLib.Statistics;

namespace SimulationLib.Simulations.EventSimulation.Vaccination.Models
{
    public class AfterReplicationValues
    {
        public int Replication { get; set; }

        public double RegistrationWorkload { get; set; }
        public double ExaminationWorkload { get; set; }
        public double VaccinationWorkload { get; set; }

        public double RegistrationAverageLength { get; set; }
        public double RegistrationAverageTime { get; set; }

        public double ExaminationAverageLength { get; set; }
        public double ExaminationAverageTime { get; set; }

        public double VaccinationAverageLength { get; set; }
        public double VaccinationAverageTime { get; set; }

        public DiscreetStatistic WaitingRoomLengthStats { get; set; } = new DiscreetStatistic();

        public double MissingPatients { get; set; }
        public double Overtime { get; set; }
    }
}
