using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Agent
{
    class Worker
    {
        public void DoWork()
        {
            while (!_shouldStop)
            {
                SystemIONamedPipeServer.Run();
            }
           // Console.WriteLine("worker thread: terminating gracefully.");
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }
        public void RequestStart()
        {
            _shouldStop = false;
            DoWork();
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop;
    }
}
