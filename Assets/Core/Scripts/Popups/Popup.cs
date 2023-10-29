using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Match3
{
    public class Popup : MonoBehaviour
    {
        public virtual void OnQuit()
        {
            
        }

        public virtual void OnClose()
        {
            Destroy(gameObject);
            PopupManager.Instance.ActivateBackground(false);
        }
    }
}
