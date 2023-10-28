using UnityEngine;

namespace  Match3
{
    class HealthSkill : ISkill
    {
        #region VARIABLES
        
        private Item item;
        #endregion
      
        #region PROPERTIES
        
        public int RemainingHp { get; private set; }
        #endregion
      
        public void Initialize(IBaseSkillData baseSkillData)
        {
            RemainingHp = ((HealthSkillData)baseSkillData).InitialHp;
        }

        public void SetItem(Item item)
        {
            this.item = item;
        }

        public void ReduceHealth()
        {
            RemainingHp--;

            if (item.GameObject.TryGetComponent(out HealthSprites healthSprites))
            {
                healthSprites.UpdateHealthSprite(RemainingHp);
            }
            
            if (RemainingHp == 0)
            {
                item.Despawn();
                MatchManager.Instance.OnItemDespawn(item);
            }
        }
      
    }
}