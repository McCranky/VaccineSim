using SimulationLib.Simulations.EventSimulation.Vaccination.Models;
using System;
using System.Collections.Generic;

namespace SimulationLib.Simulations.EventSimulation.Vaccination.Observables
{
    public class AfterEventObservable : IObservable<AfterEventValues>
    {
        private readonly List<IObserver<AfterEventValues>> _observers;

        public AfterEventObservable()
        {
            _observers = new List<IObserver<AfterEventValues>>();
        }

        public IDisposable Subscribe(IObserver<AfterEventValues> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<AfterEventValues>(_observers, observer);
        }

        public void Next(AfterEventValues value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }
    }
}
