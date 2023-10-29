using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Match3
{
    public class LinkSkill : ISkill
    {
        #region  VARIABLES
        private Item item;
        #endregion
        
        #region PROPERTIES

        public List<Cell> LinkGroup;
        #endregion
        
        public void Initialize(IBaseSkillData baseSkillData)
        {
            LinkGroup = new List<Cell>();
        }

        public void SetItem(Item item)
        {
            this.item = item;
        }

        public void SetLinkGroup(List<Cell> cells)
        {
            LinkGroup = cells;
        }
    }
}

