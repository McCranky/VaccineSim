using SimulationLib.Simulations.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimulationLib.Simulations.EventSimulation.Vaccination
{
    public class Patient : IPrioritizable
    {
        private static int _id = 0;
        public int Id { get; private set; }

        public double RegistrationEnqueue { get; set; }
        public double ExaminationEnqueue { get; set; }
        public double VaccinationEnqueue { get; set; }
        public double WaitingRoomEnter { get; set; }

        public double WaitingRoomEnd { get; set; }

        public double Priority => WaitingRoomEnd;

        public Patient()
        {
            Id = _id++;
        }

        public static void ResetId()
        {
            _id = 0;
        }
    }
}
