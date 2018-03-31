using IDEOnlineAPI.Services.Interfaces;

namespace IDEOnlineAPI.Services
{
    public class IDEService : IIDEService
    {
        public IDEService()
        {

        }

        public string Compile(string code)
        {
            var result = (code);

            return result;
        }

        public string Run()
        {
            throw new System.NotImplementedException();
        }
    }
}
