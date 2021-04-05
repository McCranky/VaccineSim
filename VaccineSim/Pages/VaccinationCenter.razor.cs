using Microsoft.AspNetCore.Components;
using SimulationLib.Simulations.EventSimulation.Vaccination.Models;
using System;
using System.Threading.Tasks;
using VaccineSim.Services;

namespace VaccineSim.Pages
{
    public partial class VaccinationCenter : IObserver<AfterEventValues>, IObserver<AfterReplicationValues>
    {
        [Inject] public VaccineSimulationService _simulationService { get; set; }
        private IDisposable unsubscriber;
        //private VaccinationSimulation _simulation;
        private AfterEventValues _afterEventValues = new AfterEventValues();
        private AfterReplicationValues _afterReplicationValues = new AfterReplicationValues();

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                unsubscriber = _simulationService.SubscribeToAfterEvent(this);
                unsubscriber = _simulationService.SubscribeToAfterReplication(this);
            }
        }

        public void OnCompleted()
        {
            Console.WriteLine("Done!");
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(AfterEventValues value)
        {
            _afterEventValues = value;
            InvokeAsync(StateHasChanged);
        }

        public void OnNext(AfterReplicationValues value)
        {
            _afterReplicationValues = value;
            InvokeAsync(StateHasChanged);
        }

        private async Task StartSimulation()
        {
            _simulationService.MCSettings.CancellationToken = false;
            await _simulationService.StartSimulation();
        }

        private void PauseSimulation()
        {
            _simulationService.PauseSimulation();
        }

        private void StopSimulation()
        {
            _simulationService.StopSimulation();
        }
    }
}
