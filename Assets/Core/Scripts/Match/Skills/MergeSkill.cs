using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class MergeSkill : ISkill
    {
        #region VARIABLES
        private Item item;
        public Sprite MergeSprite { get; private set; }
        public int MergeCount { get; private set; }
        public string ItemToCreate { get; private set; }
        private SpriteRenderer spriteRenderer;
        private Sprite initialSprite;
        #endregion
        
        public void Initialize(IBaseSkillData baseSkillData)
        {
            MergeSprite = ((MergeSkillData)baseSkillData).MergeSprite;
            ItemToCreate = ((MergeSkillData)baseSkillData).ItemToCreate;
            MergeCount = ((MergeSkillData)baseSkillData).MergeCount;
        }

        public void SetItem(Item item)
        {
            this.item = item;
            spriteRenderer = item.GameObject.GetComponentInChildren<SpriteRenderer>();
            initialSprite = spriteRenderer.sprite;
        }

        public void UpdateMergeSprite(bool merges)
        {
            if (merges)
            {
                spriteRenderer.sprite = MergeSprite;
            }
            else
            {
                spriteRenderer.sprite = initialSprite;
            }
        }
    }
}