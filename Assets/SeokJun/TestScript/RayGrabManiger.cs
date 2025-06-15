// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.XR;
// using UnityEngine.XR.Interaction.Toolkit;

// // 레이저로 잡은 오브젝트 > 트리거로 잡음
// // A X B Y 로 앞뒤이동하고 아날로그 스틱으로 회전 줌
// // 그립 버튼으로 그자리 고정


// public class RayGrabManiger : MonoBehaviour
// {
//     // Ray Interactor 참조 (Inspector에서 연결)
//     public XRRayInteractor rayInteractor;

//     // 현재 잡은 오브젝트의 Rigidbody와 Transform
//     private Rigidbody grabbedRb;
//     private Transform grabbedTransform;

//     // 컨트롤러 입력용 장치
//     private InputDevice leftHand;
//     private InputDevice rightHand;

//     // 현재 이 스크립트가 왼손용인지 오른손용인지 구분
//     private bool isLeftController = false;

//     void Start()
//     {
//         // 게임 오브젝트 이름에 'left'가 들어가 있으면 왼손용으로 간주
//         if (name.ToLower().Contains("Left"))
//         {
//             isLeftController = true;
//             var devices = new List<InputDevice>();
//             InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
//             if (devices.Count > 0) leftHand = devices[0];
//         }
//         else
//         {
//             isLeftController = false;
//             var devices = new List<InputDevice>();
//             InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
//             if (devices.Count > 0) rightHand = devices[0];
//         }

//         // Ray로 잡을 때 실행될 이벤트(함수) 등록
//         rayInteractor.selectEntered.AddListener(OnGrab);

//         // Ray로 놓을 때 실행될 이벤트(함수) 등록
//         rayInteractor.selectExited.AddListener(OnRelease);
//     }

//     void Update()
//     {
//         Debug.Log("Update 실행 중");

//         if (grabbedRb == null)
//         {
//             Debug.Log("grabbedRb is null (잡고 있지 않음)");
//             return;
//         }
//         // 아무것도 잡고 있지 않으면 리턴
//         if (grabbedRb == null) return;

//         // 사용할 컨트롤러 장치 정리
//         InputDevice device = isLeftController ? leftHand : rightHand;

//         // // 버튼 상태 체크: A/X → 앞으로 이동
//         // //                B/Y → 뒤로 이동
//         // bool forwardPressed = false;
//         // bool backwardPressed = false;

//         // A or X 버튼: 앞으로
//         if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool forward) && forward)
//         {
//             //forwardPressed = primaryPressed;
//             //오브젝트를 컨트롤러 forward 방향으로
//             grabbedRb.MovePosition(grabbedRb.position + transform.forward * Time.deltaTime * 0.3f);
//         }


//         // B or Y 버튼: 뒤로
//         if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool backward) && backward)
//         {
//             // backwardPressed = secondaryPressed;
//             //오브젝트를 컨트롤러 뒤쪽 방향으로 이동
//             grabbedRb.MovePosition(grabbedRb.position + transform.forward * Time.deltaTime * 0.3f);
//         }

//         //아날로그 스틱 (좌우) 회전
//         if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis))
//         {
//             //좌우 입력되면 회전 시작
//             if (Mathf.Abs(axis.x) > 0.2f)
//             {
//                 float rotationSpeed = 60f; //회전 속도
//                 Quaternion deltaRot = Quaternion.Euler(0, axis.x * rotationSpeed * Time.deltaTime, 0);

//                 //회전 적용
//                 grabbedRb.MoveRotation(grabbedRb.rotation * deltaRot);
//             }
//         }
//     }

//     //레이저로 오브젝트 잡았을때 호출되는거
//     void OnGrab(SelectEnterEventArgs args)
//     {
//         // 잡은 오브젝트의 Transform, Rigidbody 가져오기
//         grabbedTransform = args.interactableObject.transform;
//         grabbedRb = grabbedTransform.GetComponent<Rigidbody>();

//         // 중력 해제 & 움직일 수 있게 설정
//         if (grabbedRb != null)
//         {
//             //중력 비활성화 내맘대로 움직임
//             grabbedRb.useGravity = false;

//             //물리적으로 시뮬ㄹ할수 있게 설정
//             grabbedRb.isKinematic = false;
//         }

//         // 오브젝트가 내 앞으로 끌려오지 않도록,
//         // Ray Interactor의 attachTransform을 현재 오브젝트 위치로 맞춰줌
//         if (rayInteractor.attachTransform != null)
//         {
//             rayInteractor.attachTransform.position = grabbedTransform.position;
//             rayInteractor.attachTransform.rotation = grabbedTransform.rotation;
//         }
//     }

//     // 레이저로 오브젝트를 놓았을떄 호출
//     void OnRelease(SelectExitEventArgs args)
//     {
//         if (grabbedRb == null) return;

//         // 어느 컨트롤러에서 놓았는지 선택
//         InputDevice device = isLeftController ? leftHand : rightHand;

//         // 그립 버튼을 누른 채로 놓았는지 확인
//         bool gripHeld = false;
//         device.TryGetFeatureValue(CommonUsages.gripButton, out gripHeld);        

//         if (gripHeld)
//         {
//             // 그립을 누른 채 놓았으면 오브젝트를 고정
//             grabbedRb.isKinematic = true;
//         }
//         else
//         {
//             // 그냥 놓았으면 중력 켜서 바닥으로 떨어지게
//             grabbedRb.useGravity = true;
//             grabbedRb.isKinematic = false;
//         }

//         // 상태 초기화
//         grabbedRb = null;
//         grabbedTransform = null;
//     }
// }



            
// ///////// 이전에 쓰던거 /////////
//         // // 이동 방향 결정
//         // Vector3 moveDirection = Vector3.zero;

//         // if (forwardPressed) moveDirection += transform.forward;   // 앞으로
//         // if (backwardPressed) moveDirection -= transform.forward; // 뒤로

//         // // 이동 실행
//         // if (moveDirection != Vector3.zero)
//         // {
//         //     // Time.deltaTime으로 프레임 보정, 속도 조절
//         //     grabbedRb.MovePosition(grabbedRb.position + moveDirection * Time.deltaTime * 0.5f);
//         // }

//         // // 아날로그 스틱 입력으로 회전
//         // if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis))
//         // {
//         //     // 좌우 입력이 일정 이상일 때만 회전
//         //     if (Mathf.Abs(axis.x) > 0.2f)
//         //     {
//         //         float rotateSpeed = 60f; // 초당 회전 속도 (도 단위)
//         //         Quaternion deltaRot = Quaternion.Euler(0, axis.x * rotateSpeed * Time.deltaTime, 0);
//         //         grabbedRb.MoveRotation(grabbedRb.rotation * deltaRot);
//         //     }
//         // }