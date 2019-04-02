using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : ManagerBase<MonsterManager>
{
    public enum MonsterDictionary
    {
        Mummy,
    }

    [Header("SpawnPoints")]
    [SerializeField]
    List<Transform> spawnPoints;
    [Header("MonsterModels")]
    [SerializeField]
    List<GameObject> models;
    [Header("MonsterRef")]
    [SerializeField]
    List<MonsterScript> monsters;

    public void RegisterMonster(MonsterScript mon)
    {
        monsters.Add(mon);
    }

    public void ClearMonster()
    {
        for (int i=0; i<monsters.Count; i++)
        {
            MonsterScript mon = monsters[i];
            mon.KillMummy();
            monsters[i] = null;
        }
        monsters.Clear();
    }

    public void SpawnMonster(MonsterDictionary index, int num)
    {
        ClearMonster();

        Debug.Log("Spawn monster");
        GameObject obj = models[(int)index];

        for (int i = 0; i < num; i++) {
            int k = Random.Range(0, spawnPoints.Count);
            Instantiate(obj, spawnPoints[k].position, spawnPoints[i].rotation);
        }
    }
    
}
