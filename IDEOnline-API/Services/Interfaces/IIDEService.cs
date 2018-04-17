using System;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Services.Interfaces
{
    public interface IIDEService
    {
        Task<int> RunAsync(string ID);
        Task<string> CompileAsync(string code, string ID);
        void PassInputAsync(string input, string id);
        void Kill(string ID);
    }
}
