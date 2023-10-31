using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Match3;
using UnityEngine;

public class TNTVisuals : MonoBehaviour
{
    #region COMPONENTS

    [SerializeField] private Animation animation;
    [SerializeField] private GameObject mergeAnimationObject;
    [SerializeField] private SpriteRenderer mergeAnimationRenderer;
    [SerializeField] private SpriteRenderer mainSpriteRenderer;

    [Header("TNT + TNT Combo")] [SerializeField]
    private Animation tntComboAnimation;

    [SerializeField]private  GameObject tntComboAnimObject;
    #endregion


    public async Task PlayTntComboAnimation()
    {
        tntComboAnimObject.SetActive(true);
        await Waiter.WaitForSeconds(tntComboAnimation.clip.length);
        tntComboAnimObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    
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
