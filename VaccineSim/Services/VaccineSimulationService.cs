using SimulationLib.Simulations;
using SimulationLib.Simulations.EventSimulation;
using SimulationLib.Simulations.EventSimulation.Vaccination;
using SimulationLib.Simulations.EventSimulation.Vaccination.Models;
using System;
using System.Threading.Tasks;

namespace VaccineSim.Services
{
    public class VaccineSimulationService
    {
        public MonteCarloSettings MCSettings { get; private set; }
        public EventSimulationSettings ESSettings { get; private set; }
        public VaccinationSettings VacSettings { get; private set; }


        private VaccinationSimulation _simulation;

        public VaccineSimulationService()
        {
            MCSettings = new MonteCarloSettings
            {
                Replications = 100
            };

            ESSettings = new EventSimulationSettings
            {
                SimulationTime = 60 * 60 * 9
            };

            VacSettings = new VaccinationSettings
            {
                RegistrationWorkers = 5,
                ExaminationWorkers = 6,
                VaccinationWorkers = 3,
                Patients = 540,
                NotCommingLowerBoundry = 5,
                NotCommingHigherBoundry = 25
            };

            _simulation = new VaccinationSimulation(MCSettings, ESSettings, VacSettings);
        }

        public void ApplySettings()
        {
            _simulation = new VaccinationSimulation(MCSettings, ESSettings, VacSettings);
        }

        public void StopSimulation()
        {
            MCSettings.CancellationToken = true;
        }

        public IDisposable SubscribeToAfterEvent(IObserver<AfterEventValues> observer)
        {
            return _simulation?.Subscribe(observer);
        }
        public IDisposable SubscribeToAfterReplication(IObserver<AfterReplicationValues> observer)
        {
            return _simulation?.Subscribe(observer);
        }
        public IDisposable SubscribeToExperiment(IObserver<DoctorsExperimentValues> observer)
        {
            return _simulation?.Subscribe(observer);
        }

        public async Task StartSimulation()
        {
            if (_simulation.ActualSimulationTime != 0 || _simulation.ActualReplication != 0)
            {
                _simulation.Restart();
            }
            await _simulation.RunReplications();
        }

        public void PauseSimulation()
        {
            if (_simulation != null)
            {
                ESSettings.IsPause = !ESSettings.IsPause;
            }
        }
    }
}
