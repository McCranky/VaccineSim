using System;
using System.Collections.Generic;

namespace SimulationLib.Simulations.EventSimulation.Vaccination.Observables
{
    public class SimulationObservable<TValue> : IObservable<TValue>
    {
        private readonly List<IObserver<TValue>> _observers;

        public SimulationObservable()
        {
            _observers = new List<IObserver<TValue>>();
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<TValue>(_observers, observer);
        }

        public void Next(TValue value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }
    }
}
