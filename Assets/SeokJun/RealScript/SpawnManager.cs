using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Soldier 프리팹
    public Transform[] spawnPoints; // 스폰 위치들
    public Transform[] destinationPoints; // 목적지 위치들
    public int maxEnemies = 15; // 총 적 스폰 수
    public float spawnInterval = 3f; // 적 스폰 간격

    private int spawnedEnemies = 0; // 현재까지 스폰된 적 수

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawnedEnemies < maxEnemies)
        {
            // 랜덤 스폰 위치
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            // 랜덤 목적지
            int destIndex = Random.Range(0, destinationPoints.Length);
            Transform destPoint = destinationPoints[destIndex];

            // 적 생성
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            // NavMeshAgent로 목적지로 이동
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(destPoint.position);
            }

            spawnedEnemies++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
