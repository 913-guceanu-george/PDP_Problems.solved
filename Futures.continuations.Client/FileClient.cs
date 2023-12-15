using System.Net.Sockets;
using System.Text;
namespace Futures.continuations.Client
{
    class FileClient
    {
        public static void Run()
        {
            StartClient();
        }

        static void StartClient()
        {
            using TcpClient client = new("127.0.0.1", 8080);
            using NetworkStream stream = client.GetStream();
            StreamReader reader = new(stream, Encoding.ASCII);
            StreamWriter writer = new(stream, Encoding.ASCII) { AutoFlush = true };

            // Receive the list of available files from the server
            string fileList = reader.ReadLine()!;
            Console.WriteLine($"Available files: {fileList}");

            // Prompt the user to input the file name to download
            Console.Write("Enter the file name to download: ");
            string requestedFile = Console.ReadLine()!;

            // Send the selected file name to the server
            writer.WriteLine(requestedFile);

            // Read the response header
            string response = reader.ReadLine()!;
            Console.WriteLine($"Server Response:\n{response}");

            // Check if the response is successful (status code 200)
            if (response.StartsWith("HTTP/1.1 200 OK"))
            {
                // Extract content length from the header
                int contentLength = GetContentLength(reader);

                // Read the file data
                byte[] fileData = new byte[contentLength];
                stream.Read(fileData, 0, contentLength);

                // Save the file to the local disk
                string filePath = $"{requestedFile}";
                File.WriteAllBytes(filePath, fileData);
                Console.WriteLine($"File saved to: {filePath}");
            }
            else
            {
                Console.WriteLine("Error: File not found or server error.");
            }
        }

        static int GetContentLength(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()!) != null)
            {
                if (line.StartsWith("Content-Length:"))
                {
                    return int.Parse(line.Split(':')[1].Trim());
                }
            }

            throw new InvalidOperationException("Content-Length not found in the response header.");
        }
    }
}
