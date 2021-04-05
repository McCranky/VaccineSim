using Microsoft.AspNetCore.Components;
using SimulationLib.Simulations.EventSimulation.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccineSim.Services;

namespace VaccineSim.Pages
{
    public partial class Replications : IObserver<AfterReplicationValues>
    {
        [Inject] public VaccineSimulationService _simulationService { get; set; }

        private IDisposable unsubscriber;
        private AfterReplicationValues _afterReplicationValues = new AfterReplicationValues();

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
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
