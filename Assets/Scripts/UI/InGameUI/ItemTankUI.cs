using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using Tank3DMultiplayer.SceneManage.Lobby;

namespace Tank3DMultiplayer.UI
{
    public class ItemTankUI : UIPanelBase
    {
        Button buttonItemTank;

        public TankType TankType { get; set; }

        private void Awake()
        {
            buttonItemTank = GetComponent<Button>();    
        }

        private void Start()
        {
            buttonItemTank.onClick.AddListener(delegate 
            {
                OnButtonPressed(buttonItemTank);
            });
        }

        private void OnButtonPressed(Button button)
        {
            SceneName currentScene;
            Enum.TryParse(SceneManager.GetActiveScene().name, out currentScene);
            switch(currentScene)
            {
                case SceneName.Lobby:
                    GetComponentInParent<ListTankMenuUI>().SelectTank(TankType);
                    break;
                case SceneName.CharacterSelection:
                    Manager.SelectTank(TankType);
                    (transform.root.GetComponentInChildren(typeof(PickTankUI), true) as PickTankUI).SelectMainTank(TankType);
                    break;
                default:
                    break;
            }
        }
    }

}
