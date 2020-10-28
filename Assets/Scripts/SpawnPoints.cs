using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    void Start()
    {
        //DungeonGen.Instance.AddSpawnPoint(this.gameObject);
    }

    public void SpawnRegistering()
    {
        DungeonGen.Instance.AddSpawnPoint(gameObject);
    }
}
