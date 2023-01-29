
using Tank3DMultiplayer;
using Unity.Netcode;

public class PlayerInGameData : INetworkSerializable
{
    public ulong clientId;             // The clientId who selected this character
    public string playerName;          // Name of the player
    public TankType tankType;      // Prefab of the tank to use on gameplay scene

    public PlayerInGameData()
    {
        clientId = 0;
        tankType = TankType.Null;
        playerName = "";
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref tankType);
    }
}
