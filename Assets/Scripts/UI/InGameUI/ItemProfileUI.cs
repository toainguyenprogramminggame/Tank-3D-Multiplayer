using Tank3DMultiplayer;
using UnityEngine;
using UnityEngine.UI;

public class ItemProfileUI : MonoBehaviour
{
    [HideInInspector]
    public ProfileAvatar profileAvatar;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(delegate
        {
            ChangeAvatar();
        });
    }

    private void ChangeAvatar()
    {
        GetComponentInParent<ProfileMenuUI>().ChangeProfileAvatar(profileAvatar);
    }

    public void SetProfileAvatar(ProfileAvatar avatar)
    {
        this.profileAvatar = avatar;
    }
}
