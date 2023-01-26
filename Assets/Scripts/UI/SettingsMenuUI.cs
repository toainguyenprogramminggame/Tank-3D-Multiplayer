using UnityEngine.UI;

namespace Tank3DMultiplayer.UI
{
    public class SettingsMenuUI : UIPanelBase
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

