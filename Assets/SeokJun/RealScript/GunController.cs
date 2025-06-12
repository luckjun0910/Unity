using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("Shooting")]
    public Transform shootPoint; // 총구 위치
    public float shootRange = 100f; // 총알이 날아가는 최대 거리
    public LayerMask hitLayer; // 적만 맞게 LayerMask

    [Header("Fire Rete Settings")]
    public float fireRate = 0.5f; //발사 간격
    private float nextFireTime = 0f; //다음 발사가 가능한 시간


    [Header("Ammo Settings")] //일케하면 Inspector에서 보기 편하게 바꿀수 있음
    public int maxAmmo = 30; //장탄수
    private int currentAmmo; //현재 남은 장ㅌ난수
    private Text ammoText; //UI 지정
    private bool canShoot = true; //발사 가능한지

    [Header("Reload Settings")]
    public float reloadDuration = 3f; //재장전 걸리는 시간
    private bool isReloading = false; //장전중인지 여부
    private Text reloadText; //장전 ui

    [Header("Bullet Settings")]
    public GameObject bulletPrefab; // 총알 연결하셈
    public float bulletSpeed = 50f; //총알 속도




    void Awake()
    {
        var txtobj = GameObject.Find("AmmoText");
        if (txtobj != null)
            ammoText = txtobj.GetComponent<Text>();
        else
            Debug.Log("AmmoText 못찾고 잇음");

        // ReloadText는 비활성화 상태일 수 있으므로 Canvas에서 transform.Find로 찾기
        GameObject canvasObj = GameObject.Find("Canvas"); // Canvas를 찾는다
        if (canvasObj != null)
        {
            // Canvas의 transform에서 자식 ReloadText를 찾는다 (비활성화 상태여도 찾음)
            Transform reloadTransform = canvasObj.transform.Find("ReloadText");
            if (reloadTransform != null)
            {
                reloadText = reloadTransform.GetComponent<Text>();
            }
            else
            {
                Debug.Log("ReloadText를 Canvas에서 못찾음");
            }
        }
        else
        {
            Debug.Log("Canvas를 못찾음");
        }

    }

    private void Start()
    {
        //탄약 초기화
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    private void Update()
    {
        // 트리거 입력값 받아오고
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed);

        //리로드버튼 or 탄 다쐈을때
        device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isReloadPressed);
        if (!isReloading && (isReloadPressed || currentAmmo <= 0))
        {
            StartCoroutine(Reload());
            return;  // 장전 시작하면 발사는 잠시 금지
        }

        if (isReloading)
            return;  // 장전 중엔 발사 금지

        // 발사 딜레이를 검사해서 발사가능 시간일 때만 발사
        if (isTriggerPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; //다음 발사 가능시간 갱신
        }
    }

    public void Shoot()
    {
        if (!canShoot || isReloading ||currentAmmo <=0) return;
        /*
        // if (currentAmmo <= 0)
        // {
        //     Debug.Log("탄약 없음");
        // }


        // Debug.Log($"[레이] 시작:{shootPoint.position} 방향:{shootPoint.forward} 레이어마스크:{hitLayer.value}");*/

        /* //레이로 발사 할때때
        // 레이 방향 시각화 (디버그 용)
        Debug.DrawRay(shootPoint.position, shootPoint.forward * shootRange, Color.red, 1f);
        Debug.Log("발사됨!");  // 확인용

        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        //모든 충돌체 검사
        if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
        {
            Debug.Log("맞춘 대상: " + hit.collider.gameObject.name);
            
            // 맞춘 대상이 적 레이어인지 검사하고 적만 제거하기
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                Destroy(hit.collider.gameObject);// 맞춘 오브젝트 제거
        }
        else
        {
            Debug.Log("빗나감");
        } */

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        

        currentAmmo--;
        UpdateAmmoUI();
    }

    IEnumerator Reload()
    {
        isReloading = true;

        //재장전 시작시 장탄수 순기기
        if (ammoText != null)
            ammoText.gameObject.SetActive(false);
        

        if (reloadText != null)
        {
            reloadText.gameObject.SetActive(true);
            float t = reloadDuration;
            while (t > 0f)
            {
                reloadText.text = $"재장전 중... {t:F1}s";
                t -= Time.deltaTime;
                yield return null;
            }
            reloadText.text = "재장전 중... 0.0s";
        }

        yield return new WaitForSeconds(0.1f); // 화면에 “0.0s”가 보이게 잠깐 대기

        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        //재장전이 끝나면 다시 장탄수 보이게
        if (ammoText != null)
            ammoText.gameObject.SetActive(true);
        
        

        if (reloadText != null)
            reloadText.gameObject.SetActive(false);


        isReloading = false;
    }


    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }
    
    

}
