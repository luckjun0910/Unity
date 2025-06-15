using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f; // 총알 속도
    public float lifeTime = 5f; // 자동 파괴 시간

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
        }

        Destroy(gameObject, lifeTime); // 일정 시간 후 제거
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"충돌: {other.gameObject.name}, 레이어: {LayerMask.LayerToName(other.gameObject.layer)}");

        // 엄폐물과 충돌 → 제거
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            Debug.Log("[Bullet] Object 레이어와 충돌 → 총알 제거");
            Destroy(gameObject);
            return;
        }

        // Enemy 레이어와 충돌 → 적 사망 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("[Bullet] Enemy 레이어 감지 → 적 제거 처리");

            // 적 오브젝트 제거
            Destroy(other.gameObject);

            // 총알도 제거
            Destroy(gameObject);
            return;
        }

        // 기타 충돌은 무시
        Debug.Log("[Bullet] 처리 대상 아님 → 무시");
    }
}
