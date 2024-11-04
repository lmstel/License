using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Newtonsoft.Json;

namespace WindowsServiceLicense
{
    public partial class InitService : ServiceBase
    {
        private System.Timers.Timer _timer;
        private int N = 0;
        static string logFilePath = @"c:\Users\michael_l.MANAGIX\source\repos\WindowsServiceLicense\MyService1.log"; // Adjust the path as needed
        static MyLogger logger = new MyLogger(logFilePath);

        private TcpListener _tcpListener;
        private System.Threading.Thread _listenerThread;

        public InitService()
        {
            InitializeComponent();
            logger.Log("Service started successfully.");
        }

        
         public static void GetData()
         {
                logger.Log("Static GetData...Received Call");
         }
       

        protected override void OnStart(string[] args)
        {
            _listenerThread = new Thread(StartListener) { IsBackground = true };
            _listenerThread.Start();

            _timer = new System.Timers.Timer(10000); // Call every 10 seconds
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
            
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
            logger.Log("Service stopped.");
            _tcpListener?.Stop();
            _listenerThread?.Join();


        }

        private void StartListener()
        {
            try
            {
                // Define the TCP listener on the specified port
                int port = 5000;
                _tcpListener = new TcpListener(IPAddress.Any, port);
                _tcpListener.Start();
                logger.Log($"Service started. Listening on port {port}...");

                while (true)
                {
                    // Wait for a client connection
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    logger.Log("Client connected.");

                    NetworkStream stream = client.GetStream();
                    //byte[] buffer = new byte[1024];
                    
                    //int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    //logger.Log("after read");

                    //string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
    
 


                    // Handle the client in a new thread

                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Log($"An error occurred: {ex.Message}");
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;
            
            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    String messageJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    logger.Log($"Received message: {messageJson}");

                    Person person = JsonConvert.DeserializeObject<Person>(messageJson);

                    logger.Log($"Received Person: Name={person.Name}, Age={person.Age}");

                    // Echo message back to client
                    byte[] response = Encoding.UTF8.GetBytes($"Message received.Received Person: Name={person.Name}, Age={person.Age}");
                    stream.Write(response, 0, response.Length);
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Client disconnected with error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
                   
            logger.Log("OnTimedEvent..." + N++);
        }
    }
}
