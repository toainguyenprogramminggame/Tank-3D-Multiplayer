using UnityEngine;
using System.Collections.Generic;
using Tank3DMultiplayer;
using Tank3DMultiplayer.Support;
using Unity.Netcode;

public class PlayersManager : SingletonNetworkPersistent<PlayersManager>
{
    List<PlayerInGameData> playersData = new List<PlayerInGameData>();

    public List<PlayerInGameData> PlayersData { get => playersData;}

    public PlayerInGameData GetPlayerData(ulong clientId)
    {
        PlayerInGameData dataPlayer = playersData.Find(x => clientId == x.clientId);
        return dataPlayer;
    }

    public void Kill(ulong clientIdKill, ulong clientIdDead)
    {
        int indexKill = playersData.FindIndex(x => x.clientId == clientIdKill);
        int indexDead = playersData.FindIndex(x => x.clientId == clientIdDead);

        playersData[indexKill].Kill();
        playersData[indexDead].Dead();

        KillClientRpc(clientIdKill, clientIdDead);
    }

    [ClientRpc]
    private void KillClientRpc(ulong clientIdKill, ulong clientIdDead)
    {
        if (IsServer)
            return;
        int indexKill = playersData.FindIndex(x => x.clientId == clientIdKill);
        int indexDead = playersData.FindIndex(x => x.clientId == clientIdDead);

        playersData[indexKill].Kill();
        playersData[indexDead].Dead();

        GamePlayUI.Instance.UpdateKDA();
    }

    /// <summary>
    /// Just store data on HOST
    /// </summary>
    /// <param name="dataPlayer">Data of player who has join the game</param>
    public void PlayerJoinedRoom(PlayerInGameData dataPlayer)
    {
        playersData.Add(dataPlayer);
        PlayerJoinedRoomClientRpc(dataPlayer);
    }

    [ClientRpc]
    private void PlayerJoinedRoomClientRpc(PlayerInGameData dataPlayer)
    {
        if (IsServer)
            return;

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

        UpdateClientSelectTankClientRpc(clientId, tankType);
    }

    [ClientRpc]
    private void UpdateClientSelectTankClientRpc(ulong clientId, TankType tankType)
    {
        if (IsServer)
            return;
        int indexPlayerSelect = playersData.FindIndex(x => clientId == x.clientId);
        if (indexPlayerSelect == -1)
        {
            Debug.LogError("Can't not find player in server to select tank");
            return;
        }

        playersData[indexPlayerSelect].tankType = tankType;
    }
}
