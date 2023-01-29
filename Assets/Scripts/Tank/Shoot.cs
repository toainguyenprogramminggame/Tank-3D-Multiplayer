
using System;
using Tank3DMultiplayer;
using Unity.Netcode;
using UnityEngine;

public class Shoot : NetworkBehaviour
{
    private TankType bulletType;
    public Transform gunHolder; 
    public float speedRotate;
    public Transform shootPoint;

    NetworkVariable<Vector3> aim = new NetworkVariable<Vector3>(Vector3.zero,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<bool> shoot = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    NetworkVariable<bool> blockShoot = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<bool> autoShoot = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    float countDownTimeShot;


    float damagePerShoot;
    float shootingRange;
    float bulletSpeed;
    float timeBetweenTwoShot;


    private bool HasAuthority => IsServer;

    public void ShootUpdate(bool isShoot, Vector3 aim, bool autoShoot, bool blockShoot)
    {
        this.shoot.Value = isShoot;
        this.aim.Value = aim;
        this.autoShoot.Value = autoShoot;
        this.blockShoot.Value = blockShoot;
    }

    private void Update()
    {
        if(!HasAuthority) 
            return;

        CountDownTimeShot();
        RotateGun();

        if (blockShoot.Value || (!shoot.Value && !autoShoot.Value) || countDownTimeShot > 0)
            return;

        countDownTimeShot = timeBetweenTwoShot;
        Shooting();
    }

    void CountDownTimeShot()
    {
        countDownTimeShot -= Time.deltaTime;
    }

    void RotateGun()
    {
        // Rotate to the direction
        Vector3 direction = aim.Value - gunHolder.position;
        Quaternion lookTo = Quaternion.LookRotation(direction, gunHolder.up);
        Vector3 rotation = Quaternion.Slerp(gunHolder.rotation, lookTo, speedRotate * Time.deltaTime).eulerAngles;
        gunHolder.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shooting()
    {
        Debug.Log("Shoot");
    }

    public void SetupData(TankType bulletType, float damage, float range, float shootRate, float bulletSpeed)
    {
        this.bulletType = bulletType;
        this.damagePerShoot = damage;
        this.shootingRange = range;
        this.timeBetweenTwoShot = shootRate;
        this.bulletSpeed = bulletSpeed;
    }
}
