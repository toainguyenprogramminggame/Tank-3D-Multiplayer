using UnityEngine;

namespace Tank3DMultiplayer.UI
{
    public class Menu : MonoBehaviour
    {
        public MenuName menuName;

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }
    }
}

