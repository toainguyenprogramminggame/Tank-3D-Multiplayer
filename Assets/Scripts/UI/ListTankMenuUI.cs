using System.Collections;
using UnityEngine;
using Tank3DMultiplayer.Data;
using UnityEngine.UI;
using Tank3DMultiplayer.UI;

namespace Tank3DMultiplayer.SceneManage.Lobby
{
    public class ListTankMenuUI : UIPanelBase
    {
        [SerializeField]
        private GameObject itemTank;

        [SerializeField]
        private Transform listTanks;

        public Button m_ButtonBackToMain;

        private void Start()
        {
            m_ButtonBackToMain.onClick.AddListener(delegate
            {
                MenuManager.Instance.OpenMenu(MenuName.MainMenu);
            });

            StartCoroutine(CreateListTanks());
        }
        IEnumerator CreateListTanks()
        {
            yield return new WaitUntil(() => DataTanks.Instance != null);

            foreach(var tank in DataTanks.Instance.ListDataTanks)
            {
                TankType tankType = tank.TankType;
                string tankName = tank.Name;
                Sprite tankAvatar = DataTanks.Instance.GetAvatarOfTank(tankType);

                GameObject tankObject = Instantiate(itemTank,listTanks);
                tankObject.transform.GetChild(0).GetComponent<Image>().sprite = tankAvatar;
                tankObject.transform.GetChild(1).GetComponent<Text>().text = tankName;
            }


            yield return null;
        }
    }
}

