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
        
        Main.Instance.FirstRowTransform.gameObject.SetActive(false);
        Main.Instance.SecondRowTransform.gameObject.SetActive(false);
        
        foreach (Transform children in Main.Instance.FirstRowTransform)
        {
            Destroy(children.gameObject);
        }
        
        foreach (Transform children in Main.Instance.SecondRowTransform)
        {
            Destroy(children.gameObject);
        }
        
        int row = 0;
        var objectives = MatchManager.Instance.LevelObjectives;
        foreach (var kv in objectives)
        {
            Transform rowTransform = null;
            if (row % 2 ==  0)
            {
                rowTransform = Main.Instance.FirstRowTransform;
            }
            else
            {
                rowTransform = Main.Instance.SecondRowTransform;
            }
            var factory = ItemFactoryManager.Instance.GetItemFactory(kv.Key);
            var objectiveUI = factory.CreateObjective(kv.Value, rowTransform);
            objectiveUIs[kv.Key] = objectiveUI;
            row++;
        }

        if (Main.Instance.FirstRowTransform.childCount > 0)
        {
            Main.Instance.FirstRowTransform.gameObject.SetActive(true);
        }
        
        if (Main.Instance.SecondRowTransform.childCount > 0)
        {
            Main.Instance.SecondRowTransform.gameObject.SetActive(true);
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
