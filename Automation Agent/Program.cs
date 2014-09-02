using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Automation_Agent
{
    static class Program
    {
        // \\servername\pipe\pipename.
        internal const string ServerName = ".";
        internal const string PipeName = "SamplePipe";
        internal const string FullPipeName = @"\\" + ServerName + @"\pipe\" + PipeName;

        internal const int BufferSize = 1024;

        // Request message from client to server. '\0' is appended in the end 
        // because the client may be a native C++ application that expects 
        // NULL termiated string.
        internal const string ResponseMessage = "Default response from server\0";
        internal static string RequestMessage;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                RequestMessage = args[0];//"c:/fail.exe";
                return SystemIONamedPipeClient.Run();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
                return 0;
            }

        }
    }
}
