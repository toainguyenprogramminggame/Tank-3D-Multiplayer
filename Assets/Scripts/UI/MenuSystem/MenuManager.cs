using System.Collections;
using System.Collections.Generic;
using Tank3DMultiplayer.Support;
using UnityEngine;
using UnityEngine.UI;

namespace Tank3DMultiplayer.UI
{
    public class MenuManager : Singleton<MenuManager>
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

        public void OpenMenu(MenuName menuName)
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

