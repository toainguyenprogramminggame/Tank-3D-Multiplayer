using Tank3DMultiplayer;
using Tank3DMultiplayer.Data;
using Tank3DMultiplayer.UI;
using UnityEngine.UI;
using UnityEngine;

public class DetailTankMenuUI : UIPanelBase
{
    public Button m_ButtonBackToListTank;

    public Image m_ImageTank;
    public Text m_TextName;
    public Text m_TextDamage;
    public Text m_TextSpeed;
    public Text m_TextHealth;
    public Text m_TextBulletSpeed;
    public Text m_TextArmor;
    public Text m_PenetraterArmor;
    public Text m_TextShootingRange;
    public Text m_TextTimeBetweenTwoShoot;

    public Button m_SelectMainTank;

    private TankType m_SelectedTank = TankType.Null;

    private void Start()
    {
        m_ButtonBackToListTank.onClick.AddListener(delegate
        {
            MenuManager.Instance.OpenMenu(MenuName.ListTankMenu);
        });
        m_SelectMainTank.onClick.AddListener(delegate
        {
            SelectMainTank();
        });
    }

    public void SelectMainTank()
    {
        (transform.parent.GetComponentInChildren(typeof(MainMenuUI), true) as MainMenuUI).SelectMainTank(m_SelectedTank);
    }

    public void SelectTank(TankType tankType)
    {
        m_SelectedTank = tankType;
        TankData tankData = DataTanks.Instance.GetDataOfTank(tankType);
        if (tankData == null)
            return;

        m_ImageTank.sprite = DataTanks.Instance.GetImageOfTank(tankType);

        m_TextName.text = tankData.Name;
        m_TextDamage.text = "Damage: " + tankData.DamagePerShoot.ToString();
        m_TextSpeed.text = "Speed: " + tankData.Speed.ToString();
        m_TextHealth.text = "Health: " + tankData.MaxHealth.ToString();
        m_TextBulletSpeed.text = "Bullet Speed: " + tankData.BulletSpeed.ToString();
        m_TextArmor.text = "Armor: " + tankData.Armor.ToString();
        m_PenetraterArmor.text = "Penetrater Armor: " + tankData.PernetrateArmor.ToString();
        m_TextShootingRange.text = "Shoot Range: " + tankData.ShootingRange.ToString();
        m_TextTimeBetweenTwoShoot.text = "Time Between Two Shoot: " + tankData.TimeBetweenTwoShoot.ToString();

    }
}
