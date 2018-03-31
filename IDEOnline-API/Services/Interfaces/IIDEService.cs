using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Services.Interfaces
{
    public interface IIDEService
    {
        string Run();
        string Compile(string code);
    }
}
