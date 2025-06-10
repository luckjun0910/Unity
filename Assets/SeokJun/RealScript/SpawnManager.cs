using System.Collections;
using System.Collections.Generic;
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

    private List<Transform> shuffledDestinations = new List<Transform>(); // 섞인 도착지 리스트
    private int currentDestinationIndex = 0; // 현재 도착지 순서


    void ShuffleDestinations()
    {
        // 기존 리스트 초기화
        shuffledDestinations.Clear();

        // 배열을 리스트로 복사
        shuffledDestinations.AddRange(destinationPoints);

        // Fisher-Yates 셔플 알고리즘으로 섞는다
        for (int i = shuffledDestinations.Count - 1; i > 0; i--)
        {
            // 0부터 i까지의 인덱스 중 하나를 무작위로 선택
            int j = Random.Range(0, i + 1);

            // i번째와 j번째를 서로 바꾼다
            Transform temp = shuffledDestinations[i];
            shuffledDestinations[i] = shuffledDestinations[j];
            shuffledDestinations[j] = temp;
        }

        currentDestinationIndex = 0; // 순서 초기화
    }


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        // 시작할 때 한 번 섞어줌
        ShuffleDestinations();

        while (spawnedEnemies < maxEnemies)
        {
            // 랜덤 스폰 위치
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            // 셔플된 목적지 리스트에서 순서대로 뽑기
            Transform destPoint = shuffledDestinations[currentDestinationIndex];
            currentDestinationIndex++;

            // 모든 목적지를 다 썼으면 다시 섞기
            if (currentDestinationIndex >= shuffledDestinations.Count)
            {
                ShuffleDestinations();
            }

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
