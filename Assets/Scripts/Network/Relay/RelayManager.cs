using System.Threading.Tasks;
using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using Unity.Services.Authentication;
using Tank3DMultiplayer.Support;


namespace Tank3DMultiplayer.Network.RelayManager
{
    public class RelayManager : SingletonPersistent<RelayManager>
    {

        public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
        public bool IsRelayEnabled => Transport != null
                                    && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

        public async Task<RelayHostData> SetupRelay()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            try
            {
                Allocation allocation = await Relay.Instance.CreateAllocationAsync(ConstValue.MAX_RELAY_CONNECTION);

                RelayHostData relayHostData = new RelayHostData
                {
                    Key = allocation.Key,
                    Port = (ushort)allocation.RelayServer.Port,
                    AllocationID = allocation.AllocationId,
                    AllocationIDBytes = allocation.AllocationIdBytes,
                    IPv4Address = allocation.RelayServer.IpV4,
                    ConnectionData = allocation.ConnectionData
                };
                relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);

                await LobbyManager.LobbyManager.Instance.UpdateLobbyRelayJoinCode(relayHostData.JoinCode);

                Transport.SetHostRelayData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes,
                                relayHostData.Key, relayHostData.ConnectionData);
                return relayHostData;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return null;
        }

        public async Task<RelayJoinData> JoinRelay(string joinCode)
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            try
            {
                JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
                RelayJoinData relayJoinData = new RelayJoinData
                {
                    Key = allocation.Key,
                    Port = (ushort)allocation.RelayServer.Port,
                    AllocationID = allocation.AllocationId,
                    AllocationIDBytes = allocation.AllocationIdBytes,
                    ConnectionData = allocation.ConnectionData,
                    HostConnectionData = allocation.HostConnectionData,
                    IPv4Address = allocation.RelayServer.IpV4,
                    JoinCode = joinCode
                };

                Transport.SetClientRelayData(relayJoinData.IPv4Address, relayJoinData.Port, relayJoinData.AllocationIDBytes,
                    relayJoinData.Key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);

                return relayJoinData;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return null;
        }

    }
}

