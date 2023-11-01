using System.Collections;
using System.Collections.Generic;
using Match3;
using TMPro;
using UnityEngine;

public class MatchUI : MonoBehaviour
{
    public static MatchUI Instance { get; private set; }
    
    #region COMPONENTS
    [SerializeField] private TextMeshProUGUI moveCountText;
    #endregion
    
    #region VARIABLES

    private Dictionary<string, GameObject> objectiveUIs;
    #endregion
    

    private void Awake()
    {
        Instance = this;
        objectiveUIs = new Dictionary<string, GameObject>();
    }
    public void UpdateMoveCount(int moveCount)
    {
        moveCountText.text = moveCount.ToString();
    }

    public void OnQuitButtonClicked()
    {
        PopupManager.Instance.ShowPopup<LevelQuitPopup>();
    }


    public void CreateObjectivesUI()
    {
        objectiveUIs.Clear();
        
        foreach (Transform children in Main.Instance.FirstRowTransform)
        {
            Destroy(children.gameObject);
        }
        
        foreach (Transform children in Main.Instance.SecondRowTransform)
        {
            Destroy(children.gameObject);
        }
        
        var objectives = MatchManager.Instance.LevelObjectives;
        foreach (var kv in objectives)
        {
            var factory = ItemFactoryManager.Instance.GetItemFactory(kv.Key);
            var objectiveUI = factory.CreateObjective(kv.Value);
            objectiveUIs[kv.Key] = objectiveUI;
        }
    }

    public void UpdateObjectivesUI()
    {
        foreach (var kv in MatchManager.Instance.LevelObjectives)
        {
            var objectiveUI = objectiveUIs[kv.Key];
            objectiveUI.GetComponent<ObjectiveUI>().UpdateObjectiveCount(kv.Value);
        }
    }
}
