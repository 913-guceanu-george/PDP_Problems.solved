using System.Net.Sockets;

namespace Futures.continuations
{
    public static class Connection
    {
        public static Task ConnectAsync(Socket socket, string url)
        {
            return Task.Factory.FromAsync(socket.BeginConnect(url, 80, null, null), socket.EndConnect).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine("Connection failed");
                    return;
                }
                Console.WriteLine("Connected");
            });
        }

        public static Task ReceiveAsync(Socket socket, byte[] buffer)
        {
            return Task.Factory.FromAsync(socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, null, null), socket.EndReceive);
        }


    }
}