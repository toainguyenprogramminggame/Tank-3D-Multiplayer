using UnityEngine;
using System.Collections.Generic;
using Tank3DMultiplayer;
using Tank3DMultiplayer.Support;

public class PlayersManager : SingletonPersistent<PlayersManager>
{
    List<PlayerInGameData> playersData = new List<PlayerInGameData>();

    public List<PlayerInGameData> PlayersData { get => playersData;}

    public PlayerInGameData GetPlayerData(ulong clientId)
    {
        PlayerInGameData dataPlayer = playersData.Find(x => clientId == x.clientId);
        return dataPlayer;
    }


    /// <summary>
    /// Just store data on HOST
    /// </summary>
    /// <param name="dataPlayer">Data of player who has join the game</param>
    public void PlayerJoinedRoom(PlayerInGameData dataPlayer)
    {
        playersData.Add(dataPlayer);
    }

    public void UpdateClientSelectTank(ulong clientId, TankType tankType)
    {
        int indexPlayerSelect = playersData.FindIndex(x => clientId == x.clientId);
        if(indexPlayerSelect == -1)
        {
            Debug.LogError("Can't not find player in server to select tank");
            return;
        }

        playersData[indexPlayerSelect].tankType =  tankType;
    }
}
