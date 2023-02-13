using UnityEngine;
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

        public void SelectTank(TankType type)
        {
            (transform.parent.GetComponentInChildren(typeof(DetailTankMenuUI), true) as DetailTankMenuUI).SelectTank(type);
            MenuManager.Instance.OpenMenu(MenuName.DetailTankMenu);
        }
    }
}

