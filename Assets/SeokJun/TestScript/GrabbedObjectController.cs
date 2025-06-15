// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.XR;
// using UnityEngine.XR.Interaction.Toolkit;

// //XR Grab Interactable로 잡은 오브젝트를 조작하는 스크립트.
// //컨트롤러 입력을 통해
// //스틱으로 좌우 이동 / 위아래 회전
// //A/X 버튼으로 플레이어 방향으로 당김
// //B/Y 버튼으로 멀리 밀기
// public class GrabbedObjectController : MonoBehaviour
// {
//     // XR Grab Interactable 컴포넌트 연결
//     public XRGrabInteractable grabInteractable;

//     // 회전/이동 속도 설정
//     public float rotateSpeed = 100f;
//     public float moveSpeed = 0.3f;

//     // 현재 오브젝트를 잡고 있는 컨트롤러의 Transform
//     private Transform interactor;

//     // 오브젝트가 잡힌 상태인지 여부
//     private bool isHeld = false;

//     // 컨트롤러의 XR InputDevice
//     private InputDevice device;

//     // 왼손 컨트롤러인지 ?
//     private bool isLeftHand = false;

//     // 오브젝트를 처음 잡을 때 손의 위치를 저장
//     private Vector3 grabStartHandPosition;

//     // 오브젝트를 처음 잡을 때의 위치를 저장
//     private Vector3 grabStartObjectPosition;


//     // 오브젝트가 활성화될 때 이벤트
//     void OnEnable()
//     {
//         grabInteractable.selectEntered.AddListener(OnGrabbed);   // 잡기
//         grabInteractable.selectExited.AddListener(OnReleased);   // 놓기
//     }


//     // 오브젝트가 비활성화될 때 이벤트 제거
//     void OnDisable()
//     {
//         grabInteractable.selectEntered.RemoveListener(OnGrabbed);
//         grabInteractable.selectExited.RemoveListener(OnReleased);
//     }


//     // 오브젝트잡았을 때 호출됨
//     void OnGrabbed(SelectEnterEventArgs args)
//     {
//         // 잡은 컨트롤러의 Transform 저장 
//         interactor = args.interactorObject.transform;
//         isHeld = true;

//         // 컨트롤러가 왼손인지 오른손인지 이름으로 판단
//         isLeftHand = interactor.name.ToLower().Contains("left");

//         // 잡은 순간의 손 위치와 오브젝트 위치를 저장
//         grabStartHandPosition = interactor.position;
//         grabStartObjectPosition = transform.position;

//         // 해당 손의 XR InputDevice 가져오기
//         var devices = new List<InputDevice>();
//         InputDevices.GetDevicesAtXRNode(isLeftHand ? XRNode.LeftHand : XRNode.RightHand, devices);
//         if (devices.Count > 0)
//         {
//             device = devices[0];
//         }
//     }


//     // 오브젝트가 놓였을 때 호출됨
//     void OnReleased(SelectExitEventArgs args)
//     {
//         interactor = null;
//         isHeld = false;
//     }



//     // 매 프레임마다 오브젝트를 손 위치 변화에 따라 이동/회전시키는 메서드
//     void Update()
//     {
//         if (!isHeld || interactor == null)
//             return;

//         // interactor가 왼손인지 오른손인지 판별
//         XRNode handNode = interactor.name.ToLower().Contains("left") ? XRNode.LeftHand : XRNode.RightHand;

//         // 해당 손의 InputDevice 가져오기
//         var devices = new List<InputDevice>();
//         InputDevices.GetDevicesAtXRNode(handNode, devices);
//         if (devices.Count == 0) return;

//         var activeDevice = devices[0]; // 잡은 손 기준 디바이스

//         // 입력값 미리 받아오기 (중복 없이)
//         activeDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 stick);
//         activeDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool aOrX);
//         activeDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool bOrY);
//         activeDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger);
//         activeDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool grip);

//         // 스틱 회전 입력
//         // 좌우 -> Y축 회전 (돌리기)
//         transform.Rotate(Vector3.up, stick.x * rotateSpeed * Time.deltaTime, Space.World);

//         // 위아래 -> X축 회전 (세우기/눕히기)
//         transform.Rotate(Vector3.right, -stick.y * rotateSpeed * Time.deltaTime, Space.Self);

//         // A/X 버튼 -> 당기기 (플레이어 쪽으로)
//         if (aOrX)
//             transform.position -= interactor.forward * moveSpeed;

//         // B/Y 버튼 -> 밀기 (멀어짐)
//         if (bOrY)
//             transform.position += interactor.forward * moveSpeed;

//         // 디버그 로그 출력
//         // 어떤 키를 눌렀는지에 따라 설명 로그 출력
//         if (aOrX)
//             Debug.Log("[입력 감지됨] A/X 버튼 눌림");
//         if (bOrY)
//             Debug.Log("[입력 감지됨] B/Y 버튼 눌림");
//         if (trigger)
//             Debug.Log("[입력 감지됨] 트리거 버튼 누름");
//         if (grip)
//             Debug.Log("[입력 감지됨] 그립 버튼 누름");
//         if (stick != Vector2.zero)
//             Debug.Log($"[입력 감지됨] 스틱 움직임: {stick}");
//     }

// }
