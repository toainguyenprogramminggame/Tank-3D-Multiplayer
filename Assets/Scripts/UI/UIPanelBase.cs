using System.Collections;
using System.Collections.Generic;
using Tank3DMultiplayer.Manager;
using UnityEngine;


namespace Tank3DMultiplayer.UI
{
    public class UIPanelBase : MonoBehaviour
    {
        GameManager m_gameManager;

        protected GameManager Manager
        {
            get
            {
                if (m_gameManager != null) return m_gameManager;
                return m_gameManager = GameManager.Instance;
            }
        }


    }
}

