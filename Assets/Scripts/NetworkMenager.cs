using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct PlayerParams: NetworkMessage
{
    public string name;
}

public class CustomNetworkMenager : NetworkManager
{
    public bool isConnected;
    NetworkConnection connection;
    List<SaveDate.PlayersPositions> ServerSave;
    SaveDate.PlayerData playerSave;
    PlayerParams newPlayerParams;

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<PlayerParams>(OnCreateCharacter);
    }

    [Obsolete]
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect();
        connection = conn;
        isConnected = true;
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, PlayerParams message)
    {
        ServerSave = SaveMenager.Load<List<SaveDate.PlayersPositions>>("playersPositions.json");
        GameObject gameobject = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
        foreach (var player in ServerSave)
        {
            if (player.playerName == message.name)
            {
                gameobject = Instantiate(playerPrefab, new Vector2(player.playerPosition.x, player.playerPosition.y), Quaternion.identity);
            } 
        }
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }

    private void Update()
    {
         if (isConnected)
        {
            playerSave = SaveMenager.Load<SaveDate.PlayerData>("playerSave.json");
            newPlayerParams = new();
            newPlayerParams.name = playerSave.playerName;
            connection.Send(newPlayerParams);
            isConnected = false;
        }
    }
}
