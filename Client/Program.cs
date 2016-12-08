namespace Client
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using Orleans;
    using Orleans.Runtime.Configuration;

    internal class Program
    {
        private static void Main()
        {
            var config = ClientConfiguration.LocalhostSilo();

            try
            {
                Init(config);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Send().Wait();
        }

        private static void Init(ClientConfiguration config)
        {
            try
            {
                GrainClient.Initialize(config);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task Send()
        {
            var @case = GrainClient.GrainFactory.GetGrain<ICase>(0);
            var response = await @case.RunTest("Test Is Running");

            Console.WriteLine(response);
        }
    }
}