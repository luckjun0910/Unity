using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartUIManager : MonoBehaviour
{
    public GameObject popupPanel;
    public Text popupText;
    public GameObject quitPanel;
    public enum Difficulty { Easy, Normal, Hard }
    public static Difficulty selectedDifficulty = Difficulty.Normal;
    public GameObject difficultyPanel;


    public void OnStartButton()
    {
        difficultyPanel.SetActive(true);
    }

    public void OnHowToButton()
    {
        popupPanel.SetActive(true);
        popupText.text = "그립버튼(양손 중지):\n 오브젝트 잡기\n트리거 버튼(오른손 검지):\n 총 발사\n A버튼(오른손 아래):\n 재장전";
    }

    public void OnRuleButton()
    {
        popupPanel.SetActive(true);
        popupText.text = " 주어진 시간내에 진지구축\n 모든 적 제거 시 승리(15kill)\n 적이 도착 후 7초 생존 시 패배";
    }

    public void OnQuitButton()
    {
        quitPanel.SetActive(true);
    }

    public void OnClosePopup()
    {
        popupPanel.SetActive(false);
    }

    public void OnQuitYes()
    {
        Application.Quit(); // 종료
    }

    public void OnQuitNo()
    {
        quitPanel.SetActive(false); // 창 닫기
    }
    public void SelectEasy() { selectedDifficulty = Difficulty.Easy; StartCoroutine(DelayedSceneStart()); }
    public void SelectNormal() { selectedDifficulty = Difficulty.Normal; StartCoroutine(DelayedSceneStart()); }
    public void SelectHard() { selectedDifficulty = Difficulty.Hard; StartCoroutine(DelayedSceneStart()); }


    IEnumerator DelayedSceneStart()
    {
        //배경 음악 정지
        GameObject bgm = GameObject.Find("StartBGM");
        if (bgm != null)
        {
            AudioSource bgmAudio = bgm.GetComponent<AudioSource>();
            if (bgmAudio != null) bgmAudio.Stop();
        }
        yield return new WaitForSeconds(0.15f); // 클릭 소리 끝날 시간 확보
        SceneManager.LoadScene("SampleScene");
    }
}
