
using System;
using Tank3DMultiplayer;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Shoot : NetworkBehaviour
{
    private TankType bulletType;
    public Transform gunHolder; 
    public float speedRotate;
    public Transform shootPoint;

    Vector3 aim = Vector3.zero;
    bool shoot = false;
    bool blockShoot = false;
    bool autoShoot = false;

    float countDownTimeShot;

    float damagePerShoot;
    float shootingRange;
    float bulletSpeed;
    float timeBetweenTwoShot;


    private bool HasAuthority => IsOwner;

    public void ShootUpdate(bool isShoot, Vector3 aim, bool autoShoot, bool blockShoot)
    {
        this.shoot = isShoot;
        this.aim = aim;
        this.autoShoot = autoShoot;
        this.blockShoot = blockShoot;
    }

    private void Update()
    {
        if (!HasAuthority) 
            return;

        CountDownTimeShot();
        RotateGun();

        if (blockShoot || (!shoot && !autoShoot) || countDownTimeShot > 0)
            return;

        countDownTimeShot = timeBetweenTwoShot;
        ShootServerRpc(aim,shootPoint.position);
    }

    void CountDownTimeShot()
    {
        countDownTimeShot -= Time.deltaTime;
    }

    void RotateGun()
    {
        // Rotate to the direction
        Vector3 direction = aim - gunHolder.position;
        Quaternion lookTo = Quaternion.LookRotation(direction, gunHolder.up);
        Vector3 rotation = Quaternion.Slerp(gunHolder.rotation, lookTo, speedRotate * Time.deltaTime).eulerAngles;
        gunHolder.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    [ServerRpc]
    void ShootServerRpc(Vector3 aim, Vector3 shootPoint)
    {
        NetworkObject bulletNetworkObject = NetworkObjectPool.Instance.GetNetworkObject(bulletType);
        GameObject bulletObj = bulletNetworkObject.gameObject;

        Vector3 newAim = new Vector3(aim.x, shootPoint.y, aim.z);
        // Setup position and rotation
        Vector3 direction = (newAim - shootPoint).normalized;
        bulletObj.transform.position = shootPoint;
        Quaternion lookTo = Quaternion.LookRotation(direction, bulletObj.transform.up);
        bulletObj.transform.rotation = lookTo;

        // Setup data for bullet
        ulong ownId = GetComponent<NetworkObject>().OwnerClientId;
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SetupData(ownId, bulletSpeed,damagePerShoot, shootingRange, direction, shootPoint);

        bulletNetworkObject.Spawn();
        bullet.StartLaunch();
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
