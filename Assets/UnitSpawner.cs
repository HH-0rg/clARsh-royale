using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CRC;

public class UnitSpawner : NetworkBehaviour
{
    // Start is called before the first frame update
    public static UnitSpawner unitSpawner;
    public void SpawnUnit(Unit unit, Vector3 transform, KingTower m_KingTower)
    {
        if (!isLocalPlayer)
            return;
        CmdSpawnMyCrap(unit, transform, m_KingTower);
    }
    [Command]
    void CmdSpawnMyCrap(Unit unit, Vector3 transform, KingTower m_KingTower)
    {
        Unit spawnedUnit = Instantiate
                (
                    unit.gameObject,
                    transform + Vector3.up * unit.transform.lossyScale.y,
                    Quaternion.identity
                )
                .GetComponent<Unit>();
        if (spawnedUnit == null)
            return;
        unit.Initialize(m_KingTower);
        NetworkServer.Spawn(spawnedUnit.gameObject);
    }

    private void Awake()
    {
        if (unitSpawner == null)
            unitSpawner = this;
    }

    // Update is called once per frame
}
