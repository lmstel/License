using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceLicense
{
    public class MyLogger
    {
        private readonly string logFilePath;

        public MyLogger(string filePath)
        {
            logFilePath = filePath;
        }

        public void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log to file: {ex.Message}");
            }
        }
    }
}
