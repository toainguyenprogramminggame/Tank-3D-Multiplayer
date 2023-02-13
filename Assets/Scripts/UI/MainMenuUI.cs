using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;


namespace Tank3DMultiplayer.UI
{
    public class MainMenuUI : UIPanelBase
    {
        [Header("UI Elements")]
        [SerializeField] private Text m_TextPlayerName;
        [SerializeField] private Image m_ImageAvatarPlayer;
        [SerializeField] private Button m_ButtonSetting;
        [SerializeField] private Button m_ButtonStartFight;
        [SerializeField] private Button m_ButtonListTanks;
        [SerializeField] private Button m_ButtonShop;
        [SerializeField] private Button m_ButtonOpenChangeProfile;

        [Header("Object Elements")]
        [SerializeField] private Transform m_TankHolder;
        [SerializeField] private List<MainTank> m_TanksMesh = new List<MainTank>();

        private TankType m_SelectedTank = TankType.Hulk;

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

            m_ButtonOpenChangeProfile.onClick.AddListener(delegate
            {
                MenuManager.Instance.OpenMenu(MenuName.ProfileMenu);
            });

            LoadPlayerInformation();
        }

        public void SelectMainTank(TankType tankType)
        {
            m_SelectedTank = tankType;
            int index = m_TanksMesh.FindIndex(x => x.type== tankType);
            if(index == -1)
            {
                Debug.LogError("CANT'T FIND MESH OF " + m_SelectedTank .ToString());
                return;
            }
            GameObject prevTankMesh = m_TankHolder.GetChild(0).gameObject;
            Destroy(prevTankMesh );

            GameObject newTankMesh = Instantiate(m_TanksMesh[index].mesh);
            newTankMesh.transform.SetParent(m_TankHolder);
            newTankMesh.transform.localPosition = Vector3.zero;
            newTankMesh.transform.localRotation = Quaternion.identity;
            newTankMesh.transform.localScale = Vector3.one;
        }

        [Serializable]
        struct MainTank
        {
            public TankType type;
            public GameObject mesh;
        }

        public void LoadPlayerInformation()
        {
            string name = PlayerPrefs.GetString(ConstValue.PREF_KEY_PLAYER_NAME, NameGenerator.GetName(AuthenticationService.Instance.PlayerId));
            ProfileAvatar avatarType = ProfileAvatar.Hulk;
            Enum.TryParse(PlayerPrefs.GetString(ConstValue.PREF_KEY_PLAYER_AVATAR, ProfileAvatar.Hulk.ToString()), out avatarType);

            m_TextPlayerName.text = name;
            m_ImageAvatarPlayer.sprite = ProfileAssets.Instance.GetAvatar(avatarType);
        }

    }
}

