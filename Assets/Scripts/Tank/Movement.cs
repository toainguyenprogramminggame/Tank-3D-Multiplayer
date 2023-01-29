using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    CharacterController characterController;
    public Transform body;

    NetworkVariable<Vector3> move = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<bool> blockMove = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    Vector3 curDirection;

    float speed = 20;
    float speedRotate = 50;

    private bool HasAuthority => IsServer;

    private void Start()
    {

        if (!IsOwner && !IsServer)
            return;

        curDirection = transform.forward;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (move.Value.normalized != Vector3.zero)
            curDirection = move.Value.normalized;

        if (!HasAuthority || blockMove.Value)
            return;

        characterController.Move(move.Value * speed * Time.deltaTime);
        Rotate();
    }

    void Rotate()
    {
        // Rotate to the direction
        Vector3 direction = curDirection.normalized;
        Quaternion lookTo = Quaternion.LookRotation(direction, body.up);
        Vector3 rotation = Quaternion.Slerp(body.rotation, lookTo, speedRotate * Time.deltaTime).eulerAngles;
        body.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    public void MoveUpdate(Vector3 move, bool blockMove)
    {
        this.move.Value = move;
        this.blockMove.Value = blockMove;
    }

    public void SetupData(float speed, float speedRotate)
    {
        this.speed = speed;
        this.speedRotate = speedRotate;
    }
}
