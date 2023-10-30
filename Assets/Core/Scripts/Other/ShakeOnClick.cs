using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ShakeOnClick : MonoBehaviour, IPointerClickHandler
{
    public float duration = 0.5f; // Duration of the shake animation
    public float strength = 1.0f; // Strength of the shake
    public int vibrato = 10; // Number of vibrations in the shake
    public float randomness = 90.0f; // Randomness of the shake



    private void Awake()
    {
        
    }

    private void OnMouseDown()
    {
        transform.DOShakePosition(duration, strength, vibrato, randomness);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
