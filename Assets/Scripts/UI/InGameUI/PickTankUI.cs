using System;
using System.Collections;
using System.Collections.Generic;
using Tank3DMultiplayer;
using UnityEngine;

public class PickTankUI : MonoBehaviour
{
    [Header("Object Elements")]
    [SerializeField] private Transform m_TankHolder;
    [SerializeField] private List<MainTank> m_TanksMesh = new List<MainTank>();
    private TankType m_SelectedTank = TankType.Hulk;


    [Serializable]
    struct MainTank
    {
        public TankType type;
        public GameObject mesh;
    }

    public void SelectMainTank(TankType tankType)
    {
        m_SelectedTank = tankType;
        int index = m_TanksMesh.FindIndex(x => x.type == tankType);
        if (index == -1)
        {
            Debug.LogError("CANT'T FIND MESH OF " + m_SelectedTank.ToString());
            return;
        }
        GameObject prevTankMesh = m_TankHolder.GetChild(0).gameObject;
        Destroy(prevTankMesh);

        GameObject newTankMesh = Instantiate(m_TanksMesh[index].mesh);
        newTankMesh.transform.SetParent(m_TankHolder);
        newTankMesh.transform.localPosition = Vector3.zero;
        newTankMesh.transform.localRotation = Quaternion.identity;
        newTankMesh.transform.localScale = Vector3.one;
    }
}
