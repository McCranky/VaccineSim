using SimulationLib.Simulations.EventSimulation.Vaccination.Models;
using System;
using System.Collections.Generic;

namespace SimulationLib.Simulations.EventSimulation.Vaccination.Observables
{
    public class AfterReplicationObservable : IObservable<AfterReplicationValues>
    {
        private readonly List<IObserver<AfterReplicationValues>> _observers;

        public AfterReplicationObservable()
        {
            _observers = new List<IObserver<AfterReplicationValues>>();
        }

        public IDisposable Subscribe(IObserver<AfterReplicationValues> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<AfterReplicationValues>(_observers, observer);
        }

        public void Next(AfterReplicationValues value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }
    }
}
