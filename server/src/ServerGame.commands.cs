using System;
using System.Linq;
using Serilog;

namespace server
{
    public partial class ServerGame
    {
        private void CommandKickPlayer()
        {
            Console.WriteLine("Enter a player name: ");
            var username = Console.ReadLine();
            var player = _playerManager.GetPlayers(player => player.Name == username)[0];
            if (player != null)
            {
                player.Connection.Disconnect("Kicked.");
                Log.Information("Disconnected player {@Player}", username);
            }
            else
            {
                Log.Information("Player {@Player} is not connected", username);
            }
        }
        

        private void CommandGetPlayers()
        {
            // Gets list of all players
            var players = _playerManager.GetPlayers(_ => true);
            var output = players.Aggregate("", (current, player) => current + $"{player.Name}@{player.Connection.RemoteEndPoint}");
            Log.Information("Players: {@Players}",output);
        }

        private void CommandKickAllPlayers()
        {
            var players = _playerManager.GetPlayers(_ => true);
            foreach (var player in players)
            {
                player.Connection.Disconnect("Kicked.");
            }
        }
    }
}