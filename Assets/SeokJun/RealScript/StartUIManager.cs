using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartUIManager : MonoBehaviour
{
    public GameObject popupPanel;
    public Text popupText;

    public void OnStartButton()
    {
        //배경 음악 정지
        GameObject bgm = GameObject.Find("StartBGM");
        if (bgm != null)
        {
            AudioSource bgmAudio = bgm.GetComponent<AudioSource>();
            if (bgmAudio != null) bgmAudio.Stop();
        }
        StartCoroutine(DelayedSceneStart());
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
        Application.Quit();
    }

    public void OnClosePopup()
    {
        popupPanel.SetActive(false);
    }

    IEnumerator DelayedSceneStart()
    {
        yield return new WaitForSeconds(0.15f); // 클릭 소리 끝날 시간 확보
        SceneManager.LoadScene("SampleScene");
    }
}
