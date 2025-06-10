using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("Shooting")]
    public Transform shootPoint; // 총구 위치
    public float shootRange = 100f;
    public LayerMask hitLayer; // 적만 맞게 LayerMask


    [Header("Ammo Settings")] //일케하면 Inspector에서 보기 편하게 바꿀수 있음
    public int maxAmmo = 30; //장탄수
    private int currentAmmo; //현재 남은 장ㅌ난수
    private Text ammoText; //UI 지정
    private bool canShoot = true; //발사 가능한지

    [Header("Reload Settings")]
    public float reloadDuration = 3f; //재장전 걸리는 시간
    private bool isReloading = false; //장전중인지 여부
    private Text reloadText; //장전 ui


    void Awake()
    {
        var txtobj = GameObject.Find("AmmoText");
        if (txtobj != null)
            ammoText = txtobj.GetComponent<Text>();
        else
            Debug.Log("AmmoText 못찾고 잇음");

        //재장전 ui text 찾기
        var reloadobj = GameObject.Find("ReloadText");
        if (reloadobj != null)
            reloadText = reloadobj.GetComponent<Text>();
        else
            Debug.Log("ReloadText 못찾고 있음");

    }

    private void Start()
    {
        //탄약 초기화
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        if (reloadText != null)
            reloadText.gameObject.SetActive(false); //처음에는 비활성화
        else
            Debug.Log("reloadTet를 찾을 수 없습니다.");

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

        // 트리거 누르면 발사
        if (isTriggerPressed)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!canShoot) return;

        // if (currentAmmo <= 0)
        // {
        //     Debug.Log("탄약 없음");
        // }


        // Debug.Log($"[레이] 시작:{shootPoint.position} 방향:{shootPoint.forward} 레이어마스크:{hitLayer.value}");
        // 레이 방향 시각화 (디버그 용)
        Debug.DrawRay(shootPoint.position, shootPoint.forward * shootRange, Color.red, 1f);
        Debug.Log("발사됨!");  // 확인용
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, shootRange, hitLayer))
        {
            Debug.Log("맞춘 대상: " + hit.collider.gameObject.name);
            Destroy(hit.collider.gameObject);// 맞춘 오브젝트 제거
        }
        else
        {
            Debug.Log("빗나감");
        }

        currentAmmo--;
        UpdateAmmoUI();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        if (reloadText != null)
        {
            reloadText.gameObject.SetActive(true);
            float t = reloadDuration;
            while (t > 0f)
            {
                reloadText.text = $"Reloading... {t:F1}s";
                t -= Time.deltaTime;
                yield return null;
            }
            reloadText.text = "Reloading... 0.0s";
        }

        yield return new WaitForSeconds(0.1f); // 화면에 “0.0s”가 보이게 잠깐 대기

        currentAmmo = maxAmmo;
        UpdateAmmoUI();

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
