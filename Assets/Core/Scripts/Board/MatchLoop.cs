using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
   public class MatchLoop : SingletonGameSystem<MatchLoop>
   {
      #region PROPERTIES
      public bool IsMatchActive { get; private set; }
      #endregion

      public override void Initialize()
      {
         IsMatchActive = true;
         base.Initialize();
      }

      public void Stop()
      {
         IsMatchActive = false;
      }

      public void Start()
      {
         IsMatchActive = true;
      }

      public void OnTouchRelease(Cell touchedCell)
      {
         if (!IsMatchActive) return;
         
         Item selectedItem = touchedCell.Item;
         if (selectedItem == null) return;

         bool didSpendMove = false;

         if (selectedItem.FlowTarget != null) return;
         
         if (selectedItem.TryGetSkill(out LinkSkill linkSkill))
         {
            if (linkSkill.LinkGroup.Count < 2) return;
            
            foreach (var cell in linkSkill.LinkGroup)
            {
               cell.Item.Despawn();
            }

            didSpendMove = true;
         }
         
         
         // if (selectedItem.TryGetSkill(out HealthSkill healthSkill))
         // {
         //    healthSkill.ReduceHealth();
         // }

         if (didSpendMove)
         {
            MatchManager.Instance.SpendMove();
         }
         
      }
   }
}
