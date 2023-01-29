using System.Collections;
using UnityEngine;
using Tank3DMultiplayer.Data;
using UnityEngine.UI;
using Tank3DMultiplayer.UI;

namespace Tank3DMultiplayer.SceneManage.Lobby
{
    public class ListTankMenuUI : UIPanelBase
    {
        public Button m_ButtonBackToMain;

        private void Start()
        {
            m_ButtonBackToMain.onClick.AddListener(delegate
            {
                MenuManager.Instance.OpenMenu(MenuName.MainMenu);
            });

        }
    }
}

