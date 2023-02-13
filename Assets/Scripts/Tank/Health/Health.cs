using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    NetworkVariable<float> maxHealth = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    NetworkVariable<float> curHealth = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public HealthBar healthBar;


    public override void OnNetworkSpawn()
    {
        maxHealth.OnValueChanged += (float previousHealth, float newHealth) =>
        {
            healthBar.SetMaxHealth(newHealth);
        };
        curHealth.OnValueChanged += (float previousHealth, float newHealth) => // Update health bar if current HP is changed
        {
            healthBar.SetHealth(newHealth);
        };
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetupDataServerRpc(float maxHealth)
    {
        if (!IsServer)
            return;
        this.maxHealth.Value = maxHealth;
        this.curHealth.Value = maxHealth;

        SetupHealthBarClientRpc(maxHealth);
    }

    [ClientRpc]
    public void SetupHealthBarClientRpc(float maxHealth)
    {
        if(IsServer) return;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
    }

    public void Damage(ulong clientIdDamaged, ulong clientIdWhoShoot, float damage)
    {
        if(!IsServer) return;
        curHealth.Value -= damage;
        // Tru giap cac kieu

        // Done

        //If Death
        if(curHealth.Value <= 0)
        {
            OnDeath(clientIdWhoShoot, clientIdDamaged);
        }
    }

    public void OnDeath(ulong clientIdWhoShoot, ulong clientIdWhoDead)
    {
        // Cap nhat ket qua Score Manager
        PlayersManager.Instance.Kill(clientIdWhoShoot, clientIdWhoDead);

        // Gui toi client bi kill ClientRpc dem nguoc de hoi sinh
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientIdWhoDead }
            }
        };
        ChangePlayerStatusClientRpc(true, clientRpcParams);

        // Giet thang nay
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }


    [ClientRpc]
    private void ChangePlayerStatusClientRpc(bool isDead, ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("Start Change");
        if(!IsOwner) return;
        Debug.Log("Can Change");

        GamePlayManager.Instance.ChangeStatus(isDead);
    }


    //private void Update()
    //{
    //    if (!IsServer)
    //        return;
    //    curHealth.Value -= 300 * Time.deltaTime;
    //    if(curHealth.Value <= 0)
    //    {
    //        OnDeath(0, 0);
    //    }
    //}

}
