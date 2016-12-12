namespace CaseGrains
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Orleans;
    using Orleans.Providers;

    [StorageProvider(ProviderName = "AzureStore")]
    public class CaseGrain : Grain<CaseGrainState>, ICase
    {
        private readonly Action<string> _log;

        public CaseGrain()
        {
            _log = Console.WriteLine;
        }

        public async Task<string> RunTest(string args)
        {
            State.CountTestRuns++;
            await WriteStateAsync();
            return string.Join("", args.Reverse());
        }

        public override Task OnActivateAsync()
        {
            var id = this.GetPrimaryKeyLong();

            _log(id.ToString());

            return base.OnActivateAsync();
        }
    }
}