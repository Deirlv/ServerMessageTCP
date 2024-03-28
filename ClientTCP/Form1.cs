using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientTCP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Process.Start("ServerTCP.exe");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect("192.168.178.34", 11000);

                NetworkStream stream = tcpClient.GetStream();
                stream.Write(Encoding.Default.GetBytes(textBox1.Text));

                textBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                tcpClient.Client.Close();
            }
            
        }
    }
}
