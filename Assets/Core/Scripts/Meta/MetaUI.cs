using System.Collections;
using System.Collections.Generic;
using Match3;
using TMPro;
using UnityEngine;

public class MetaUI : MonoBehaviour
{
    #region COMPONENTS

    [SerializeField] private TextMeshProUGUI playLevelText;
    #endregion
    
    public static MetaUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateLevelText()
    {
        playLevelText.text = "Level " + Player.Instance.PlayerData.CurrentLevel;
    }
}
