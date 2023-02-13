using System;
using Tank3DMultiplayer;
using Unity.Netcode;

public class PlayerInGameData : INetworkSerializable
{
    public ulong clientId;             // The clientId who selected this character
    public string playerName;          // Name of the player
    public TankType tankType;      // Prefab of the tank to use on gameplay scene
    private int kill;               // Count kill
    private int dead;               // Count Dead

    public int GetKill { get => kill;}
    public int GetDead { get => dead;}

    public PlayerInGameData()
    {
        clientId = 0;
        tankType = TankType.Hulk;
        playerName = "";
        kill = 0;
        dead = 0;
    }

    public void Kill()
    {
        kill++;
    }

    public void Dead()
    {
        dead++;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref tankType);
        serializer.SerializeValue(ref kill);
        serializer.SerializeValue(ref dead);
    }

}
