using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3;
using UnityEngine;

namespace  Match3
{
    public class WinScreenPopup : Popup
    {
        #region COMPONENTS

        [SerializeField] private GameObject starVisual;
        [SerializeField] private GameObject text;
        #endregion
        

        private async void Start()
        {
            starVisual.transform.DORotate(new Vector3(0, 0, 0), 0.6f, RotateMode.FastBeyond360);
            starVisual.transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.Flash);
            await Waiter.WaitForSeconds(0.6f);
            text.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 1)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
            
        }

        public void OnTap()
        {     
            MetaUI.Instance.UpdateLevelText();
            AreaManager.Instance.OnLevelWin();
            Main.Instance.LoadMeta();
            MatchManager.Instance.Board.DestroyBoard();
            base.OnClose();
        }
    }
}