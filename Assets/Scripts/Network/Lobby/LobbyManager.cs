using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using Tank3DMultiplayer.Support;
using System.Collections.Generic;

namespace Tank3DMultiplayer.Network.LobbyManager
{
    public class LobbyManager : SingletonPersistent<LobbyManager>
    {
        

        Lobby m_CurrentLobby = null;

        public event EventHandler OnLeftLobby;

        public event EventHandler<LobbyEventArgs> OnJoinedLobby;
        public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
        public event EventHandler<LobbyEventArgs> OnKickedFromLobby;


        private float lobbyPollTimer;
        private float lobbyHeartBeatTimer;

        public class LobbyEventArgs : EventArgs
        {
            public Lobby lobby;
        }

        public Lobby CurrentLobby
        {
            get { return m_CurrentLobby; }
        }

        private void Update()
        {
            if (m_CurrentLobby == null)
                return;
            HandleLobbyPolling();
        }
        private async void HandleLobbyPolling()
        {
            if (m_CurrentLobby != null)
            {
                lobbyPollTimer -= Time.deltaTime;
                if (lobbyPollTimer < 0f)
                {
                    lobbyPollTimer = ConstValue.LOBBY_POLL_TIMER;

                    try
                    {
                        m_CurrentLobby = await LobbyService.Instance.GetLobbyAsync(m_CurrentLobby.Id);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }

                    OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = m_CurrentLobby });

                    if (!IsPlayerInLobby())
                    {
                        // Player was kicked out of this lobby
                        Debug.Log("Kicked from Lobby!");

                        OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = m_CurrentLobby });

                        m_CurrentLobby = null;
                    }
                }
            }
        }

        public async Task<Lobby> CreateLobbyAsync(string lobbyName, int maxPlayers, bool isPrivate)
        {
            Player player = GetPlayerInformation();

            CreateLobbyOptions options = new CreateLobbyOptions
            {
                Player = player,
                IsPrivate = isPrivate
            };
            try
            {
                m_CurrentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
                OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = m_CurrentLobby });
                return m_CurrentLobby;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Lobby Create failed:\n{ex}");
                return null;
            }
        }
        
        public async Task<Lobby> JoinLobbyByCodeAsync(string lobbyCode)
        {
            Player player = GetPlayerInformation();
            try
            {
                Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions
                {
                    Player = player
                });

                m_CurrentLobby = lobby;

                OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = m_CurrentLobby });
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

            return m_CurrentLobby;
        }

        public async void QuickJoinLobby()
        {
            try
            {
                QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

                Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
                m_CurrentLobby = lobby;

                OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async void LeaveLobbyAsync()
        {
            if (!InLobby() || m_CurrentLobby == null)
                return;
            try
            {
                string playerId = AuthenticationService.Instance.PlayerId;

                await LobbyService.Instance.RemovePlayerAsync(m_CurrentLobby.Id, playerId);
                Debug.Log("Leave");
                m_CurrentLobby = null;

                OnLeftLobby?.Invoke(this, EventArgs.Empty);
            }
            catch(Exception ex)
            {
                Debug.Log(ex);
            }

        }

        public bool InLobby()
        {
            if (m_CurrentLobby == null)
            {
                Debug.LogWarning("LobbyManager not currently in a lobby. Did you CreateLobbyAsync or JoinLobbyAsync?");
                return false;
            }

            return true;
        }

        public Player GetPlayerInformation()
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            return new Player(playerId, null, new Dictionary<string, PlayerDataObject>
            {
                {ConstValue.KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public,NameGenerator.GetName(playerId)) },
                {ConstValue.KEY_PLAYER_READY, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, ConstValue.KEY_VALUE_NOT_READY)}
            });
        }

        public async void KickPlayer(string playerId)
        {
            if (IsLobbyHost() && m_CurrentLobby != null)
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(m_CurrentLobby.Id, playerId);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
        }

        public bool IsLobbyHost()
        {
            return m_CurrentLobby != null && m_CurrentLobby.HostId == AuthenticationService.Instance.PlayerId;
        }

        private bool IsPlayerInLobby()
        {
            if (m_CurrentLobby != null && m_CurrentLobby.Players != null)
            {
                foreach (Player player in m_CurrentLobby.Players)
                {
                    if (player.Id == AuthenticationService.Instance.PlayerId)
                    {
                        // This player is in this lobby
                        return true;
                    }
                }
            }
            return false;
        }

        public async void UpdatePlayerReady(string isReady)
        {
            if (m_CurrentLobby != null)
            {
                try
                {
                    string playerId = AuthenticationService.Instance.PlayerId;
                    UpdatePlayerOptions options = new UpdatePlayerOptions();

                    options.Data = new Dictionary<string, PlayerDataObject>() {
                        {
                            ConstValue.KEY_PLAYER_NAME, new PlayerDataObject(
                                visibility: PlayerDataObject.VisibilityOptions.Public,
                                value: NameGenerator.GetName(playerId))
                        },
                        {
                            ConstValue.KEY_PLAYER_READY, new PlayerDataObject(
                                visibility: PlayerDataObject.VisibilityOptions.Member,
                                value: isReady)
                        }
                    };

                    

                    Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(m_CurrentLobby.Id, playerId, options);
                    m_CurrentLobby = lobby;

                    OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = m_CurrentLobby });
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
        }
    }
}

