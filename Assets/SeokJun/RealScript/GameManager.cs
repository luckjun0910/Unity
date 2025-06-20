using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int maxEnemies;
    private int killedEnemies = 0; //죽인 적 수
    private bool gameEnded = false; //승리/패배 중복 방지용

    public static GameManager Instance;
    public float gameStartTime;

    [Header("GameOver UI")]
    // UI 연결
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private Text resultText;
    [SerializeField] private Text timeText;
    public Text enemyCountText;

    [Header("Sound")]
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;

    private AudioSource audioSource;



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
        audioSource = GetComponent<AudioSource>();

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

        if (spawnManager != null)
        {
            maxEnemies = spawnManager.maxEnemies;
            Debug.Log($"[GameManager] maxEnemies 설정됨: {maxEnemies}");
        }

        if (enemyCountText != null)
            enemyCountText.gameObject.SetActive(false);

    }

    // EnemyController에서 적이 죽었을 때 호출됨s
    public void NotifyEnemyKilled()
    {
        // 이미 게임이 끝났다면 무시
        if (gameEnded) return;

        killedEnemies++; // 적 처치 수 증가
        Debug.Log($"적 처치됨: {killedEnemies}/{maxEnemies}");

        UpdateEnemyUI();

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
        ShowGameOverUI(true);
        if (audioSource != null && winClip != null)
        {
            audioSource.PlayOneShot(winClip);
        }
    }

    //패배 처리
    void GameOver()
    {
        gameEnded = true;
        Debug.Log("Lose");
        ShowGameOverUI(false);
        if (audioSource != null && loseClip != null)
        {
            audioSource.PlayOneShot(loseClip);
        }
    }

    // 게임 재시작
    public void RestartGame()
    {
        Time.timeScale = 1f; // 시간 되돌리기

        // 현재 씬을 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ShowGameOverUI(bool win)
    {
        Time.timeScale = 0f; // 전체 게임 정지

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        if (resultText != null)
            resultText.text = win ? "승리 !!" : "패배...";

        if (timeText != null)
        {
            float elapsed = Time.time - GameManager.Instance.gameStartTime;
            timeText.text = $"전투 시간: {elapsed:F1}초";
        }

        if (enemyCountText != null)
            enemyCountText.gameObject.SetActive(false);

        // 총 숨기기
        GameObject gun = GameObject.FindWithTag("Gun");
        if (gun != null) gun.SetActive(false);

        // 탄약 UI 숨기기
        GameObject ammoUI = GameObject.Find("AmmoText");
        if (ammoUI != null) ammoUI.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // 시간 되돌리기

        SceneManager.LoadScene("StartScene"); // ← StartScene 이름에 맞게
    }

    void UpdateEnemyUI()
    {
        if (enemyCountText != null)
        {
            int remaining = maxEnemies - killedEnemies;
            enemyCountText.text = $"{maxEnemies} / {remaining}";
        }
    }

    public void SetMaxEnemies(int count)
    {
        maxEnemies = count;
        UpdateEnemyUI();
        
    }
    
}
