using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverClip;
    public AudioClip clickClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverClip != null)
            audioSource.PlayOneShot(hoverClip);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickClip != null)
            audioSource.PlayOneShot(clickClip);
    }
}
