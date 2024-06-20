using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ScoreServer
{
    private static Dictionary<string, int> playerScores = new Dictionary<string, int>();
    private static TcpListener listener;

    static void Main(string[] args)
    {
        listener = new TcpListener(IPAddress.Any, 7777);
        listener.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            string[] parts = message.Split('|');
            string command = parts[0];

            if (command == "ADD")
            {
                string playerName = parts[1];
                int score = int.Parse(parts[2]);
                if (playerScores.ContainsKey(playerName))
                {
                    playerScores[playerName] += score;
                }
                else
                {
                    playerScores[playerName] = score;
                }
                BroadcastScores();
            }
            else if (command == "GET")
            {
                SendScores(stream);
            }
        }

        client.Close();
    }

    private static void BroadcastScores()
    {
        foreach (var player in playerScores)
        {
            Console.WriteLine($"{player.Key}: {player.Value}");
        }
    }

    private static void SendScores(NetworkStream stream)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var player in playerScores)
        {
            sb.AppendLine($"{player.Key}: {player.Value}");
        }
        byte[] data = Encoding.ASCII.GetBytes(sb.ToString());
        stream.Write(data, 0, data.Length);
    }
}
