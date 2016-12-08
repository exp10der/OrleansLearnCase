namespace CaseGrains
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using Orleans;

    public class CaseGrain : Grain, ICase
    {
        public Task<string> RunTest(string args)
        {
            throw new NotImplementedException();
        }
    }
}