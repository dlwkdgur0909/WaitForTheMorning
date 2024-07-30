using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefab;

    public float spawnTimer = 5f;

    private void Start()
    {
        StartCoroutine(SpawnMonster(spawnTimer));
    }

    private void RandomSpawnMonster()
    {
        int randomIndex = Random.Range(0, monsterPrefab.Length);
        Instantiate(monsterPrefab[randomIndex], transform.position, Quaternion.identity);
    }

    private IEnumerator SpawnMonster(float time)
    {
        while (true)
        {
            RandomSpawnMonster();
            yield return new WaitForSeconds(time);
        }
    }
}
