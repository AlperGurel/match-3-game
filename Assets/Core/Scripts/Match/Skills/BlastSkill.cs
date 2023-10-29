using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public enum BlastType{
        WEAK,
        STRONG
    }
    
    public class BlastSkill : ISkill
    {
        #region VARIABLES

        private Item item;

        #endregion
        
        #region PROPERTIES
        public BlastType BlastType { get; private set; }
        #endregion

        public void Initialize(IBaseSkillData baseSkillData)
        {
            BlastType = ((BlastSkillData)baseSkillData).BlastType;
        }

        public void SetItem(Item item)
        {
            this.item = item;
        }

        public void Blast(BlastType hiterBlastType)
        {
            if (hiterBlastType < BlastType)
            {
                return;
            }

            if (item.TryGetSkill(out HealthSkill healthSkill))
            {
                healthSkill.ReduceHealth();
            }
        }
    }
}