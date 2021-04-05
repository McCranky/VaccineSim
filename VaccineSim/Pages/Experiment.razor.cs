using Microsoft.AspNetCore.Components;
using SimulationLib.Simulations.EventSimulation.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccineSim.Services;
using Plotly.Blazor;
using Plotly.Blazor.LayoutLib;
using Plotly.Blazor.Traces;
using Plotly.Blazor.Traces.ScatterLib;

namespace VaccineSim.Pages
{
    public partial class Experiment : IObserver<DoctorsExperimentValues>
    {
        [Inject] public VaccineSimulationService _simulationService { get; set; }

        private IDisposable unsubscriber;
        private DoctorsExperimentValues _experimentResult = new DoctorsExperimentValues();


        private PlotlyChart chart;
        private Config config = new Config();
        private Layout layout = new Layout
        {
            Title = new Title { Text = "Doctors experiment" },
            YAxis = new List<YAxis> { new YAxis { Title = new Plotly.Blazor.LayoutLib.YAxisLib.Title { Text = "Front length" } } },
            XAxis = new List<XAxis> { new XAxis { Title = new Plotly.Blazor.LayoutLib.XAxisLib.Title { Text = "Doctors count" } } },
            AutoSize = true
        };
        private IList<ITrace> data = new List<ITrace>
        {
            new Scatter
            {
                Name = "Length",
                Mode = ModeFlag.Lines,
                X = new List<object>{},
                Y = new List<object>{}
            }
        };


        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                unsubscriber = _simulationService.SubscribeToExperiment(this);
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

        public void OnNext(DoctorsExperimentValues value)
        {
            //_experimentResult = value;
            
            InvokeAsync(async () => await AddToPlot(value.AverageFrontLength, value.DoctorsCount));
        }

        private async Task AddToPlot(double frontLength, int doctorsCount)
        {
            await chart.ExtendTrace(doctorsCount, frontLength, 0);
        }

        private async Task StartExperiment()
        {
            await chart.Clear();
            await chart.AddTrace(new Scatter
            {
                Name = "Length",
                Mode = ModeFlag.Lines,
                X = new List<object> { },
                Y = new List<object> { }
            });

            _simulationService.MCSettings.CancellationToken = false;
            await _simulationService.StartSimulation();
        }
    }
}
