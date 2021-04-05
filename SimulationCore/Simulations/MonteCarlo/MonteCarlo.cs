using System.Threading.Tasks;

namespace SimulationLib.Simulations
{
    public abstract class MonteCarlo
    {
        public int ActualReplication { get; set; }
        public MonteCarloSettings MCSettings { get; private set; }
        public MonteCarlo(MonteCarloSettings settings)
        {
            MCSettings = settings;
        }

        protected virtual void BeforeReplication() { }
        public async Task RunReplications()
        {
            for (int i = 0; i < MCSettings.Replications && !MCSettings.CancellationToken; i++)
            {
                ActualReplication = i + 1;

                BeforeReplication();

                await DoReplication();
                await Task.Delay(1); // For Blazor rerendering purposes

                AfterReplication();
            }
        }
        protected virtual void AfterReplication() { }

        protected abstract Task DoReplication();
    }
}
