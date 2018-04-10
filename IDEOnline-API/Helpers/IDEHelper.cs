using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Helpers
{
    /// <summary>
    /// Helper class to manage compiling and running process of console application.
    /// </summary>
    public class IDEHelper
    {
        private Process process;
        private ProcessStartInfo startInfo;
        private string directory;

        /// <summary>
        /// EventHandler invoked when output from application is recived.
        /// </summary>
        public event EventHandler<string> OnOutputRecived;

        /// <summary>
        /// EventHandler ivoked when input of application is needed.
        /// </summary>
        public event EventHandler<string> OnStandardInputRequest;

        /// <summary>
        /// IDEHelper Constructor.
        /// </summary>
        public IDEHelper()
        {
            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "IDEOnline", "Miscs");
        }

        /// <summary>
        /// Method used to compile code saved in file.
        /// </summary>
        /// <param name="ID">File name where code is saved. Further process name.</param>
        /// <returns></returns>
        public async Task<string> CompileCodeAsync(string ID)
        {
            startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\Roslyn\csc.exe",
                WorkingDirectory = directory,
                Arguments = $"{ID}.cs",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            var exitCode = process.ExitCode;

            if (exitCode == 0)
                return "Compile Succeded!";

            return output;
        }

        /// <summary>
        /// Method to start console application process.
        /// </summary>
        /// <param name="ID">File name created during compiling process</param>
        /// <returns></returns>
        public async Task<int> RunAppAsync(string ID)
        {
            startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(directory, $"{ID}"),
                WorkingDirectory = directory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            await ScanOutputAsync();

            return 0;
        }

        /// <summary>
        /// Kill process of given ID name.
        /// </summary>
        /// <param name="ID">Process name</param>
        public void KillRunningProcess(string ID)
        {
            var processes = Process.GetProcessesByName(ID);

            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        /// <summary>
        /// Method used to pass value to standard input
        /// </summary>
        /// <param name="input">Input value</param>
        /// <returns></returns>
        public async Task PassInputAsync(string input)
        {
        }

        /// <summary>
        /// Method to scan standard output of console application.
        /// </summary>
        /// <returns></returns>
        private async Task ScanOutputAsync()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    OnOutputRecived.Invoke(this, process.StandardOutput.ReadLine());

                    if (process.StandardOutput.EndOfStream)
                    {
                        break;
                    }
                }
            });
        }
    }
}
