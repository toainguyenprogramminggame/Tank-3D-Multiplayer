using System.Collections.Generic;
using Tank3DMultiplayer.Support;
using UnityEngine;
using Newtonsoft.Json.Linq;


namespace Tank3DMultiplayer.Data
{
    public class DataTanks : SingletonPersistent<DataTanks>
    {
        private List<TankData> listDataTanks = new List<TankData>();

        public bool ParseData(string dataTanks)
        {
            string afterCut = dataTanks.ToString().Remove(dataTanks.Length - 1, 1).Remove(0, 1);

            string dataObjects = afterCut.Replace("},{", "};{");
            string[] parts = dataObjects.Split(';');

            foreach (string part in parts)
            {
                JObject jObject = JObject.Parse(part);

                string type = jObject["Type"].ToString();
                string name = jObject["Name"].ToString();
                string image = jObject["Image"].ToString();
                float speed = (float)jObject["Speed"];
                float speedRotate = (float)jObject["SpeedRotate"];
                float maxHealth = (float)jObject["MaxHealth"];
                float damagePerShoot = (float)jObject["DamagePerShoot"];
                float shootingRange = (float)jObject["ShootingRange"];
                float timeBetweenTwoShoot = (float)jObject["TimeBetweenTwoShoot"];
                float bulletSpeed = (float)jObject["BulletSpeed"];
                float penetrateArmor = (float)jObject["PenetraterArmor"];
                float armor = (float)jObject["Armor"];

                TankData tankData = new TankData(type, name, image, speed, speedRotate, maxHealth, damagePerShoot
                                        , shootingRange, timeBetweenTwoShoot, bulletSpeed, penetrateArmor, armor);

                listDataTanks.Add(tankData);
            }

            return true;
        }
    }
}

