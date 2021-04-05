using SimulationLib.Simulations.EventSimulation.Core;
using SimulationLib.Simulations.Structures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationLib.Simulations.EventSimulation
{
    public class EventSimulationCore : MonteCarlo
    {
        public EventSimulationSettings Settings { get; private set; }
        public PriorityQueue<Event> Timeline { get; private set; }
        public double ActualSimulationTime { get; set; }
        public bool IsPauseEventInTimeline { get; set; }

        public EventSimulationCore(MonteCarloSettings monteCarloSettings, EventSimulationSettings eventSimulationSettings) : base(monteCarloSettings)
        {
            Settings = eventSimulationSettings;
            Timeline = new PriorityQueue<Event>();
        }

        protected override async Task DoReplication()
        {
            while (Timeline.Count > 0 
                && !MCSettings.CancellationToken)
            {
                // zistenie či sa sleduje simulacia a zaradenie čakacieho eventu na os
                if (Settings.IsWatching && !IsPauseEventInTimeline)
                {
                    Timeline.Enqueue(new PauseEvent
                    {
                        Simulation = this,
                        Time = ActualSimulationTime + Settings.PauseEventGap
                    });
                    IsPauseEventInTimeline = true;
                }

                // vybratie eventu
                var timelineEvent = Timeline.Dequeue();
                ActualSimulationTime = timelineEvent.Time;

                // ak je čakaci event tak sa počka a pripadne znovu naplanuje
                if (timelineEvent is PauseEvent)
                {
                    await Task.Delay(Settings.PauseDelay / Settings.SpeedModification);
                    if (Settings.IsWatching)
                    {
                        timelineEvent.Time += Settings.PauseEventGap;
                        Timeline.Enqueue(timelineEvent);
                    }
                    else
                    {
                        IsPauseEventInTimeline = false;
                    }
                }
                else // inak sa vykoná daný event
                {
                    timelineEvent.Execute();
                }

                AfterEvent();

                while (Settings.IsPause)
                {
                    await Task.Delay(100);
                }
            }
        }

        protected virtual void AfterEvent() { }
        public virtual void Restart(bool overReplication = false)
        {
            Timeline = new PriorityQueue<Event>();
        }
    }
}
