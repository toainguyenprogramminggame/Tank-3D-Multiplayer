using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tank3DMultiplayer.UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private List<Menu> listMenus = new List<Menu>();

        public void OpenMenu(Menu menu)
        {
            foreach (Menu menu2 in listMenus)
            {
                menu2.Close();
            }

            menu.Open();
        }

        public void OpenMenu(string menuName)
        {
            foreach(Menu menu in listMenus)
            {
                if (menuName == menu.menuName)
                {
                    menu.Open();
                }
                else
                {
                    
                    menu.Close();
                }
            }
        }
    }
}

