using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "LinkSkillData", menuName = "Skills/Link Skill")]
    public class LinkSkillData : ScriptableObject, IBaseSkillData
    {
        #region VARIABLES

        public string Name = "LinkSkill";
        #endregion
        
        public string GetName()
        {
            return Name;
        }
    }
}
