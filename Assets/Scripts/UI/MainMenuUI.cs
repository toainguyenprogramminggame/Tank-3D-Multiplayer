using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;


namespace Tank3DMultiplayer.UI
{
    public class MainMenuUI : UIPanelBase
    {
        [SerializeField] private Text m_TextPlayerName;
        [SerializeField] private Button m_ButtonSetting;
        [SerializeField] private Button m_ButtonStartFight;
        [SerializeField] private Button m_ButtonListTanks;
        [SerializeField] private Button m_ButtonShop;

        private void Start()
        {
            m_TextPlayerName.text = NameGenerator.GetName(AuthenticationService.Instance.PlayerId);

            m_ButtonSetting.onClick.AddListener(delegate
            {
                MenuManager.Instance.OpenMenu(MenuName.SettingMenu);
            });

            m_ButtonStartFight.onClick.AddListener(delegate
            {
                MenuManager.Instance.OpenMenu(MenuName.CreateAndJoinRoomMenu);
            });

            m_ButtonListTanks.onClick.AddListener(delegate
            {
                MenuManager.Instance.OpenMenu(MenuName.ListTankMenu);
            });

            m_ButtonShop.onClick.AddListener(delegate
            {

            });
        }
    }
}

