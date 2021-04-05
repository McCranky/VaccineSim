using System;
using System.Collections.Generic;

namespace SimulationLib.Simulations.EventSimulation.Vaccination.Observables
{
    public class Unsubscriber<TValue> : IDisposable
    {
        private readonly List<IObserver<TValue>> _observers;
        private readonly IObserver<TValue> _observer;

        public Unsubscriber(List<IObserver<TValue>> observers, IObserver<TValue> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (!(_observer == null)) _observers.Remove(_observer);
        }
    }
}
