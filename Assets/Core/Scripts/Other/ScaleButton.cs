using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;


public class ScaleButton : MonoBehaviour
{
    [Header("Scale Settings")]
    public Vector3 normalScale = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 touchScale = new Vector3(1.1f, 1.1f, 1.1f);
    public float scaleDuration = 0.2f;

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;

        // Attach the event listeners for touch events to trigger the scale animation.
        ButtonAnimationSetup();
    }

    private void ButtonAnimationSetup()
    {
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }

        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        var trigger = gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        entry.callback.AddListener((data) => OnPointerDown());
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        entry.callback.AddListener((data) => OnPointerUp());
        trigger.triggers.Add(entry);
    }

    private void OnPointerDown()
    {
        // Scale up on touch down
        transform.DOScale(touchScale, scaleDuration);
    }

    private void OnPointerUp()
    {
        // Scale down on touch up
        transform.DOScale(initialScale, scaleDuration);
    }

    private void OnButtonClick()
    {
        // You can add custom button click logic here.
    }
}
