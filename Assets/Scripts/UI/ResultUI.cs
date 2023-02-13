
using System.Collections.Generic;
using Tank3DMultiplayer;
using Tank3DMultiplayer.SceneManage;
using Tank3DMultiplayer.UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    private Button buttonBackToRoom;

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private GameObject itemResultInGame;

    [SerializeField]
    private Text textReward;

    private void Start()
    {
        buttonBackToRoom.onClick.AddListener(delegate
        {
            BackToRoom();
        });
        UpdateKDA();
        UpdateReward();
    }

    private void UpdateKDA()
    {
        foreach (Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (PlayerInGameData plData in PlayersManager.Instance.PlayersData)
        {
            GameObject itemResult = Instantiate(itemResultInGame);

            itemResult.GetComponent<ItemResultUI>().SetupData(plData);
            itemResult.transform.SetParent(content.transform, false);
        }
    }

    private void UpdateReward()
    {
        //NetworkManager.Singleton.LocalClient;
        List<PlayerInGameData> playersData = PlayersManager.Instance.PlayersData;

        int index = playersData.FindIndex(x => x.clientId == NetworkManager.Singleton.LocalClientId);
        if(index == -1)
        {
            Debug.Log("CAN'T NOT FIND CLIENT TO UPDATE REWARD");
            return;
        }

        int positiveKill = playersData[index].GetKill - playersData[index].GetDead;
        if (positiveKill < 0)
            positiveKill = 0;

        int reward = positiveKill * ConstValue.REWARD_PER_KILL;
        textReward.text = $"+{reward}";
    }

    private void BackToRoom()
    {
        LoadSceneManager.Instance.LoadScene(Tank3DMultiplayer.SceneName.Lobby);
        MenuManager.Instance.OpenMenu(MenuName.RoomMenu);
    }
}
