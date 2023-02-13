using System;
using Tank3DMultiplayer;
using Tank3DMultiplayer.UI;
using UnityEngine;
using UnityEngine.UI;
using static ProfileAssets;

public class ProfileMenuUI : UIPanelBase
{
    [SerializeField]
    private Button buttonBackToMainMenu;

    [SerializeField]
    private Button confirmChangeProfile;

    [SerializeField]
    private InputField inputNewName;

    [SerializeField]
    private GameObject itemProfile;

    [SerializeField]
    private Transform gridContent;

    private string currentNameChanged = "";

    private ProfileAvatar currentAvatar = ProfileAvatar.Hulk;

    private void Start()
    {
        buttonBackToMainMenu.onClick.AddListener(delegate
        {
            MenuManager.Instance.OpenMenu(MenuName.MainMenu);
        });

        inputNewName.onValueChanged.AddListener(delegate
        {
            currentNameChanged = inputNewName.text;
        });

        confirmChangeProfile.onClick.AddListener(() =>
        {
            ConfirmChangeProfile();
        });

        Enum.TryParse(PlayerPrefs.GetString(ConstValue.PREF_KEY_PLAYER_AVATAR, ProfileAvatar.Hulk.ToString()), out currentAvatar);

        CreateGridAvatar();
    }

    private void ConfirmChangeProfile()
    {
        if (!string.IsNullOrEmpty(currentNameChanged))
        {
            PlayerPrefs.SetString(ConstValue.PREF_KEY_PLAYER_NAME, currentNameChanged);
        }
        PlayerPrefs.SetString(ConstValue.PREF_KEY_PLAYER_AVATAR, currentAvatar.ToString());

        (transform.parent.GetComponentInChildren(typeof(MainMenuUI), true) as MainMenuUI).LoadPlayerInformation();
    }

    public void ChangeProfileAvatar(ProfileAvatar avatar)
    {
        currentAvatar = avatar;
    }

    private void CreateGridAvatar()
    {
        foreach(Profile profile in ProfileAssets.Instance.listProfile)
        {
            GameObject item = Instantiate(itemProfile, gridContent,false);
            item.GetComponent<Image>().sprite = profile.image;
            item.GetComponent<ItemProfileUI>().SetProfileAvatar(profile.nameAvatar);
        }

    }
}
