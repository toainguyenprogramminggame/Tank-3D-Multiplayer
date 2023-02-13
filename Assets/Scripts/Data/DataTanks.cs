using System.Collections.Generic;
using Tank3DMultiplayer.Support;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Tank3DMultiplayer.Data
{
    public class DataTanks : SingletonPersistent<DataTanks>
    {
        private List<TankData> listDataTanks = new List<TankData>();
        [SerializeField]
        private List<TankDataLocal> listDataTanksLocal = new List<TankDataLocal>();

        public List<TankData> ListDataTanks { get { return listDataTanks; } }
        public List<TankDataLocal> ListDataTanksLocal { get => listDataTanksLocal; }

        public TankData GetDataOfTank(TankType type)
        {
            int index = listDataTanks.FindIndex(x => x.TankType== type);
            if (index == -1)
                return null;
            return listDataTanks[index];
        }
        public Sprite GetImageOfTank(TankType type)
        {
            int index = listDataTanksLocal.FindIndex(x => x.TankType == type);
            if (index == -1)
                return null;

            return listDataTanksLocal[index].TankAvatar;
        }
        public bool ParseData(string dataTanks)
        {
            string afterCut = dataTanks.ToString().Remove(dataTanks.Length - 1, 1).Remove(0, 1);

            string dataObjects = afterCut.Replace("},{", "};{");
            string[] parts = dataObjects.Split(';');

            foreach (string part in parts)
            {
                Debug.Log(part);
                JObject jObject = JObject.Parse(part);

                string type = jObject["Type"].ToString();
                string name = jObject["Name"].ToString();
                float speed = (float)jObject["Speed"];
                float speedRotate = (float)jObject["SpeedRotate"];
                float maxHealth = (float)jObject["MaxHealth"];
                float damagePerShoot = (float)jObject["DamagePerShoot"];
                float shootingRange = (float)jObject["ShootingRange"];
                float timeBetweenTwoShoot = (float)jObject["TimeBetweenTwoShoot"];
                float bulletSpeed = (float)jObject["BulletSpeed"];
                float penetrateArmor = (float)jObject["PenetraterArmor"];
                float armor = (float)jObject["Armor"];

                TankData tankData = new TankData(type, name, speed, speedRotate, maxHealth, damagePerShoot
                                        , shootingRange, timeBetweenTwoShoot, bulletSpeed, penetrateArmor, armor);

                listDataTanks.Add(tankData);
            }

            return true;
        }

        public Sprite GetAvatarOfTank(TankType tankType)
        {
            int index = listDataTanksLocal.FindIndex((item) => item.TankType == tankType);

            if(index == -1)
                return null;

            return listDataTanksLocal[index].TankAvatar;
        }
    }
}

