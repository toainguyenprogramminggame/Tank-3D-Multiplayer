#if UNITY_EDITOR
using ParrelSync;
#endif
using System.Threading.Tasks;
using Tank3DMultiplayer.Network.Auth;
using Tank3DMultiplayer.Support;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Tank3DMultiplayer.Network.LobbyManager;
using Tank3DMultiplayer.UI;

namespace Tank3DMultiplayer.Manager
{
    public class GameManager : SingletonPersistent<GameManager>
    {
        async void Start()
        {
            await InitializeServices();
        }

        async Task InitializeServices()
        {
            string serviceProfileName = "player";

#if UNITY_EDITOR
            serviceProfileName = $"{serviceProfileName}_{ClonesManager.GetCurrentProject().name}";
#endif
            await Auth.Authenticate(serviceProfileName);
        }

        public async Task<Lobby> CreateLobby(string name, bool isPrivate, int maxPlayers = ConstValue.MAX_PLAYERS)
        {
            if(name.Length <= 4)
            {
                Debug.LogWarning("Room code must longer than 4 character");
                return null;
            }
            Lobby lobby = await LobbyManager.Instance.CreateLobbyAsync(name, maxPlayers, isPrivate);

            if(lobby != null)
            {
                MenuManager.Instance.OpenMenu(MenuName.RoomMenu);
            }
            return lobby;
        }

        public async Task<Lobby> JoinLobby(string lobbyCode)
        {
            Lobby lobby = await LobbyManager.Instance.JoinLobbyByCodeAsync(lobbyCode);
            MenuManager.Instance.OpenMenu(MenuName.RoomMenu);
            return lobby;
        }

        public void LeaveLobby()
        {
            LobbyManager.Instance.LeaveLobbyAsync();
        }

        public void KickPlayer(string playerId)
        {
            LobbyManager.Instance.KickPlayer(playerId);
        }

        public void UpdatePlayerReady(string isReady)
        {
            LobbyManager.Instance.UpdatePlayerReady(isReady);
        }

    }
}

