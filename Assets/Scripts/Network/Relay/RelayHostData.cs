using System;
using UnityEngine;

namespace Tank3DMultiplayer.Network.RelayManager
{
    public class RelayHostData : MonoBehaviour
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] Key;
    }
}

