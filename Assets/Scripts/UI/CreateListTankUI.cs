using System.Collections;
using Tank3DMultiplayer.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Tank3DMultiplayer.UI
{
    public class CreateListTankUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject itemTank;

        [SerializeField]
        private Transform listTanks;


        private void Start()
        {
            StartCoroutine(CreateListTanksUI());
        }
        IEnumerator CreateListTanksUI()
        {
            yield return new WaitUntil(() => DataTanks.Instance != null);

            foreach (var tank in DataTanks.Instance.ListDataTanks)
            {
                TankType tankType = tank.TankType;
                string tankName = tank.Name;
                Sprite tankAvatar = DataTanks.Instance.GetAvatarOfTank(tankType);

                GameObject tankObject = Instantiate(itemTank, listTanks);
                tankObject.transform.GetChild(0).GetComponent<Image>().sprite = tankAvatar;
                tankObject.transform.GetChild(1).GetComponent<Text>().text = tankName;
                tankObject.GetComponent<ItemTankUI>().TankType = tankType;
            }

            yield return null;
        }
    }
}

