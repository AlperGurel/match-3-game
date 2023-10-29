using System;
using UnityEngine;

namespace  Match3
{
    [CreateAssetMenu(fileName = "BlastSkillData", menuName = "Skills/Blast Skill")]
    class BlastSkillData : ScriptableObject, IBaseSkillData
    {
        #region VARIABLES
        public BlastType BlastType;
        public string Name = "BlastSkill";

        #endregion

        public string GetName()
        {
            return Name;
        }
    }
}