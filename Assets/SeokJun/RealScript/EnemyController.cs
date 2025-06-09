using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
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
    }

    // Update()는 매 프레임마다 호출됨
    void Update()
    {
        // NavMeshAgent의 속도를 기반으로 "IsRunning" 파라미터를 true/false로 설정
        // -> 적이 이동 중이면 true (Run 애니메이션 재생), 멈췄으면 false (Idle 애니메이션 재생)
        animator.SetBool("IsRunning", agent.velocity.magnitude > 0.1f);

        // 목적지에 거의 도착했을 때 (remainingDistance가 stoppingDistance보다 작거나 같을 때)
        // 그리고 현재 재생 중인 애니메이션이 "demo_combat_shoot"가 아니라면
        // "Shoot" 트리거를 발동 (Shoot 애니메이션으로 전환)
        if (agent.remainingDistance <= agent.stoppingDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("demo_combat_shoot"))
        {
            animator.SetTrigger("Shoot");
        }
    }
}
