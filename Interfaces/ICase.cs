namespace Interfaces
{
    using System.Threading.Tasks;
    using Orleans;

    public interface ICase : IGrainWithIntegerKey
    {
        Task<string> RunTest(string args);
    }
}