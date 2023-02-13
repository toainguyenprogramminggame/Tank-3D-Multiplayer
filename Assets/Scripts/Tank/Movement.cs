
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    CharacterController characterController;
    public Transform body;

    Vector3 move = Vector3.zero;
    bool blockMove = false;

    Vector3 curDirection;

    float speed = 20;
    float speedRotate = 50;

    private bool HasAuthority => IsOwner;

    private void Start()
    {

        if (!IsOwner && !IsServer)
            return;

        curDirection = transform.forward;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (move.normalized != Vector3.zero)
            curDirection = move.normalized;

        if (!HasAuthority || blockMove)
            return;
        characterController.Move(move.normalized * speed * Time.deltaTime);

        Rotate(curDirection);
    }

    void Rotate(Vector3 direction)
    {
        // Rotate to the direction
        Quaternion lookTo = Quaternion.LookRotation(direction, body.up);
        Vector3 rotation = Quaternion.Slerp(body.rotation, lookTo, speedRotate * Time.deltaTime).eulerAngles;
        body.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    public void MoveUpdate(Vector3 move, bool blockMove)
    {
        this.move = move;
        this.blockMove = blockMove;
    }

    public void SetupData(float speed, float speedRotate)
    {
        this.speed = speed;
        this.speedRotate = speedRotate;
    }

    public bool IsMoving()
    {
        return move != Vector3.zero;
    }
}
