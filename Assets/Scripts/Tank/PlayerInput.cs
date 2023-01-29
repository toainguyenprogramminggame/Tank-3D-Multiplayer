using UnityEngine;
using Unity.Netcode;
using Tank3DMultiplayer;

public class PlayerInput : NetworkBehaviour
{
    public LayerMask whatIsGround;
    Camera cam;

    Vector3 move;
    Vector3 aim;
    bool shoot, autoShoot = false;
    bool skill1,skill2,skill3;

    public TankController controller;
    private bool HasAuthority => IsServer;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        aim = transform.forward;
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, whatIsGround))
        {
            aim = new Vector3(hit.point.x, ConstValue.DEFAULT_Y_SHOOT, hit.point.z);
        }

        shoot = Input.GetMouseButtonDown(0);

        if (Input.GetMouseButtonDown(1))
        {
            autoShoot = !autoShoot;
        };

        skill1 = Input.GetKeyDown(KeyCode.Q);
        skill2 = Input.GetKeyDown(KeyCode.E);
        skill3 = Input.GetKeyDown(KeyCode.Space);

        // Update data input to Controller
        controller.SkillUpdate(skill1, skill2, skill3);
        controller.ShootUpdate(aim, shoot, autoShoot);
        controller.MoveUpdate(move.normalized);
    }
}
