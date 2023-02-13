using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    Movement movement;

    private float speedRotate = 250;

    private void Start()
    {
        movement= GetComponentInParent<Movement>();
    }
    private void Update()
    {
        if (movement.IsMoving())
        {
            transform.Rotate(Vector3.right, speedRotate * Time.deltaTime);
        }
    }
}
