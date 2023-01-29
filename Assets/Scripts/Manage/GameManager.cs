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
using System.Collections.Generic;
using Tank3DMultiplayer.Network.RelayManager;
using Unity.Netcode;
using Tank3DMultiplayer.SceneManage;
using Unity.Services.Authentication;

namespace Tank3DMultiplayer.Manager
{
    public class GameManager : SingletonNetworkPersistent<GameManager>
    {
        [HideInInspector]
        public bool readyStartGame = false;

        async void Start()
        {
            await InitializeServices();
            NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
            {
                if(NetworkManager.Singleton.IsServer)
                {
                }
            };
        }


        private void Update()
        {
            if (readyStartGame)
                return;
            readyStartGame = IsReadyStartGame();
            if (readyStartGame)
            {
                StartNetworkGame();
            }
        }

        private async void StartNetworkGame()
        {
            if (LobbyManager.Instance.IsLobbyHost())
            {
                await RelayManager.Instance.SetupRelay();
                NetworkManager.Singleton.StartHost();
                LoadSceneManager.Instance.LoadScene(SceneName.CharacterSelection);
            }
            else
            {
                await AwaitRelayCode();

                string joinCode = LobbyManager.Instance.CurrentLobby.Data[ConstValue.KEY_RELAY_JOIN_CODE].Value;
                await RelayManager.Instance.JoinRelay(joinCode);
                Debug.Log(joinCode);
                NetworkManager.Singleton.StartClient();
            }
        }

        async Task AwaitRelayCode()
        {
            while(LobbyManager.Instance.CurrentLobby == null)
            {
                await Task.Delay(100);
            }

            while (string.IsNullOrEmpty(LobbyManager.Instance.CurrentLobby.Data[ConstValue.KEY_RELAY_JOIN_CODE].Value))
            {
                await Task.Delay(100);
            }
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
                Debug.LogWarning("Room name must longer than 4 character");
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

        public async Task UpdatePlayerReadyAsync(string isReady)
        {
            await LobbyManager.Instance.UpdatePlayerReady(isReady);
        }

        bool IsReadyStartGame()
        {
            if (LobbyManager.Instance == null)
            {
                return false;
            }
            Lobby lobby = LobbyManager.Instance.CurrentLobby;

            if(lobby == null)
            {
                return false;
            }
            else
            {
                    List<Player> players = lobby.Players;
                    //if (players.Count <= 1)
                    //    return false;
                    foreach (Player player in players)
                    {
                        if (player.Data[ConstValue.KEY_PLAYER_READY].Value == ConstValue.KEY_VALUE_NOT_READY)
                        {
                            return false;
                        }
                    }
            }
            return true;
        }


        #region IN GAME
        public void SelectTank(TankType tankType)
        {
            SelectTankManager.Instance.SelectTankServerRpc(tankType);
        }

        public void OnPlayerJoinedGame(ulong clientId)
        {
            if (!IsServer)
                return;
            PlayerInGameData plData = new PlayerInGameData();
            plData.clientId = clientId;
            plData.playerName = NameGenerator.GetName(AuthenticationService.Instance.PlayerId);
            
            PlayersManager.Instance.PlayerJoinedRoom(plData);
        }

        public void StartGamePlay()
        {
            if(!IsServer)
                return;
            LoadSceneManager.Instance.LoadScene(SceneName.GamePlay);
        }

        public void OnGamePlayLoaded()
        {
            SpawnTanks();
            InitPoolObject();
        }

        public void SpawnTanks()
        {
            StartCoroutine(SpawnManager.Instance.SpawnTanks());
        }

        public void InitPoolObject()
        {
            StartCoroutine( SpawnManager.Instance.SpawnObjectInGame());
        }
        #endregion
    }
}

