using System.IO.Pipes;
using System.Text;

namespace BasedLauncher;

/// <summary>
/// This is the unity script, you want to put this in awake and attach it to a gameobject in the first scene
/// </summary>
public class unity_script
{
    private async void Awake()
    {
        using (var pipeClient = new NamedPipeClientStream(".", "MyAppPipe", PipeDirection.InOut))
        {
            Console.WriteLine("Connecting to server...");
            await pipeClient.ConnectAsync();

            using (var streamWriter = new StreamWriter(pipeClient, Encoding.UTF8))
            {
                await streamWriter.WriteLineAsync("Unity ready");
                await streamWriter.FlushAsync();
            }
        }
    }
}