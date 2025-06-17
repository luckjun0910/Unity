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
        popupText.text = "그립버튼: 오브젝트 잡기\n트리거 버튼: 총 발사\n A버튼: 재장전";
    }

    public void OnRuleButton()
    {
        popupPanel.SetActive(true);
        popupText.text = " 모든 적 제거 시 승리(15kill)\n 적이 도착 후 7초 생존 시 패배";
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
