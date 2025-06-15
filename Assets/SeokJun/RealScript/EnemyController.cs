using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //GameManager.cs 참조
    private GameManager gameManager;
    //목적지에 도달 했는지?
    private bool reachedDestination = false;

    //이미 죽었는지
    private bool isDead = false;

    // 적 애니메이션을 제어할 Animator
    private Animator animator;

    // NavMeshAgent: 목적지까지 이동 경로를 찾는 컴포넌트
    private NavMeshAgent agent;

    public float SaveTime = 7f;

    // Start()는 게임 시작 시 한 번 호출됨
    void Start()
    {
        // 이 오브젝트에 붙어있는 Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        // NavMeshAgent 컴포넌트 가져오기
        agent = GetComponent<NavMeshAgent>();

        //게임매니저 가져오기
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update()는 매 프레임마다 호출됨
    void Update()
    {
        // NavMeshAgent의 속도를 기반으로 "IsRunning" 파라미터를 true/false로 설정
        // -> 적이 이동 중이면 
        // true (Run 애니메이션 재생), 멈췄으면 false (Idle 애니메이션 재생)
        animator.SetBool("IsRunning", agent.velocity.magnitude > 0.1f);

        //도착하지 않거나 & 죽지 않았으면
        if (!reachedDestination && !isDead)
        {
            // 목적이 도착 판정 & 남은거리 <= 멈추는거리
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                reachedDestination = true;

                // 목적지에 거의 도착했을 때 (remainingDistance가 stoppingDistance보다 작거나 같을 때)
                // 그리고 현재 재생 중인 애니메이션이 "demo_combat_shoot"가 아니라면
                // "Shoot" 트리거를 발동 (Shoot 애니메이션으로 전환)
                if (agent.remainingDistance <= agent.stoppingDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("demo_combat_shoot"))
                {
                    animator.SetTrigger("Shoot");
                }

                //목적지에 도달함 = 타이머 코루틴
                StartCoroutine(SurvivalCountdown());
            }
        }
    }
    /*
        // 플레이어가 적 시야에 보이는지 확인하는 함수
        bool IsPlayerVisible()
        {
            Transform playerHead = Camera.main.transform; // 플레이어 카메라 위치

            Vector3 dirToPlayer = (playerHead.position - transform.position).normalized;


            // Enemy 레이어는 무시하고 Ray 발사
            int layerMask = ~(1 << LayerMask.NameToLayer("Enemy"));
            //위에서 ray쏘기
            Vector3 origin = transform.position + Vector3.up * 1.6f;

            // 적이 플레이어 방향으로 raycast 날림
            if (Physics.Raycast(origin, dirToPlayer, out RaycastHit hit, 100f, layerMask))
            {
                // ray 쏘는거 화긴용
                Debug.DrawRay(origin, dirToPlayer * 100f, Color.green, 0.5f);
                // 충돌한 대상이 플레이어면 시야에 있음
                return hit.collider.CompareTag("MainCamera"); // 또는 "Player"
            }

            // 충돌이 플레이어가 아니면 안보이게 (아무것도 안함)
            return false;
        }
    */

    bool IsPlayerVisible()
    {
        Transform playerHead = Camera.main.transform;

        Vector3 origin = transform.position + Vector3.up * 1.6f;
        Vector3 dirToPlayer = (playerHead.position - origin).normalized;

        // 시야각 제한: 정면 120도 (90도 좌우)
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > 90f)
        {
            Debug.Log("[Enemy] 플레이어 시야각 바깥 → false");
            return false;
        }

        // 레이어 마스크: Enemy 제외
        int layerMask = ~(1 << LayerMask.NameToLayer("Enemy"));

        if (Physics.Raycast(origin, dirToPlayer, out RaycastHit hit, 100f, layerMask))
        {
            Debug.DrawRay(origin, dirToPlayer * 100f, Color.green, 0.5f);
            return hit.collider.CompareTag("MainCamera");
        }

        return false;
    }




    //적이 도착 후 7초 생존시 패배를 엄폐 상태에서는 시간 초기화
    IEnumerator SurvivalCountdown()
    {
        float timer = SaveTime;
        Debug.Log($"[Enemy:{gameObject.name}] 목적지 도달 → 생존 카운트 시작");

        while (timer > 0f)
        {
            if (isDead)
            {
                Debug.Log($"[Enemy:{gameObject.name}] 중간에 사망 → 생존 카운트 중단");
                yield break;
            }
            if (!IsPlayerVisible())
            {
                timer = SaveTime;
                Debug.Log($"[Enemy:{gameObject.name}] 플레이어 안 보임 → 타이머 초기화");
            }
            else
            {
                timer -= Time.deltaTime;
            }


            yield return null;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("[Enemy] GameManager.Instance is STILL null after 7s");
        }
        else
        {
            Debug.Log("[Enemy] GameManager.Instance 유효 → Notify 호출 시도");
            GameManager.Instance.NotifyEnemySurvived();
        }
    }



    public void Die()
    {
        if (isDead) return;

        isDead = true;

        //제거 보내기
        if (gameManager != null)
        {
            gameManager.NotifyEnemyKilled();
        }

        Destroy(gameObject);
    }

    
    // 총알 일때 썻던거
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Bullet"))
    //     {
    //         Die();
    //     }
    // }
}
