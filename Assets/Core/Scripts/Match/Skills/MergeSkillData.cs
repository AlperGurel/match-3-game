using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace Match3
{
    [CreateAssetMenu(fileName = "MergeSkillData", menuName = "Skills/Merge Skill")]
    public class MergeSkillData : ScriptableObject, IBaseSkillData
    {
        #region VARIABLES
        public string Name = "MergeSkill";
        public int MergeCount;
        public string ItemToCreate;
        public Sprite MergeSprite;
        #endregion 
        
        public string GetName()
        {
            return Name;
        }
    }
}