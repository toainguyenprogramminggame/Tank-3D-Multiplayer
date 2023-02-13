using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemResultUI : MonoBehaviour
{
    public Text textName;
    public Text textKill;
    public Text textDead;

    public void SetupData(PlayerInGameData data)
    {
        textName.text = data.playerName;
        textKill.text = data.GetKill.ToString();
        textDead.text = data.GetDead.ToString();
    }
}
