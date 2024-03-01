using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BasedLauncher
{
    public class Program
    {
        
        static async Task Main(string[] args)
        {
            Process process = StartProcessMinimizedAndMaximize(File.ReadAllText("path.txt"));
            using (var pipeServer = new NamedPipeServerStream("MyAppPipe", PipeDirection.InOut))
            {
                Console.WriteLine("Waiting for client connection...");
                await pipeServer.WaitForConnectionAsync();
                Console.WriteLine("Client connected.");

                using (var streamReader = new StreamReader(pipeServer, Encoding.UTF8))
                {
                    string message = await streamReader.ReadLineAsync();
                    Console.WriteLine($"Message from client: {message}");
                    if (message == "Unity ready")
                    {
                        if (!ShowWindowAsync(process.MainWindowHandle, SW_SHOWMAXIMIZED))
                        {
                            Console.WriteLine("Failed to maximize process window.");
                        }
                    }
                }
            }
        }

        public static Process StartProcessMinimizedAndMaximize(string filePath)
        {
            // Configure the process start to be minimized
            var startInfo = new ProcessStartInfo(filePath)
            {
                WindowStyle = ProcessWindowStyle.Minimized,
                UseShellExecute = true
            };

            // Start the process
            var process = Process.Start(startInfo);
            

            return process;
        }

        
        
        
        //Silly
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWMAXIMIZED = 3; // Command to maximize the window

        
    }
}