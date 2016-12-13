namespace JsonFileStorage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Orleans;
    using Orleans.Providers;
    using Orleans.Runtime;
    using Orleans.Storage;

    public class JsonFileStorageProvider : IStorageProvider
    {
        private Func<string, GrainReference, FileInfo> _strategyTakeFileInfo;
        public string Directory { get; private set; }
        public string Name { get; set; }
        public Logger Log { get; }

        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;
            Directory = config.Properties["directory"];

            _strategyTakeFileInfo =
                (grainType, grainReference) =>
                    new FileInfo(Path.Combine(Directory,
                        string.Format($"{grainType}-{grainReference.ToKeyString()}.json")));

            return TaskDone.Done;
        }

        public Task Close() => TaskDone.Done;

        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var fileInfo = _strategyTakeFileInfo(grainType, grainReference);
            if (!fileInfo.Exists) return;

            using (var stream = fileInfo.OpenText())
            {
                var stringData = await stream.ReadToEndAsync();
                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(stringData);
                grainState.State = json;
            }
        }

        public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var json = JsonConvert.SerializeObject(grainState.State);
            var fileInfo = _strategyTakeFileInfo(grainType, grainReference);

            using (var stream = fileInfo.OpenWrite())
            using (var writer = new StreamWriter(stream))
            {
                return writer.WriteAsync(json);
            }
        }

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var fileInfo = _strategyTakeFileInfo(grainType, grainReference);
            fileInfo.Delete();

            return TaskDone.Done;
        }
    }
}