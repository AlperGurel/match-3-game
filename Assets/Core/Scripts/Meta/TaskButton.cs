using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


namespace Match3
{
    public class TaskButton : MonoBehaviour
    {
        #region COMPONENTS

        [SerializeField] private GameObject lockPrefab;
        [SerializeField] private GameObject hammerPrefab;
        #endregion
        
        
        #region VARIABLES

        private int TaskIndex;
        public TaskState TaskState { get; set; }
        public AreaTaskObject AreaTaskObject { get; set; }
        #endregion

        public void Start()
        {
            TaskState = TaskState.LOCKED;
        }
        
        private void OnMouseDown()
        {
            if (TaskState == TaskState.LOCKED)
            {
                lockPrefab.transform.DOShakePosition(0.3f, 0.4f, 10, 90);
            }

            if (TaskState == TaskState.UNLOCKED)
            {
                AreaTaskObject.CompleteTask(true);
                gameObject.SetActive(false);
            }
        }
        
        
        public void UpdateVisuals()
        {
            if (TaskState == TaskState.LOCKED)
            {
                hammerPrefab.SetActive(false);
                lockPrefab.SetActive(true);
            } else if (TaskState == TaskState.UNLOCKED)
            {
                hammerPrefab.SetActive(true);
                hammerPrefab.transform.DOScale(new Vector3(1.3f, 1.3f, 1f), 0.3f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
                lockPrefab.SetActive(false);
            } else if (TaskState == TaskState.COMPLETED)
            {
                hammerPrefab.SetActive(false);
                lockPrefab.SetActive(false);
            } else if (TaskState == TaskState.SHOWN_COMPLETED_ANIMATION)
            {
                hammerPrefab.SetActive(false);
                lockPrefab.SetActive(false);
            }
        }


    }
}