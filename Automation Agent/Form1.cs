using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automation_Agent
{
    public partial class Form1 : Form
    {
        Worker worker = new Worker();
        System.Threading.Thread workerthread;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            workerthread = new System.Threading.Thread(worker.DoWork);
            workerthread.Start();
            // while (true)
           // {

                //if (args.Length > 0 && args[0] == "-native")
                //{
                // If the command line is "CSNamedPipeServer -native", 
                // P/Invoke the native pipe APIs to create the named pipe.
                //     NativeNamedPipeServer.Run();
                // }
                // else
                // {
                // Use the types in the System.IO.Pipes namespace to create 
                // the named pipe. This solution is recommended when you 
                // program against .NET Framework 3.5 or any newer releases 
                // of .NET Framework.
             //   SystemIONamedPipeServer.Run(textBox1);
                // }
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            lblstatus.Text = "STOPPING...";
            //this.Hide();
            worker.RequestStop();
            //

            
            Program.RequestMessage = "";
            SystemIONamedPipeClient.Run();
            workerthread.Abort();
            Application.Exit();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Automation Agent";
            notifyIcon1.BalloonTipText = "You have successfully minimized your form.";

            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            //this.Hide();
            lblstatus.Text = "STOPPING...";
            worker.RequestStop();
            //
            Program.RequestMessage = "";
            SystemIONamedPipeClient.Run();
            workerthread.Join();
            Application.Exit();
        }
    }
}
