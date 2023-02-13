using Tank3DMultiplayer;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private TankType bulletType;

    private ulong ownId;
    private float speed;
    private float damage;
    private float range;
    private Vector3 direction;

    private Vector3 rootPoint;

    private bool isLaunched = false;

    Vector3 prevPosition;

    private bool HasAuthority => IsServer;

    public void SetupData(ulong clientId, float speed, float damage,float range, Vector3 direction, Vector3 rootPoint)
    {
        this.ownId = clientId;
        this.speed = speed;
        this.damage = damage;
        this.range = range;
        this.direction = direction;
        this.rootPoint = rootPoint;
    }
    private void Update()
    {
        if (!HasAuthority)
            return;
        if (isLaunched)
        {
            prevPosition = transform.position;
            transform.Translate(direction * speed * 3 * Time.deltaTime, Space.World);
            Collide();
        }
        DestroyWhenOutOfRange();
    }
    public void Collide()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPosition, (transform.position - prevPosition).normalized), (transform.position - prevPosition).magnitude);

        foreach (var ray in hits)
        {
            GameObject obstacle = ray.collider.gameObject;
            if(obstacle.tag == ConstValue.TAG_TANK)
            {
                NetworkObject obsNetObj = obstacle.GetComponent<NetworkObject>();
                if (obsNetObj != null)
                {
                    if (obsNetObj.OwnerClientId != ownId)
                    {
                        // Kiem tra neu no la tank dich thi tru mau
                        Health health = obstacle.GetComponent<Health>();
                        if(health != null)
                        {
                            health.Damage(obsNetObj.OwnerClientId, ownId, damage);
                        }
                    }
                }
            }
        }

        if(hits.Length > 0)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
    public void StartLaunch()
    {
        isLaunched= true;
    }
    public void DestroyWhenOutOfRange()
    {
        if(Vector3.Distance(transform.position, rootPoint) > range)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
