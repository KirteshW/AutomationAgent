using System;
using System.IO.Pipes;
using System.Text;

namespace Automation_Agent
{
    class SystemIONamedPipeClient
    {
        /// <summary>
        /// Use the types in the System.IO.Pipes namespace to connect to the 
        /// named pipe. This solution is recommended when you program against 
        /// .NET Framework 3.5 or any newer releases of .NET Framework.
        /// </summary>
        public static int Run()
        {
            NamedPipeClientStream pipeClient = null;
            int retcode=0;
            try
            {
                // Try to open the named pipe identified by the pipe name.

                pipeClient = new NamedPipeClientStream(
                    Program.ServerName,         // The server name
                    Program.PipeName,           // The unique pipe name
                    PipeDirection.InOut,        // The pipe is duplex
                    PipeOptions.None            // No additional parameters
                    );

                pipeClient.Connect(5000);
                Console.WriteLine("The named pipe ({0}) is connected.",
                    Program.FullPipeName);

                // Set the read mode and the blocking mode of the named pipe. In 
                // this sample, we set data to be read from the pipe as a stream 
                // of messages. This allows a reading process to read varying-
                // length messages precisely as sent by the writing process. In 
                // this mode, you should not use StreamWriter to write the pipe, 
                // or use StreamReader to read the pipe. You can read more about 
                // the difference from http://go.microsoft.com/?linkid=9721786.
                pipeClient.ReadMode = PipeTransmissionMode.Message;

                // 
                // Send a request from client to server
                // 

                string message = Program.RequestMessage;
                byte[] bRequest = Encoding.Unicode.GetBytes(message);
                int cbRequest = bRequest.Length;

                pipeClient.Write(bRequest, 0, cbRequest);

                Console.WriteLine("Send {0} bytes to server: \"{1}\"",
                    cbRequest, message.TrimEnd('\0'));

                //
                // Receive a response from server.
                // 
                Console.Write("---------EXE Response START-----------");
                if (Program.RequestMessage == "")
                {
                    throw new Exception();
                }
                do
                {
                    byte[] bResponse = new byte[Program.BufferSize];
                    int cbResponse = bResponse.Length, cbRead;

                    cbRead = pipeClient.Read(bResponse, 0, cbResponse);

                    // Unicode-encode the received byte array and trim all the 
                    // '\0' characters at the end.
                    message = Encoding.Unicode.GetString(bResponse).TrimEnd('\0');
                    //Console.WriteLine("Receive {0} bytes from server: \"{1}\"",
                    //    cbRead, message);
                    retcode = Convert.ToInt32(message.Substring(message.IndexOf("###AAA###") + 9, message.IndexOf("###BBB###") - message.IndexOf("###AAA###") - 9));
                          
                }
                while (!pipeClient.IsMessageComplete);
                
                return (retcode);
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine("The client throws the error: {0}", ex.Message);
                return (-1);
            }
            finally
            {
                // Close the pipe.
                if (pipeClient != null)
                {
                    pipeClient.Close();
                    pipeClient = null;
                }
            }
        }
    }
}
