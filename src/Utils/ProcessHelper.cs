using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Utils.Logging;

namespace Utils
{
    public class ProcessHelper
    {
        /// <summary>
        /// Standard input for the process
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Standard output from the process
        /// </summary>
        public string Output { get; private set; }

        /// <summary>
        /// Standard error output from the process
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Instance of the process
        /// </summary>
        private Process Process { get; set; }

        /// <summary>
        /// Hidden constructor since there's a factory method
        /// </summary>
        /// <param name="p"></param>
        private ProcessHelper(Process p)
        {
            Process = p;
        }

        /// <summary>
        /// Factory method for creating an instance of <see cref="ProcessHelper"/>
        /// </summary>
        /// <param name="tool">Name of the tool</param>
        /// <param name="argFormat">Arguments string format</param>
        /// <param name="values">Values for the string format</param>
        /// <returns>An instance of <see cref="ProcessHelper"/></returns>
        public static ProcessHelper Create(string tool, string argFormat = null, params object[] values)
        {
            string args = string.Format(argFormat ?? string.Empty, values);
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = tool,
                    Arguments = args,

                    UseShellExecute = false,
                    CreateNoWindow = true,

                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };

            return new ProcessHelper(process);
        }

        /// <summary>
        /// Starts the process and wait for exit
        /// </summary>
        /// <param name="waitMs">Time to wait for process to exit in milliseconds</param>
        /// <returns>True if it was run successfully</returns>
        public bool Run(int waitMs = int.MaxValue)
        {
            try
            {
                LogHelper.LogInfo("Starting process [{0} {1}]", Process.StartInfo.FileName, Process.StartInfo.Arguments);
                Process.Start();
            }
            catch (Exception e)
            {
                LogHelper.LogError(
                    "Exception while trying to run command ({0} {1}). Message: {2}",
                    Process.StartInfo.FileName,
                    Process.StartInfo.Arguments,
                    e.Message);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Input))
            {
                using (StreamWriter stdIn = Process.StandardInput)
                {
                    stdIn.WriteLine(Input);
                }
            }

            Task<string> stdOut = Process.StandardOutput.ReadToEndAsync();
            Task<string> stdErr = Process.StandardError.ReadToEndAsync();

            Process.WaitForExit(waitMs);
            if (!Process.HasExited)
            {
                Process.Kill();
                Process.WaitForExit(5000);
                LogHelper.LogError("Process: ({0} {1}) timed out.", Process.StartInfo.FileName, Process.StartInfo.Arguments);
            }

            Output = stdOut.Result;
            Error = stdErr.Result;
            LogHelper.LogInfo("Process exited with code {0}. StdOut -\r\n[{1}]", Process.ExitCode, Output);
            if (!string.IsNullOrWhiteSpace(Error))
            {
                LogHelper.LogInfo("Standard error: [{0}]", Error);
            }

            return Process.ExitCode == 0;
        }
    }
}