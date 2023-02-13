using System.Collections.Generic;
using Tank3DMultiplayer.Support;
using Tank3DMultiplayer.UI;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : Singleton<GamePlayUI>
{
    [SerializeField]
    private Text textCountDownTimeGamePlay;

    [Header("Respawn")]
    [SerializeField]
    private GameObject panelCountDownRespawn;

    [SerializeField]
    private Text textCountDownTimeRespawn;


    [Header("Result")]
    [SerializeField]
    private GameObject panelResult;

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private GameObject itemResultInGame;


    private void Start()
    {
        UpdateKDA();
    }

    public void ChangeStatusRespawn(bool isDead)
    {
        panelCountDownRespawn.SetActive(isDead);
    }

    private void Update()
    {

        float curTimeGamePlay = GamePlayManager.Instance.CurrentTimeGamePlay();
        if(curTimeGamePlay >= 0)
        {
            textCountDownTimeGamePlay.text = $" {(int)curTimeGamePlay}s";
        }

        float curTimeRespawn = GamePlayManager.Instance.CurrentTimeToRespawn();
        if(curTimeRespawn >= 0)
        {
            textCountDownTimeRespawn.text = $" {(int)curTimeRespawn}s\nTo Respawn";
        }
        if (Input.GetKey(KeyCode.G) && curTimeGamePlay < 0)
        {
            panelResult.SetActive(true);
        }
        else
        {
            panelResult.SetActive(false);
        }
    }

    public void UpdateKDA()
    {
        foreach(Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }

        foreach(PlayerInGameData plData in PlayersManager.Instance.PlayersData)
        {
            GameObject itemResult = Instantiate(itemResultInGame);

            itemResult.GetComponent<ItemResultUI>().SetupData(plData);
            itemResult.transform.SetParent(content.transform, false);
        }
    }
}
