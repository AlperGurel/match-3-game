using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Match3
{
    [CreateAssetMenu(fileName = "TNT Item Factory", menuName = "TNT Item Factory")]
    public class TNTItemFactory : ItemFactory
    {
        #region VARIABLES

        // [SerializeField] private string itemName;
        // [SerializeField] private GameObject itemPrefab;
        // [SerializeField] private string dataId;
        // [SerializeField] private Sprite objectiveSprite;
        // [SerializeField] private bool isObjective;
        // [SerializeField] private bool canFall;
        // [SerializeField] public List<ScriptableObject> SkillData;
        #endregion


        #region PROPERTIES

        // public string ItemName => itemName;
        // public GameObject ItemPrefab => itemPrefab;
        // public bool IsObjective => isObjective;
        // public string DataId => dataId;

        #endregion

        public override Item CreateItem()
        {
            GameObject createdItem = Instantiate(ItemPrefab);
            var skills = GetSkills();
            Item item = new TNTItem(dataId, itemName, createdItem, skills, canFall);
            return item;
        }



        // public GameObject CreateObjective(int initialObjectiveCount)
        // {
        //     GameObject objectiveUi = Instantiate(Main.Instance.ObjectivePrefab, Main.Instance.FirstRowTransform);
        //     objectiveUi.GetComponent<ObjectiveUI>().Initialize(initialObjectiveCount, objectiveSprite);
        //     return objectiveUi;
        // }
    }
}