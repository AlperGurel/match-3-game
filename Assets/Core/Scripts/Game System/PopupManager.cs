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
        if (toggle)
        {
            backgroundImage.DOFade(0, 0);
            backgroundImage.DOFade(0.6f, 0.4f);
        }
        else
        {
            backgroundImage.DOFade(0, 0);
        }
    }

    public void ShowPopup<T>() where T : Popup
    {
        string typeName = typeof(T).Name;
        GameObject popupPrefab = Resources.Load<GameObject>(typeName);

        if (popupPrefab != null)
        {
            ActivateBackground(true);
            GameObject popupInstance = GameObject.Instantiate(popupPrefab, Main.Instance.PopupCanvas);
            popupInstance.SetActive(false);
            
            var initialScale = popupInstance.transform.localScale;
            popupInstance.transform.localScale = initialScale * 0.7f;
            var initialPos = popupInstance.transform.position;
            popupInstance.transform.position += (Vector3.down * 10);
            popupInstance.SetActive(true);
            
            
            popupInstance.transform.DOMove(initialPos, 0.3f);
            popupInstance.transform.DOScale(initialScale, 0.2f).SetEase(Ease.InOutBounce);

        }
    } 
}
