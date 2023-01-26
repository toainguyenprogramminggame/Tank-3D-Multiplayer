using System.Collections;
using System.Collections.Generic;
using Tank3DMultiplayer;
using Tank3DMultiplayer.Network.LobbyManager;
using Tank3DMultiplayer.UI;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class ItemPlayerInRoomUI : UIPanelBase
{
    [SerializeField] Button kickPlayerButton;
    [SerializeField] Text playerReadyText;
    [SerializeField] Text playerNameText;

    private Player player;


    private void Start()
    {
        kickPlayerButton.onClick.AddListener(delegate
        {
            Manager.KickPlayer(player.Id);
        });
    }


    public void SetKickPlayerButton(bool visible)
    {
        kickPlayerButton.gameObject.SetActive(visible);
    }

    public void UpdatePlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.Data[ConstValue.KEY_PLAYER_NAME].Value;
        
        if(player.Id == AuthenticationService.Instance.PlayerId)
        {
            GetComponent<Outline>().enabled = true;
        }

        string isReady = player.Data[ConstValue.KEY_PLAYER_READY].Value;
        if (isReady == ConstValue.KEY_VALUE_IS_READY)
            playerReadyText.gameObject.SetActive(true);
        else
            playerReadyText.gameObject.SetActive(false);
    }

    public void UpdatePlayerReady(bool isReady)
    {
        playerReadyText.gameObject.SetActive(isReady);
    }
}
