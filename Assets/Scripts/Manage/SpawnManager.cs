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

    public IEnumerator SpawnTanks()
    {
        yield return new WaitUntil(() => DataTanks.Instance != null && PlayersManager.Instance != null);

        if (positionSpawnTank.Count <= 0)
        {
            Debug.LogError("NEED ASSIGN POSITION SPAWN TANK");
            yield return null;
        }
        List<TankDataLocal> tanksDataLocal = DataTanks.Instance.ListDataTanksLocal;
        List<TankData> tanksData = DataTanks.Instance.ListDataTanks;
        List<PlayerInGameData> playersData = PlayersManager.Instance.PlayersData;

        bool[] checkPositionUsed = Enumerable.Repeat(false, positionSpawnTank.Count).ToArray();

        foreach(var player in playersData)
        {
            TankType tankType = player.tankType;

            int index = tanksDataLocal.FindIndex(x => x.TankType == tankType);
            if (index == -1)
            {
                Debug.LogError("PREFAB OF " + tankType.ToString() + " NOT EXISTS");
                continue;
            }

            int indexPosition = 0;
            do
            {
                indexPosition = Random.Range(0, positionSpawnTank.Count);
            } while (checkPositionUsed[indexPosition]);

            checkPositionUsed[indexPosition] = true;
            Vector3 positionSpawn = positionSpawnTank[indexPosition].position;

            GameObject tankPrefab = tanksDataLocal[index].TankPrefab;
            GameObject tank = NetworkObjectSpawner.SpawnNewNetworkObjectChangeOwnershipToClient(tankPrefab, positionSpawn, player.clientId);

            // Setup data for tank on Server
            TankData dataOfCurTank = tanksData.Find(x => x.TankType == player.tankType);
            TankController tankController = tank.GetComponent<TankController>();
            if (tankController != null)
            {
                tankController.SetupData(player.clientId, dataOfCurTank);
            }
        }

        yield return null;
    }

    public IEnumerator SpawnObjectInGame()
    {
        yield return new WaitUntil(() => PlayersManager.Instance != null);

        List<PlayerInGameData> playersData = PlayersManager.Instance.PlayersData;

        NetworkObjectPool.Instance.InitializedPool(playersData);

        yield return null;
    } 
}
