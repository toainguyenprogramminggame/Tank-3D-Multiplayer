using System;
using UnityEngine;

namespace Tank3DMultiplayer.Network.RelayManager
{
    public class RelayJoinData  
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] HostConnectionData;
        public byte[] Key;
    }
}

