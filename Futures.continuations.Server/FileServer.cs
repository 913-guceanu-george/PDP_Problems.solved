using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Futures.continuations.Server
{
    class FileServer
    {
        public static void Run()
        {
            StartServer();
        }

        static void StartServer()
        {
            TcpListener server = new(IPAddress.Any, 8080);

            try
            {
                server.Start();
                Console.WriteLine("Server started. Waiting for connections...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Task.Run(() => HandleClient(client));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                server.Stop();
            }
        }

        static void HandleClient(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            StreamReader reader = new(stream, Encoding.ASCII);
            StreamWriter writer = new(stream, Encoding.ASCII) { AutoFlush = true };

            // Send list of available files to the client
            string[] availableFiles = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Files"));
            for (int i = 0; i < availableFiles.Length; i++)
            {
                availableFiles[i] = availableFiles[i].Replace(Path.Combine(Environment.CurrentDirectory, "Files"), "");
            }
            string fileList = string.Join(", ", availableFiles);
            writer.WriteLine($"Available files: {fileList}");

            // Receive the selected file name from the client
            string requestedFile = reader.ReadLine()!;
            Console.WriteLine($"Client requested file: {requestedFile}");

            SendFile(requestedFile, writer);
            client.Close();
        }

        static void SendFile(string fileName, StreamWriter writer)
        {
            try
            {
                string fullPath = Path.Combine(Environment.CurrentDirectory, "Files", fileName);
                Console.WriteLine($"Sending file: {fullPath}");

                if (File.Exists(fullPath))
                {
                    byte[] fileData = File.ReadAllBytes(fullPath);

                    // Send HTTP response header
                    writer.WriteLine("HTTP/1.1 200 OK");
                    writer.WriteLine($"Content-Length: {fileData.Length}");
                    // writer.WriteLine("Content-Type: application/octet-stream");
                    writer.WriteLine();

                    // Send file data
                    writer.BaseStream.Write(fileData, 0, fileData.Length);
                    Console.WriteLine("File sent successfully.");
                }
                else
                {
                    // Send 404 Not Found response
                    writer.WriteLine("HTTP/1.1 404 Not Found");
                    writer.WriteLine("Content-Type: text/plain");
                    writer.WriteLine("Content-Length: 13");
                    writer.WriteLine();
                    writer.WriteLine("File not found");
                    Console.WriteLine("File not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
