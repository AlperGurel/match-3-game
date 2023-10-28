using System;
using UnityEngine;

namespace  Match3
{
    [CreateAssetMenu(fileName = "HealthSkillData", menuName = "Skills/Health Skill")]
    class HealthSkillData : ScriptableObject, IBaseSkillData
    {
        #region VARIABLES
        public int InitialHp;
        public string Name = "HealthSkill";

        #endregion

        public string GetName()
        {
            return Name;
        }
    }
}