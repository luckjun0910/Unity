using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;



public class GameStartCountdown : MonoBehaviour
{
    public Text countdownText;
    public GameObject countdownCanvas;
    [SerializeField] XRRayInteractor leftInteractor;
    [SerializeField] XRRayInteractor rightInteractor;

    [SerializeField] InteractionLayerMask interactLayerBeforeStart;

    void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        countdownCanvas.SetActive(true);

        if (leftInteractor != null && rightInteractor != null)
        {
            leftInteractor.interactionLayers = 0;
            rightInteractor.interactionLayers = 0;
        }
        else
        {
            Debug.LogError("[Countdown] 인터랙터가 연결되지 않았습니다!");
        }


        string[] countTexts = { "3", "2", "1", "시작!" };

        foreach (string t in countTexts)
        {
            countdownText.text = t;
            yield return new WaitForSeconds(1f);
        }

        leftInteractor.interactionLayers = interactLayerBeforeStart;
        rightInteractor.interactionLayers = interactLayerBeforeStart;

        countdownCanvas.SetActive(false);

        // 게임 시작 (BuildTimer 실행)
        BuildTimer buildTimer = FindObjectOfType<BuildTimer>();
        if (buildTimer != null)
        {
            buildTimer.StartBuildPhase();
        }
    }
}
