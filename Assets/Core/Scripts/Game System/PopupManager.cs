using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : SingletonGameSystem<PopupManager>
{
    #region VARIABLES

    private Image backgroundImage;
    #endregion

    public override void Initialize()
    {
        backgroundImage = Main.Instance.PopupBackground.GetComponent<Image>();
        base.Initialize();
    }


    public void ActivateBackground(bool toggle)
    {
        backgroundImage.gameObject.SetActive(toggle);
    }

    public void ShowPopup<T>() where T : Popup
    {
        string typeName = typeof(T).Name;
        GameObject popupPrefab = Resources.Load<GameObject>(typeName);

        if (popupPrefab != null)
        {
            ActivateBackground(true);
            GameObject popupInstance = GameObject.Instantiate(popupPrefab, Main.Instance.PopupCanvas);
        }
    } 
}
