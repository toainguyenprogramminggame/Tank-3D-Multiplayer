using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tank3DMultiplayer.UI
{
    public class CreateOrJoinLobbyUI : UIPanelBase
    {
        public Button m_ButtonBackToMain;

        bool m_IsLobbyPrivate = false;
        [Header("Create Lobby")]
        public Toggle m_TogglePrivate;
        public Button m_ButtonCreateLobby;


        string m_LobbyCode = "";

        [Header("Join Lobby")]
        public InputField m_InputLobbyCode;
        public Button m_ButtonJoinLobby;




        private void Start()
        {
            m_ButtonBackToMain.onClick.AddListener(delegate
            {
                MenuManager.Instance.OpenMenu(MenuName.MainMenu);
            });

            m_TogglePrivate.onValueChanged.AddListener(delegate
            {
                SetPrivateServer(m_TogglePrivate);
            });

            m_ButtonCreateLobby.onClick.AddListener(delegate
            {
                OnCreatePressed();
            });


            m_InputLobbyCode.onValueChanged.AddListener(delegate
            {
                SetLobbyCode(m_InputLobbyCode);
            });

            m_ButtonJoinLobby.onClick.AddListener(delegate
            {
                OnJoinPressed();
            });
        }

        void SetPrivateServer(Toggle togglePrivate)
        {
            m_IsLobbyPrivate = togglePrivate.isOn;
        }

        public async void OnCreatePressed()
        {
            await Manager.CreateLobby(ConstValue.LOBBY_NAME, m_IsLobbyPrivate);
        }


        void SetLobbyCode(InputField inputField)
        {
            m_LobbyCode = inputField.text;
        }


        public async void OnJoinPressed()
        {
            await Manager.JoinLobby(m_LobbyCode);
        }
    }

}
