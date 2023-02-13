using Tank3DMultiplayer;
using Tank3DMultiplayer.SceneManage;
using Tank3DMultiplayer.Support;
using Unity.Netcode;
using UnityEngine;

public class GamePlayManager : SingletonNetwork<GamePlayManager>
{
    NetworkVariable<float> countDownTimeGamePlay = new NetworkVariable<float>(ConstValue.TIME_REMAIN_GAMEPLAY, NetworkVariableReadPermission.Everyone,
                                                                NetworkVariableWritePermission.Server);
    private bool isDead = false;

    private float countDownTimeToRespawn;

    public void ChangeStatus(bool isDead)
    {
        this.isDead = isDead;
        if (this.isDead)
            countDownTimeToRespawn = ConstValue.TIME_TO_RESPAWN;
        else
            countDownTimeToRespawn = -1f;

        GamePlayUI.Instance.ChangeStatusRespawn(isDead);
    }

    public float CurrentTimeGamePlay()
    {
        return countDownTimeGamePlay.Value;
    }

    public float CurrentTimeToRespawn()
    {
        return countDownTimeToRespawn;
    }

    private void Start()
    {
        countDownTimeGamePlay.Value = ConstValue.TIME_REMAIN_GAMEPLAY;
    }

    private void Update()
    {
        if(isDead)
        {
            if(countDownTimeToRespawn >= 0)
            {
                countDownTimeToRespawn -= Time.deltaTime;
            }else
            {
                RespawnTankServerRpc(NetworkManager.Singleton.LocalClientId);
                isDead = false;
                GamePlayUI.Instance.ChangeStatusRespawn(isDead);
            }
        }

        if (!IsServer)
            return;
        countDownTimeGamePlay.Value -= Time.deltaTime;
        if(countDownTimeGamePlay.Value <= 0)
        {
            // Go to next scene
            LoadSceneManager.Instance.LoadScene(SceneName.Result);
        }
    }

    [ServerRpc(RequireOwnership =false)]
    public void RespawnTankServerRpc(ulong clientId)
    {
        Debug.Log("Start Respawn");
        if (!IsServer)
            return;
        Debug.Log("Respawn");

        StartCoroutine(SpawnManager.Instance.SpawnTanks(clientId));
    }

    //private struct CountTimeRespawn :INetworkSerializable, IEquatable<CountTimeRespawn>
    //{
    //    public float countDownTimeRespawn;
    //    public ulong clientId;

    //    public CountTimeRespawn(ulong clientId)
    //    {
    //        this.clientId = clientId;
    //        countDownTimeRespawn= 0;
    //    }

    //    public bool Equals(CountTimeRespawn other)
    //    {
    //        return other.clientId == this.clientId;
    //    }

    //    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    //    {
    //        serializer.SerializeValue(ref countDownTimeRespawn);
    //        serializer.SerializeValue(ref clientId);
    //    }
    //}
}
