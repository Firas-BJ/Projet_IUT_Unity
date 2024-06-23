using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ScoreServer
{
    private static TcpListener listener;
    private static SQLiteConnection dbConnection;

    static void Main(string[] args)
    {
        InitializeDatabase();
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

    private static void InitializeDatabase()
    {
        dbConnection = new SQLiteConnection("Data Source=scores.db;Version=3;");
        dbConnection.Open();
        string createTableQuery = "CREATE TABLE IF NOT EXISTS Scores (Id INTEGER PRIMARY KEY AUTOINCREMENT, PlayerName TEXT, Score INTEGER)";
        SQLiteCommand command = new SQLiteCommand(createTableQuery, dbConnection);
        command.ExecuteNonQuery();
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
                AddScore(playerName, score);
            }
            else if (command == "GET")
            {
                SendScores(stream);
            }
        }

        client.Close();
    }

    private static void AddScore(string playerName, int score)
    {
        string insertQuery = "INSERT INTO Scores (PlayerName, Score) VALUES (@PlayerName, @Score)";
        SQLiteCommand command = new SQLiteCommand(insertQuery, dbConnection);
        command.Parameters.AddWithValue("@PlayerName", playerName);
        command.Parameters.AddWithValue("@Score", score);
        command.ExecuteNonQuery();
    }

    private static void SendScores(NetworkStream stream)
    {
        string selectQuery = "SELECT PlayerName, Score FROM Scores ORDER BY Score DESC LIMIT 10";
        SQLiteCommand command = new SQLiteCommand(selectQuery, dbConnection);
        SQLiteDataReader reader = command.ExecuteReader();
        StringBuilder sb = new StringBuilder();

        while (reader.Read())
        {
            sb.AppendLine($"{reader["PlayerName"]}:{reader["Score"]}");
        }

        byte[] data = Encoding.ASCII.GetBytes(sb.ToString());
        stream.Write(data, 0, data.Length);
    }
}
