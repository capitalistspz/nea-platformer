using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using MonoGame.Extended.Collections;
using Serilog;
using server.entities;

namespace server.networking
{
    public class PlayerManager
    {
        private KeyedCollection<NetConnection, ServerPlayerEntity> _players;

        public PlayerManager()
        {
            _players = new KeyedCollection<NetConnection, ServerPlayerEntity>(e => e.Connection);
        }

        public void AddPlayer(ServerPlayerEntity playerEntity)
        {
            Log.Information("Player {@Player} joined", playerEntity.Name);
            _players.Add(playerEntity);
        }
        public ServerPlayerEntity GetPlayer(NetConnection connection)
        {
            return _players[connection];
        }

        public void RemovePlayer(NetConnection connection)
        {
            _players.Remove(_players[connection]);
        }
        
        public List<ServerPlayerEntity> GetPlayers(Predicate<ServerPlayerEntity> predicate)
        {
            return _players.Where(player => predicate(player)).ToList();
        }


    }
}