using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Services.Interfaces
{
    /// <summary>
    /// IDEService interface.
    /// </summary>
    public interface IIDEService
    {
        event EventHandler<string> OnOutputRecived;
        event EventHandler<string> OnStandardInputRequest;

        Task<int> RunAsync(string ID);
        Task<string> CompileAsync(string code, string ID);
        Task PassInputAsync(string input);
    }
}
