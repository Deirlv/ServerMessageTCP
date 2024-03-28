using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Diagnostics;

namespace ServerTCP
{
    public partial class Form1 : Form
    {
        Thread thread;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (thread != null)
            {
                return;
            }
            thread = new Thread(ServerFunc);
            thread.IsBackground = true;
            thread.Start();
            Text = "Server is start!";
        }

        private void ServerFunc()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("192.168.178.34"), 11000);
            try
            {
                tcpListener.Start(5);
                do
                {
                    if(tcpListener.Pending())
                    {
                        TcpClient tcpClient = tcpListener.AcceptTcpClient();

                        byte[] buffer = new byte[1024];

                        NetworkStream ns = tcpClient.GetStream();
                        int len = ns.Read(buffer, 0, buffer.Length);

                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append($"{len} byte received from {tcpClient.Client.RemoteEndPoint}");

                        string message = Encoding.Default.GetString(buffer, 0, len);

                        if(message == "EXIT")
                        {
                            tcpClient.Client.Shutdown(SocketShutdown.Both);
                            tcpListener.Stop();
                            Environment.Exit(1);
                        }

                        stringBuilder.Append($"\n Message: {message}\n");
                        textBox1.BeginInvoke(new Action<string>(AddText), stringBuilder.ToString());
                        
                        tcpClient.Client.Shutdown(SocketShutdown.Receive);
                    }
                } while (true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        private void AddText(string obj)
        {
            StringBuilder sb = new StringBuilder(textBox1.Text);
            sb.Append(obj);
            textBox1.Text = sb.ToString();
        }
    }
}
