using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 50f;//총알속도
    public float lifeTime = 5f;// 자동 파괴시간
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
        }

        Destroy(gameObject, lifeTime); // 일정 시간 후 제거
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position += transform.forward * speed * Time.deltaTime;
    }

    //맞히면 죽이기
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"충돌: {other.gameObject.name}");
        // 충돌한 오브젝트나 부모 중에서 EnemyController를 찾는다
        EnemyController enemy = other.GetComponentInParent<EnemyController>();

        // 못 찾으면 root 기준으로 강제 탐색
        if (enemy == null)
        {
            enemy = other.transform.root.GetComponent<EnemyController>();
            Debug.LogWarning($"[Bullet] GetComponentInParent 실패, root 기준 InChildren 시도 → {(enemy != null ? "성공" : "실패")}");
        }


        if (enemy != null)
        {
            Debug.Log("적 감지됨 -> 죽인다");
            enemy.Die();
            Destroy(gameObject); //총알 제거하는거임임
        }
        else
        {
            Debug.LogWarning($"[Bullet] EnemyController 못 찾음. 충돌 대상: {other.name}, Root: {other.transform.root.name}");;
        }

        
    }
}
