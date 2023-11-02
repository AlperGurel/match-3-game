using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Match3
{
    public class LevelFailedPopup : Popup
    {
        public void OnPlayAgainButtonClicked()
        {
            base.OnClose();       
            MatchManager.Instance.RestartMatch();
        }
        
        
        public override void OnClose()
        {
            MatchManager.Instance.SwitchToArea();
            base.OnClose();
        }
    }
}