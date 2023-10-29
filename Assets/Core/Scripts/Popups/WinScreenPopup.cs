using System.Collections;
using System.Collections.Generic;
using Match3;
using UnityEngine;

namespace  Match3
{
    public class WinScreenPopup : Popup
    {
        public void OnTap()
        {
            Main.Instance.LoadMeta();
            base.OnClose();
        }
    }
}