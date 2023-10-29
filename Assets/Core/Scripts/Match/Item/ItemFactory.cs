using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Match3
{
    [CreateAssetMenu(fileName = "New Item Factory", menuName = "Item Factory")]
    public class ItemFactory : ScriptableObject
    {
        #region VARIABLES

        [SerializeField] private string itemName;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private string dataId;
        [SerializeField] private Sprite objectiveSprite;
        [SerializeField] private bool isObjective;
        [SerializeField] private bool canFall;
        [SerializeField] public List<ScriptableObject> SkillData;
        #endregion


        #region PROPERTIES

        public string ItemName => itemName;
        public GameObject ItemPrefab => itemPrefab;
        public bool IsObjective => isObjective;
        public string DataId => dataId;

        #endregion

        public virtual Item CreateItem()
        {
            GameObject createdItem = Instantiate(ItemPrefab);
            var skills = GetSkills();
            Item item = new Item(dataId, itemName, createdItem, skills, canFall);
            return item;
        }

        private List<ISkill> GetSkills()
        {
            var skills = new List<ISkill>();
            foreach (var skillData in SkillData)
            {
                var baseSkillData = (IBaseSkillData)skillData;
                switch (baseSkillData.GetName())
                {
                    case "HealthSkill":
                        HealthSkill healthSkill = new HealthSkill();
                        healthSkill.Initialize(baseSkillData);
                        skills.Add(healthSkill);
                        break;
                    case "LinkSkill":
                        LinkSkill linkSkill = new LinkSkill();
                        linkSkill.Initialize(baseSkillData);
                        skills.Add(linkSkill);
                        break;
                    case "BlastSkill":
                        BlastSkill blastSkill = new BlastSkill();
                        blastSkill.Initialize(baseSkillData);
                        skills.Add(blastSkill);
                        break;
                }
            }

            return skills;
        }

        public GameObject CreateObjective(int initialObjectiveCount)
        {
            GameObject objectiveUi = Instantiate(Main.Instance.ObjectivePrefab, Main.Instance.FirstRowTransform);
            objectiveUi.GetComponent<ObjectiveUI>().Initialize(initialObjectiveCount, objectiveSprite);
            return objectiveUi;
        }
    }
}