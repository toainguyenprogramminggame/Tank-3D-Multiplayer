using Tank3DMultiplayer;
using Tank3DMultiplayer.Manager;
using Tank3DMultiplayer.Support;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SelectTankManager : SingletonNetwork<SelectTankManager>
{
    private NetworkVariable<float> countDownTimeSelectTank =
        new NetworkVariable<float>(ConstValue.TIME_TO_SELECT_TANK,
                                    NetworkVariableReadPermission.Everyone,
                                    NetworkVariableWritePermission.Server);
    public Text textCountDownTime;


    private void Update()
    {
        textCountDownTime.text = "  " + (int)countDownTimeSelectTank.Value + " s";

        if (!IsServer || countDownTimeSelectTank.Value < 0)
            return;

        CountDownTimeSelectTank();

        StartGame();
    }

    private void CountDownTimeSelectTank()
    {
        countDownTimeSelectTank.Value -= Time.deltaTime;
    }

    private void StartGame()
    {
        if(countDownTimeSelectTank.Value > 0 || !IsServer)
            return;
        GameManager.Instance.StartGamePlay();
    }


    [ServerRpc(RequireOwnership = false)]
    public void SelectTankServerRpc(TankType tankType, ServerRpcParams serverRpcParams = default)
    {
        PlayersManager.Instance.UpdateClientSelectTank(serverRpcParams.Receive.SenderClientId, tankType);
    }
}
