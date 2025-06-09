using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // 카메라 앞에 위치할 거리 (1~2m 추천)
    public float distance = 2f;

    // 카메라와 UI의 높이(Y) 차이를 그대로 유지할지 여부
    public bool lockY = true;

    void LateUpdate()
    {
        // 메인 카메라가 존재할 때만 실행
        if (Camera.main != null)
        {
            // 카메라의 forward 방향으로 distance만큼 떨어진 위치 계산
            Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * distance;

            // Y축을 고정할지 여부에 따라 위치 조정
            if (lockY)
            {
                targetPosition.y = transform.position.y; // Canvas 높이를 유지
            }

            // Canvas 위치 업데이트
            transform.position = targetPosition;

            // Canvas가 카메라를 항상 정면으로 바라보도록 회전
            transform.LookAt(Camera.main.transform);

            // LookAt으로 인해 뒤로 보일 수 있어서 180도 회전 보정
            transform.Rotate(0, 180, 0);
        }
    }
}
