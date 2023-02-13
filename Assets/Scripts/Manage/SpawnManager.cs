using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tank3DMultiplayer;
using Tank3DMultiplayer.Data;
using Tank3DMultiplayer.Network;
using Tank3DMultiplayer.Support;
using UnityEngine;

public class SpawnManager : SingletonNetwork<SpawnManager>
{
    [SerializeField]
    private List<Transform> positionSpawnTank = new List<Transform>();

    bool[] checkPositionUsed;

    private void Start()
    {
        checkPositionUsed = Enumerable.Repeat(false, positionSpawnTank.Count).ToArray();
    }

    private bool checkAllPositionSpawnedUsed()
    {
        foreach(var position in checkPositionUsed)
        {
            if (!position)
                return false;
        }
        for (int i = 0; i < checkPositionUsed.Length; i++)
        {
            checkPositionUsed[i] = false;
        }
        return true;
    }
    public IEnumerator SpawnTanks(ulong clientId)
    {
        yield return new WaitUntil(() => DataTanks.Instance != null && PlayersManager.Instance != null);

        if (positionSpawnTank.Count <= 0)
        {
            Debug.LogError("NEED ASSIGN POSITION SPAWN TANK");
            yield return null;
        }
        List<TankDataLocal> tanksDataLocal = DataTanks.Instance.ListDataTanksLocal;
        PlayerInGameData playerJoinScene = PlayersManager.Instance.GetPlayerData(clientId);

        TankType tankType = playerJoinScene.tankType;
        if (tankType == TankType.Null)
            tankType = TankType.Hulk;
        int index = tanksDataLocal.FindIndex(x => x.TankType == tankType);
        if (index == -1)
        {
            Debug.LogError("PREFAB OF " + tankType.ToString() + " NOT EXISTS");
        }
        else
        {
            int indexPosition = 0;
            do
            {
                indexPosition = Random.Range(0, positionSpawnTank.Count);
                if (checkAllPositionSpawnedUsed())
                    break;
            } while (checkPositionUsed[indexPosition]);

            checkPositionUsed[indexPosition] = true;
            Vector3 positionSpawn = positionSpawnTank[indexPosition].position;

            GameObject tankPrefab = tanksDataLocal[index].TankPrefab;
            GameObject tank = NetworkObjectSpawner.SpawnNewNetworkObjectChangeOwnershipToClient(tankPrefab, positionSpawn, playerJoinScene.clientId);

            // Setup data for tank on Server
            TankController tankController = tank.GetComponent<TankController>();
            if (tankController != null)
            {
                tankController.SetupDataClientRpc(clientId, tankType);
            }
        }
    }

    public IEnumerator SpawnObjectInGame()
    {
        yield return new WaitUntil(() => PlayersManager.Instance != null);

        List<PlayerInGameData> playersData = PlayersManager.Instance.PlayersData;

        NetworkObjectPool.Instance.InitializedPool(playersData);

        yield return null;
    } 
}
