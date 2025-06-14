using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int maxEnemies;
    private int killedEnemies = 0; //죽인 적 수
    private bool gameEnded = false; //승리/패배 중복 방지용

    public static GameManager Instance;
    // Start is called before the first frame update

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[GameManager] Singleton 인스턴스 설정 완료");
        }
        else
        {
            Debug.LogWarning("[GameManager] 중복 인스턴스 발견 → 삭제됨");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //SpawnManager.cs 찾아서 최대 적 수 가져옴
        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
        {
            maxEnemies = spawnManager.maxEnemies;
            Debug.Log($"[GameManager] maxEnemies 설정됨: {maxEnemies}");
        }
        else
        {
            Debug.Log("SpawnManager.cs 못찾음");
        }

    }

    // EnemyController에서 적이 죽었을 때 호출됨
    public void NotifyEnemyKilled()
    {
        // 이미 게임이 끝났다면 무시
        if (gameEnded) return;

        killedEnemies++; // 적 처치 수 증가
        Debug.Log($"적 처치됨: {killedEnemies}/{maxEnemies}");

        // 승리 조건: 모든 적을 처치했을 경우
        if (killedEnemies >= maxEnemies)
        {
            GameWin();
        }
    }

    //패배 조건
    public void NotifyEnemySurvived()
    {
        Debug.Log("[GameManager] NotifyEnemySurvived 호출됨");
        
        if (!gameEnded)
        {
            GameOver();
        }
        else
        {
            Debug.Log("[GameManager] 이미 gameEnded 상태 → 무시");
        }
    }


    //승리
    void GameWin()
    {
        gameEnded = true;
        Debug.Log("win");
    }

    //패배 처리
    void GameOver()
    {
        gameEnded = true;
        Debug.Log("Lose");
    }

    // 게임 재시작
    public void RestartGame()
    {
        // 현재 씬을 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    

}
