using System.Net;
using System.Net.Sockets;
using System.Text;

public class SessionAwait
{
    static TcpListener? server;
    public static void Run()
    {

        try
        {
            // Set the TcpListener on a specific port (e.g., 8888).
            int port = 8888;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            server = new TcpListener(ipAddress, port);

            // Start listening for client requests.
            server.Start();

            Console.WriteLine($"Server is listening on port {port}");

            // Accept the TcpClient connection.
            TcpClient client = server.AcceptTcpClient();
            IPAddress clientAddress = (client.Client.RemoteEndPoint as IPEndPoint)!.Address;
            Console.WriteLine($"Client connected: {clientAddress}");

            // Get the client stream.
            NetworkStream stream = client.GetStream();

            // Send a message to the client.
            string message = "Hello, client! How are you?";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);

            // Clean up.
            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        finally
        {
            server?.Stop();
        }
    }
}
