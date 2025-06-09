using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public Transform shootPoint; // 총구 위치
    public float shootRange = 100f;
    public LayerMask hitLayer; // 적만 맞게 LayerMask

    [Header("Ammo Settings")] //일케하면 Inspector에서 보기 편하게 바꿀수 있음
    public int maxAmmo = 30; //장탄수
    private int currentAmmo; //현재 남은 장ㅌ난수
    private Text ammoText; //UI 지정

    private bool canShoot = true; //발사 가능한지

    void Awake()
    {
        var txtobj = GameObject.Find("AmmoText");
        if (txtobj != null)
            ammoText = txtobj.GetComponent<Text>();
        else
            Debug.Log("AmmoText 못찾고 잇음");
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
        // 트리거 누르면 발사
        if (isTriggerPressed)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!canShoot) return;

        if (currentAmmo <= 0)
        {
            Debug.Log("탄약 없음");
        }


        Debug.Log($"[레이] 시작:{shootPoint.position} 방향:{shootPoint.forward} 레이어마스크:{hitLayer.value}");
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

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

}
