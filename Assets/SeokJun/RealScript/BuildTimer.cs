using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;


public class BuildTimer : MonoBehaviour
{
    //1분 동안 오브젝트 설치 시간 1분 (초단위임
    public float bulidTime = 60f;

    [Header("UI")]
    //타이버를 표시할 UI 넣기 
    public Text timerText;
    public Text AmmoText;

    //타이머 카운트 변수
    private float timer;
    public GameObject gunPrefab; // 손에 붙일 총 Prefab

    void Start()
    {
        //타이머를 설치 시간으로 초기화
        timer = bulidTime;

        //AmmoText 처음에 숨긺
        if (AmmoText != null)
            AmmoText.gameObject.SetActive(false);

        //코루틴으로 타이머 시작
        StartCoroutine(BuildPhase());
    }

    IEnumerator BuildPhase()
    {
        //timer가 0보다 클 때 까지 반복하게
        while (timer > 0)
        {
            //매 프레임마다 경과한 시간을 빼기
            timer -= Time.deltaTime;

            //UI에 남은 시간을 정수로
            if (timerText != null)
            {
                timerText.text = Mathf.Ceil(timer).ToString() + "s";
            }

            //1프레임 대기하기
            yield return null;
        }

        //1분이 지나면 오브젝트 고정
        LockAllObjects();
    }

    void LockAllObjects()
    {
        Rigidbody rb;
        // "BuildObject" 태그를 가진 모든 오브젝트를 찾기
        GameObject[] buildObjects = GameObject.FindGameObjectsWithTag("BuildObject");

        // 각 오브젝트마다 Rigidbody와 XRGrabInteractable 컴포넌트를 찾기
        foreach (GameObject obj in buildObjects)
        {
            // Rigidbody를 찾아서 isKinematic을 true로 설정 (물리적 움직임 차단)
            rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // XRGrabInteractable을 찾아서 비활성화 (Grab 불가)
            var grab = obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>();
            if (grab != null)
            {
                grab.enabled = false;
            }
        }

        // 디버그 로그로 확인
        Debug.Log("1분 경과! 오브젝트 고정 완료");

        if (timerText != null)
            timerText.gameObject.SetActive(false);

        if (AmmoText != null)
            AmmoText.gameObject.SetActive(true);

        // 총을 손에 생성
        Transform hand = GameObject.Find("Right Controller/Attach").transform;
        // Instantiate(gunPrefab, hand);
        GameObject gunInstance = Instantiate(gunPrefab, hand, false);
        /*
        // // hand.rotation에 Y축 180° 오프셋을 추가
        // Quaternion spawnRot = hand.rotation * Quaternion.Euler(0, 180, 0);

        // 위치, 회전, 부모 한 번에 지정
        // GameObject gunInstance = Instantiate(
        // gunPrefab,
        // handAttach.position,
        // handAttach.rotation,
        // handAttach         // 부모로 지정
        // );   

        // 부모 기준 로컬 위치,회전 다시 0으로
        // gunInstance.transform.localPosition = Vector3.zero;
        // gunInstance.transform.localRotation = Quaternion.identity;*/
        // Rigidbody 고정
        rb = gunInstance.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // 적 스폰 시작
        FindObjectOfType<SpawnManager>().StartSpawning();
        /*
        // 손 Transform 찾기 (이름 정확히 확인해라;; 띄어쓰기 조심)
        Transform hand = GameObject.Find("Right Controller").transform;

        // 총 생성 및 손 위치로 이동
        GameObject gunInstance = Instantiate(gunPrefab, hand);
        gunInstance.transform.SetParent(hand);
        gunInstance.transform.localPosition = Vector3.zero;
        gunInstance.transform.localRotation = Quaternion.identity;



        // Rigidbody가 있으면 isKinematic = true
        rb = gunInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        ==========================================================================
        // ray -> direct로 전환 하는거
        InteractorSwitcher rightSwitcher = GameObject.Find("Right Controller").GetComponent<InteractorSwitcher>();
        if (rightSwitcher != null)
        {
            Debug.Log("Right EnableDirectInteractor() 호출됨"); // 확인 로그
            rightSwitcher.EnableDirectInteractor();
        }

        //왼손도 바꾸기
        InteractorSwitcher leftSwitcher = GameObject.Find("Left Controller").GetComponent<InteractorSwitcher>();
        if (leftSwitcher != null)
        {
            Debug.Log("Left EnableDirectInteractor() 호출됨");
            leftSwitcher.EnableDirectInteractor();
        }

        // 총 생성
        // 1카메라 Transform 가져오기
        Transform cameraTransform = Camera.main.transform;

        // 카메라 앞 50cm 위치 계산
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * 0.5f;

        // 총 생성 (카메라 앞)
        GameObject gunInstance = Instantiate(gunPrefab, spawnPosition, cameraTransform.rotation);

        // 손의 Attach Transform 찾기 (RightHandGrab로 가정)
        Transform rightAttach = GameObject.Find("RightHandGrab").transform;

        // AttachTransform 위치로 이동 및 회전
        // gunInstance.transform.SetPositionAndRotation(rightAttach.position, rightAttach.rotation);

        // 손의 자식으로 붙이기
        // gunInstance.transform.SetParent(rightAttach, true);

        // Rigidbody 비활성화
        rb = gunInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        // 적 스폰 시작
        FindObjectOfType<SpawnManager>().StartSpawning();
        */
    }

    void Update()
    {

    }
}
