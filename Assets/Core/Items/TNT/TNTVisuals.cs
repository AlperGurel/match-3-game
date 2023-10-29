using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTVisuals : MonoBehaviour
{
    #region COMPONENTS

    [SerializeField] private Animation animation;
    [SerializeField] private GameObject mergeAnimationObject;
    [SerializeField] private SpriteRenderer mergeAnimationRenderer;
    [SerializeField] private SpriteRenderer mainSpriteRenderer;

    #endregion


    public void StartMergeReadyAnimation()
    {
        mainSpriteRenderer.enabled = false;
        mergeAnimationObject.SetActive(true);
        animation.Play();
    }

    public void StopMergeReadyAnimation()
    {
        mainSpriteRenderer.enabled = true;
        mergeAnimationObject.SetActive(false);
        animation.Stop();
    }

    public void SetSortingOrder(int indexY)
    {
        mergeAnimationRenderer.sortingOrder = indexY;
    }
}
