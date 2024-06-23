using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

public class ScoreClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;

    private void Start()
    {
        ConnectToServer("127.0.0.1", 7777);
    }

    private void ConnectToServer(string ipAddress, int port)
    {
        client = new TcpClient();
        client.Connect(ipAddress, port);
        stream = client.GetStream();
    }

    public void AddScore(string playerName, int score)
    {
        string message = $"ADD|{playerName}|{score}";
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    public List<string> GetScores()
    {
        string message = "GET";
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);

        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        List<string> scores = new List<string>(response.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries));
        return scores;
    }

    private void OnApplicationQuit()
    {
        stream.Close();
        client.Close();
    }
}
