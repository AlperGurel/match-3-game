using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3;
using UnityEngine;

public class AreaTaskObject : MonoBehaviour
{
    #region COMPONENTS

    [SerializeField] private GameObject taskVisuals;
    #endregion

    #region VARIABLES

    private TaskButton taskButton;
    #endregion
    
    
    public void Initialize()
    {
        taskButton = GetComponentInChildren<TaskButton>();
        taskButton.AreaTaskObject = this;
    }
    
    public void UnlockTask()
    {
        var taskButton = GetComponentInChildren<TaskButton>();
        taskButton.TaskState = TaskState.UNLOCKED;
        taskButton.UpdateVisuals();
    }

    public void CompleteTask(bool showAnimation)
    {
        if (showAnimation)
        {
            taskVisuals.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            taskVisuals.SetActive(true);
            taskVisuals.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InSine);
        }
        else
        {
            taskVisuals.SetActive(true);
        }
    }
}
