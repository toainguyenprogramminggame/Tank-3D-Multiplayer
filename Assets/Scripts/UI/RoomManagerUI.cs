using System.Collections;
using Tank3DMultiplayer.Network.LobbyManager;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Tank3DMultiplayer.UI
{
    public class RoomManagerUI : UIPanelBase
    {
        [SerializeField] private GameObject playerInRoomUI;
        [SerializeField] private Transform containListPlayers;

        [SerializeField] private Text roomCodeText;
        [SerializeField] private Button leaveLobbyButton;

        [SerializeField] private Button buttonReady;
        [SerializeField] private Button buttonNotReady;

        private void Start()
        {
            leaveLobbyButton.onClick.AddListener(() =>
            {
                Manager.LeaveLobby();
            });

            buttonReady.onClick.AddListener(() =>
            {
                Manager.UpdatePlayerReady(ConstValue.KEY_VALUE_IS_READY);
                buttonReady.gameObject.SetActive(false);
                buttonNotReady.gameObject.SetActive(true);
                buttonNotReady.GetComponent<Button>().interactable = false;
                StartCoroutine(EnableButtonInteractionAfterSeconds(buttonNotReady.GetComponent<Button>()));
            });

            buttonNotReady.onClick.AddListener(() =>
            {
                Manager.UpdatePlayerReady(ConstValue.KEY_VALUE_NOT_READY);
                buttonNotReady.gameObject.SetActive(false);
                buttonReady.gameObject.SetActive(true);
                buttonReady.GetComponent<Button>().interactable = false;
                StartCoroutine(EnableButtonInteractionAfterSeconds(buttonReady.GetComponent<Button>()));
            });

            LobbyManager.Instance.OnJoinedLobby += UpdateLobbyUI_Event;
            LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobbyUI_Event;
            LobbyManager.Instance.OnLeftLobby += UpdateLeftLobbyUI_Event;
            LobbyManager.Instance.OnKickedFromLobby += UpdateLeftLobbyUI_Event;
        }

        private void UpdateLeftLobbyUI_Event(object sender, System.EventArgs e)
        {
            ClearLobbyUI();
            MenuManager.Instance.OpenMenu(MenuName.CreateAndJoinRoomMenu);
        }

        private void UpdateLobbyUI_Event(object sender, System.EventArgs e)
        {
            UpdateLobbyUI(LobbyManager.Instance.CurrentLobby);
        }

        private void UpdateLobbyUI(Lobby lobby)
        {
            if (lobby == null)
                return;
            roomCodeText.text = "Room Code: " + lobby.LobbyCode;

            ClearLobbyUI();

            foreach (Player player in lobby.Players)
            {
                Transform itemPlayerInRoom = Instantiate(playerInRoomUI.transform, containListPlayers);

                ItemPlayerInRoomUI itemPlayerInRoomUI = itemPlayerInRoom.GetComponent<ItemPlayerInRoomUI>();

                itemPlayerInRoomUI.UpdatePlayer(player);
                itemPlayerInRoomUI.SetKickPlayerButton(LobbyManager.Instance.IsLobbyHost() && player.Id != AuthenticationService.Instance.PlayerId);
            }
        }

        private void ClearLobbyUI()
        {
            foreach (Transform child in containListPlayers)
            {
                Destroy(child.gameObject);
            }
        }

        IEnumerator EnableButtonInteractionAfterSeconds(Button btn,float seconds = ConstValue.MIN_TIME_ENABLE)
        {
            yield return new WaitForSeconds(seconds);
            btn.interactable = true;
        }
    }

}
