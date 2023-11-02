using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3;
using TMPro;
using UnityEngine;

public class MetaUI : MonoBehaviour
{
    #region COMPONENTS

    [SerializeField] private TextMeshProUGUI playLevelText;
    [SerializeField] private GameObject playButton;
    #endregion
    
    public static MetaUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        playButton.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.8f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void UpdateLevelText()
    {
        playLevelText.text = "Level " + Player.Instance.PlayerData.CurrentLevel;
    }
}
