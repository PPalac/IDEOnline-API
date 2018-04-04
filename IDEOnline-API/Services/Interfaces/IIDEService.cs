using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Services.Interfaces
{
    public interface IIDEService
    {
        Task<string> RunAsync();
        Task<string> CompileAsync(string code);
    }
}
