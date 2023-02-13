using System;
using System.Collections.Generic;
using Tank3DMultiplayer;
using Tank3DMultiplayer.Support;
using UnityEngine;

public class ProfileAssets : SingletonPersistent<ProfileAssets>
{
    [Serializable]
    public class Profile
    {
        public ProfileAvatar nameAvatar;
        public Sprite image;
    }

    public List<Profile> listProfile = new List<Profile>();

    public Sprite GetAvatar(ProfileAvatar profileAvatar = ProfileAvatar.Hulk)
    {
        int index = listProfile.FindIndex(x=> x.nameAvatar == profileAvatar);
        if (index == -1)
            return null;

        return listProfile[index].image;
    }
}
