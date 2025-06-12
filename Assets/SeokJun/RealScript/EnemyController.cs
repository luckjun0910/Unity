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

    System.Collections.IEnumerator SurvivalCountdown()
    {
        float timer = 7f;

        while (timer > 0f)
        {
            //적이 죽었으면 즉시종료
            if (isDead) yield break;

            timer -= Time.deltaTime;
            yield return null;
        }

        //살아남은 경우 -> 패배
        if (gameManager != null)
        {
            gameManager.NotifyEnemySurvived();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Die();
        }
    }
}
