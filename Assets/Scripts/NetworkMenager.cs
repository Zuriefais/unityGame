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
    List<SaveDate.PlayersPositions> serverSave;
    SaveDate.PlayerData playerSave;
    PlayerParams newPlayerParams;

    public override void Start()
    {
        base.Start();
        if (SaveMenager.Load<List<SaveDate.PlayersPositions>>("playersPositions.json") == null)
        {
            List<SaveDate.PlayersPositions> newServerSave = new();
            SaveMenager.Save(newServerSave, "playersPositions.json");
        }
    }

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

    [Obsolete]
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, PlayerParams message)
    {
        serverSave = SaveMenager.Load<List<SaveDate.PlayersPositions>>("playersPositions.json");
        if (serverSave == null)
        {
            serverSave = new();
        }
        int _listLengh = 1;
        int listLengh = serverSave.Count;
        if (listLengh > 0)
        {
            foreach (var player in serverSave)
                {
                   if (player.playerName == message.name)
                      {
                        SpawnWithSave(conn, player);
                      }
                        else
                        {
                            if(listLengh == _listLengh)
                            {
                                SpawnWithNotSave(message, conn);
                            }
                            _listLengh++;
                        }
                    }
        }
        else
        {
            SpawnWithNotSave(message, conn);
        }
    }

    private void SpawnWithSave(NetworkConnectionToClient conn, SaveDate.PlayersPositions player)
    {
        GameObject gameobject;
        gameobject = Instantiate(playerPrefab, new Vector2(player.playerPosition.x, player.playerPosition.y), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }

    private void SpawnWithNotSave(PlayerParams message, NetworkConnectionToClient conn)
    {
        GameObject gameobject;
        SaveDate.PlayersPositions newSave = new();
        newSave.playerName = message.name;
        newSave.playerPosition = new(0, 0);
        serverSave.Add(newSave);
        SaveMenager.Save(serverSave, "playersPositions.json");
        gameobject = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
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
