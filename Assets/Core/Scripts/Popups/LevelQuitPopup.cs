using System.Collections;
using System.Collections.Generic;
using Match3;
using UnityEngine;

public class LevelQuitPopup : Popup
{
    public void OnReturnToAreaButtonClicked()
    {
        MatchManager.Instance.SwitchToArea();
        base.OnClose();
    }
}
