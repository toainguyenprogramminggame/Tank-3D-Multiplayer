using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

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
                    break;
                case SceneName.CharacterSelection:
                    Manager.SelectTank(TankType);
                    break;
                default:
                    break;
            }
        }
    }

}
