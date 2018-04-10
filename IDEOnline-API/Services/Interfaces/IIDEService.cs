using System;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Services.Interfaces
{
    public interface IIDEService
    {
        event EventHandler<string> OnOutputRecived;
        event EventHandler<string> OnStandardInputRequest;

        Task<int> RunAsync(string ID);
        Task<string> CompileAsync(string code, string ID);
        Task PassInputAsync(string input);
        void Kill(string ID);
    }
}
