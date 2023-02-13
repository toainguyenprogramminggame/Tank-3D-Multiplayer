using UnityEngine;
using Unity.Netcode;
using Tank3DMultiplayer.Data;
using System.Collections.Generic;
using Tank3DMultiplayer;

public class TankController : NetworkBehaviour
{
    [Header("Scripts References")]
    public Movement playerMovement;
    public Shoot playerShoot;
    public CameraFollow cameraFollow;
    public Health health;

    ulong ownId { get; set; }
    private bool HasAuthority => IsOwner;

    Vector3 move;
    bool blockMove = false;

    Vector3 aim;
    bool shoot, autoShoot;
    bool blockShoot = false;

    bool skill1, skill2, skill3;
    private void Update()
    {
        if (!HasAuthority)
            return;
        playerMovement.MoveUpdate(move, blockMove);
        playerShoot.ShootUpdate(shoot, aim, autoShoot, blockShoot);
    }

    // Update data input to controller 
    public void MoveUpdate(Vector3 move)
    {
        this.move = move;
    }


    public void ShootUpdate(Vector3 aim, bool isShoot, bool autoShoot)
    {
        this.shoot = isShoot;
        this.aim = aim;
        this.autoShoot = autoShoot;
    }

    public void SkillUpdate(bool skill1, bool skill2, bool skill3)
    {
        this.skill1 = skill1;
        this.skill2 = skill2;
        this.skill3 = skill3;
    }

    [ClientRpc]
    public void SetupDataClientRpc(ulong clientId, TankType tankType)
    {
        List<TankData> tanksData = DataTanks.Instance.ListDataTanks;
        TankData dataOfCurTank = tanksData.Find(x => x.TankType == tankType);

        this.ownId = clientId;
        playerMovement.SetupData(dataOfCurTank.Speed, dataOfCurTank.SpeedRotate);
        playerShoot.SetupData(dataOfCurTank.TankType, dataOfCurTank.DamagePerShoot, dataOfCurTank.ShootingRange, dataOfCurTank.TimeBetweenTwoShoot, dataOfCurTank.BulletSpeed);
        health.SetupDataServerRpc(dataOfCurTank.MaxHealth);
    }

    // Use to block move and shoot of player
    public void BlockMove()
    {
        blockMove = true;
    }
    public void UnBlockMove()
    {
        blockMove = false;
    }
    public void BlockShoot()
    {
        blockShoot = true;
    }
    public void UnBlockShoot()
    {
        blockShoot = false;
    }
}
