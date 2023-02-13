using System.Collections;
using System.Collections.Generic;
using Tank3DMultiplayer;
using Tank3DMultiplayer.Data;
using UnityEngine;
using UnityEngine.Networking;

public class UploadDataToServer : MonoBehaviour
{
    public IEnumerator UpdateGoldForPlayer(string playerId, int gold)
    {
        UnityWebRequest req = UnityWebRequest.Get(ConstValue.LOAD_DATA_TANKS);
        var handler = req.SendWebRequest();


        yield return null;
    }
}
