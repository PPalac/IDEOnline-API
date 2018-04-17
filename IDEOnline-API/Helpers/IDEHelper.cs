using IDEOnlineAPI.Hubs;
using IDEOnlineAPI.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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
        private IHubContext<RuntimeHub> hubContext;

        private static List<StreamWriterModel> InputStreams = new List<StreamWriterModel>();
        private static List<string> processesToScan = new List<string>();

        /// <summary>
        /// IDEHelper Constructor.
        /// </summary>
        public IDEHelper()
        {
            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "IDEOnline", "Miscs");
        }

        public IDEHelper(IHubContext<RuntimeHub> hubContext)
        {
            this.hubContext = hubContext;
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

            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => { InputStreams.RemoveAll(st => st.Id == ID); RuntimeHub.connections.RemoveAll(c => c.ProcessId == ID); processesToScan.RemoveAll(p => p == ID); };
            process.Start();


            InputStreams.Add(new StreamWriterModel
            {
                Id = ID,
                StreamWriter = process.StandardInput
            });

            var outputScanThread = new Thread(new ParameterizedThreadStart(ScanOutputAsync))
            {
                IsBackground = true
            };

            processesToScan.Add(ID);
            outputScanThread.Start();

            return 0;
        }

        /// <summary>
        /// Kill process of given ID name.
        /// </summary>
        /// <param name="id">Process name</param>
        public void KillRunningProcess(string id)
        {
            var processes = Process.GetProcessesByName(id);

            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        /// <summary>
        /// Method used to pass value to standard input
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public void PassInputAsync(string input, string id)
        {
            var stream = InputStreams.Where(s => s.Id == id).FirstOrDefault();

            if (stream != null)
            {
                stream.StreamWriter.WriteLine(input);
                processesToScan.Add(id);
            }
        }

        /// <summary>
        /// Method to scan standard output of console application.
        /// </summary>
        /// <returns></returns>
        private void ScanOutputAsync(object data)
        {
            while (true)
            {
                if (process.StandardOutput.EndOfStream)
                {
                    break;
                }
                else
                {
                    var connection = RuntimeHub.connections.Where(c => c.ProcessId == process.ProcessName).SingleOrDefault();
                    if (connection != null)
                    {
                        hubContext.Clients.Client(connection.ConnectionId).SendAsync("Output", process.StandardOutput.ReadLine());
                    }
                }
            }
        }
    }
}
