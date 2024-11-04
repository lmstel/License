using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using MySharedLibrary;

namespace ConsoleTestLicense
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverIp = "127.0.0.1";
            int port = 5000;

            Person person = new Person { Name = "John Doe", Age = 30 };

            try
            {
                using (TcpClient client = new TcpClient(serverIp, port))
                {

                    //  while (true)
                    {
                        //string message = Console.ReadLine();
                        //if (string.IsNullOrEmpty(message)) break;

                        NetworkStream stream = client.GetStream();

                        string jsonData = JsonSerializer.Serialize(person);
                        byte[] data = Encoding.UTF8.GetBytes(jsonData);


                        //byte[] data = Encoding.UTF8.GetBytes(message);
                        Console.WriteLine("Send  " + jsonData);
                        stream.Write(data, 0, data.Length);

                        // Receive response
                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Console.WriteLine("Server response: " + response);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
