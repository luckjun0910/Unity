# 스마트 진지구축 훈련 (Smart Reserve Army)

**카테고리:** 쾌적한 환경에서의 훈련  
**팀:** 홍석준  
**기간:** 2025년 5월 - 2025년 6월  
**사용 기술:** Unity, VR, XR, C#, META

---

## 프로젝트 소개

스마트 진지구축 훈련(Smart Reserve Army)은 기존의 낙후된 예비군 훈련장을 개선하기 위해 개발한 **VR 기반 시뮬레이션 훈련**입니다.  
기존 훈련장은 접근성이 낮고 외부 환경(날씨 등)에 따라 영향을 받았지만, 이 프로젝트를 통해 쾌적한 사무실이나 실내 공간에서 몰입도 높은 훈련을 받을 수 있습니다.

**주요 목적:**
- 비상 상황 발생 시 대응 역량 향상
- 안전하고 실감나는 전투 시뮬레이션
- 정부 기관, 직장, 학교 등에서 손쉽게 VR 훈련 제공
- 기존 훈련의 한계를 극복하고, 참여율을 높이는 방안 모색

훈련은 단순 이론이 아니라, 실제 전장과 유사한 가상 환경에서 **적 침투 대응 및 진지 구축**을 실습할 수 있도록 설계되었습니다.

---

## 작업 내용

- Unity 2022.3.6f1 기반 프로젝트를 새롭게 구성
- **XR Plugin Management**를 통해 **Oculus XR** 설정
- XR Interaction Toolkit을 사용해 **양손 컨트롤러** 기반 상호작용 구현
- Quest 2와 PC Link 환경에서 플레이어와 오브젝트 상호작용
- 오브젝트 배치 및 진지 구축 시스템 개발 (나무박스, 나무 판자, 등으로 진지 구성)
- 특정 시간(1~2분) 이후 적군 등장 및 사격 시스템 설계 (적을 15초 내 제압 실패 시 게임 종료)
- 10명의 적군을 처치하면 게임 승리 처리
- Oculus Quest Link 연결 문제 해결 및 윈도우 사용자 계정 한글 이름 문제까지 해결
- Grab Interactable / XR Direct & Ray Interactor 기반의 그립 동작 세팅
- 사용자 경험 최적화를 위해 Track Position/Rotation 옵션과 직접 만든 스크립트 테스트
- Git을 사용해 Unity 프로젝트를 GitHub와 연동 (버전 관리 및 백업)

---

## 개발 환경 및 요구사항

- Unity 버전: 2022.3.62f1
- VR 디바이스: Meta Quest 2 (오큘러스)
- 플랫폼: Meta Quest / Windows PC (Link Cable)
- [Meta XR All-in-One SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657)
- Unity XR Interaction ToolKit

---

## 팀원

- 홍석준 [팀장]  
  - VR 진지 구축 시뮬레이션 시스템 설계 및 구현  
  - Unity 기반 3D 환경과 인터랙션 개발  
  - 적 침투 감지 알고리즘 설계 및 최적화  
  - 사용자 경험 최적화

---

## 사용 에셋

- [Sandbags Cover](https://assetstore.unity.com/packages/3d/environments/sandbags-cover-7834)  
- [Covered Cars Free](https://assetstore.unity.com/packages/3d/props/exterior/covered-cars-free-74510)  
- [Wood Box Pack (15 Objects)](https://assetstore.unity.com/packages/3d/props/industrial/wood-box-pack-15-objects-105811)  
- [Low Poly Soldiers Demo](https://assetstore.unity.com/packages/3d/characters/low-poly-soldiers-demo-73611)  
- [M16 A1 Rifle](https://assetstore.unity.com/packages/3d/props/weapons/m16-a1-rifle-182512)

---

## 저장소

- [프로젝트 GitHub 저장소](https://github.com/luckjun0910/Unity)
- 프로젝트가 진행되고 있는 저장소 : SeokJun HOME
- 프로젝트가 돌아다니는 저장소 : SeokJun USB

---

## 실행 방법

1. Unity 2022.3.6f1 이상으로 프로젝트 열기  
2. XR Plugin Management에서 Oculus 설정을 확인  
3. Meta Quest 2 (또는 오큘러스 디바이스)를 PC와 연결  
4. Oculus Link 실행 및 Unity Editor에서 Play 모드로 VR 실행  
5. VR 헤드셋 내에서 손 컨트롤러로 진지 구축 및 적군 사격 훈련

---

## Unity 및 Git 연동 방법

Unity 프로젝트를 Git으로 관리하며, `.gitignore`를 활용해 불필요한 파일을 무시하고 있습니다.  

### 주요 내용
- Unity에서 **버전 관리**를 위해:  
  - **Edit > Project Settings > Editor**   
    - Asset Serialization: **Force Text**  
  - 이렇게 하면 Unity 에셋의 변경 내용이 Git으로 명확히 관리됩니다.

- **.gitignore**를 사용해, 아래처럼 불필요한 파일/폴더를 무시:  
  - `Library/`, `Temp/`, `Obj/`  
  - `.sln`, `.csproj` 등 빌드 시 자동 생성 파일  
  - OS, IDE별 임시 파일 (예: `.DS_Store`, `.idea/`)

- **.gitignore 초기 설정 방법**  
  - Unity 프로젝트 폴더에서 `.gitignore` 파일을 생성 (새로만들기 > 텍스트파일)
  - 아래 내용을 복사해 `.gitignore`에 붙여넣기:  
    ```text
    [Ll]ibrary/
    [Tt]emp/
    [Bb]uild/
    [Oo]bj/
    [Bb]in/
    [Uu]ser[Ss]ettings/
    *.csproj
    *.unityproj
    *.sln
    *.suo
    *.tmp
    *.user
    *.userprefs
    *.pidb
    *.booproj
    *.svd
    *.pdb
    *.mdb
    *.opendb
    *.VC.db
    *.pidb.meta

    # OS generated files
    .DS_Store
    *.swp
    *.swo
    *.tmp
    ehthumbs.db
    Icon?
    Thumbs.db

    # Rider
    .idea/
    *.sln.iml
    ```
  - Git에 반영:  
    ```bash
    git add .gitignore
    git commit -m "Add .gitignore for Unity project"
    git push
    ```

- **Git 명령어**  
  - 변경사항을 Git에 추가:  
    ```bash
    git add .
    git commit -m "작업 내용"
    git push
    ```
  - 다른 PC에서 최신 상태로 유지:  
    ```bash
    git pull
    ```
  - 다른 PC에서 사용 후 인증서 삭제
    ```bash
    git credential-cache exit
    ```

- **협업 주의사항**  
  - 다른 사람이 작업 후 `push`를 했으면, 내 PC에서 반드시 `git pull` 후 새로 작업하기  

---
