using System;

namespace Tank3DMultiplayer.Data
{
    public class TankData
    {
        private TankType tankType;
        private string name;
        private float speed;
        private float speedRotate;
        private float maxHealth;
        private float damagePerShoot;
        private float shootingRange;
        private float timeBetweenTwoShoot;
        private float bulletSpeed;
        private float pernetrateArmor;
        private float armor;

        public TankData(string tankType, string name, float speed, float speedRotate, float maxHealth, float damagePerShoot, float shootingRange, float timeBetweenTwoShoot, float bulletSpeed, float pernetrateArmor, float armor)
        {
            Enum.TryParse<TankType>(tankType, out this.tankType);
            this.name = name;
            this.speed = speed;
            this.speedRotate = speedRotate;
            this.maxHealth = maxHealth;
            this.damagePerShoot = damagePerShoot;
            this.shootingRange = shootingRange;
            this.timeBetweenTwoShoot = timeBetweenTwoShoot;
            this.bulletSpeed = bulletSpeed;
            this.pernetrateArmor = pernetrateArmor;
            this.armor = armor;
        }

        public TankType TankType { get => tankType; set => tankType = value; }
        public string Name { get => name; set => name = value; }
        public float Speed { get => speed; set => speed = value; }
        public float SpeedRotate { get => speedRotate; set => speedRotate = value; }
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float DamagePerShoot { get => damagePerShoot; set => damagePerShoot = value; }
        public float ShootingRange { get => shootingRange; set => shootingRange = value; }
        public float TimeBetweenTwoShoot { get => timeBetweenTwoShoot; set => timeBetweenTwoShoot = value; }
        public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }
        public float PernetrateArmor { get => pernetrateArmor; set => pernetrateArmor = value; }
        public float Armor { get => armor; set => armor = value; }
    }
}

