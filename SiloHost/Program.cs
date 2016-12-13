namespace SiloHost
{
    using System;
    using JsonFileStorage;
    using Orleans.Runtime;
    using Orleans.Runtime.Configuration;
    using Orleans.Runtime.Host;

    internal class Program
    {
        private static void Main()
        {
            CreateServer();

            Console.ReadKey();
        }

        private static void CreateServer()
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.AddMemoryStorageProvider();
            config.AddJsonFileStorageProvider("", "AzureStore");
            //config.AddAzureTableStorageProvider("AzureStore", "UseDevelopmentStorage=true");

            var host = new SiloHost("Test", config);
            host.LoadOrleansConfig();

            try
            {
                host.InitializeOrleansSilo();

                if (host.StartOrleansSilo())
                    Console.WriteLine($"Successfully started Orleans silo '{host.Name}' as a {host.Type} node.");
                else
                    throw new OrleansException($"Failed to start Orleans silo '{host.Name}' as a {host.Type} node.");
            }
            catch (Exception e)
            {
                host.ReportStartupError(e);
            }
        }
    }
}