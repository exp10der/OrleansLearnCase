namespace JsonFileStorage
{
    using System;
    using System.Collections.Generic;
    using Orleans.Runtime.Configuration;

    public static class JsonFileConfigurationExtensions
    {
        public static void AddJsonFileStorageProvider(this ClusterConfiguration config, string directory, string providerName = "JsonFileStore")
        {
            if (string.IsNullOrWhiteSpace(providerName))
                throw new ArgumentNullException(nameof(providerName));

            var dictionary = new Dictionary<string, string>
            {
                {"directory", directory}
            };

            config.Globals.RegisterStorageProvider<JsonFileStorageProvider>(providerName, dictionary);
        }
    }
}