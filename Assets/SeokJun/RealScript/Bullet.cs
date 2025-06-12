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
        //일정 시간 후 총알 삭제
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //맞히면 죽이기
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Die();
            }
            Destroy(gameObject); //총알 제거하는거임임
        }
    }
}
