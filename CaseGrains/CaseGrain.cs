namespace CaseGrains
{
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Orleans;

    public class CaseGrain : Grain, ICase
    {
        public Task<string> RunTest(string args) => Task.FromResult(string.Join("", args.Reverse()));
    }
}