using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
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

      public async void OnTouchRelease(Cell touchedCell)
      {
         if (!IsMatchActive) return;
         
         Item selectedItem = touchedCell.Item;
         if (selectedItem == null) return;

         bool didSpendMove = false;

         if (selectedItem.FlowTarget != null) return;


         selectedItem.TryGetSkill(out LinkSkill linkSkill);
         selectedItem.TryGetSkill(out MergeSkill mergeSkill);


         if (selectedItem is TNTItem tntItem && linkSkill.LinkGroup.Count == 1)
         {
            tntItem.Explode(false);
            didSpendMove = true;
         }
         

         bool canMerge = false;
         
         
         if (mergeSkill != null && linkSkill != null)
         {
            if (linkSkill.LinkGroup.Count >= mergeSkill.MergeCount)
            {
               canMerge = true;

               bool tntMerge = selectedItem is TNTItem;

               if (!tntMerge)
               {
                  List<Cell> surroundingCells = MatchManager.Instance.Board.GetSurroundingCells(linkSkill.LinkGroup);
                  
                  foreach (var cell in surroundingCells)
                  {
                     if ( cell.Item != null && cell.Item.TryGetSkill(out BlastSkill blastSkill))
                     {
                        blastSkill.Blast(BlastType.WEAK);
                     }
                  }

                  List<Task> tweenTasks = new List<Task>();
                  foreach (var cell in linkSkill.LinkGroup)
                  {
                     cell.ToggleFlow(false);
                     var spriteItem = cell.Item.Transform.GetChild(0);
                     cell.Item.SetSortingOrder(10);
                     tweenTasks.Add(spriteItem.DOMove(selectedItem.Transform.position, 0.3f).AsyncWaitForCompletion());
                     // tweenTasks.Add(spriteItem.DOScale(new Vector3(0.2f, 0.2f, 0), 0.1f).SetEase(Ease.OutElastic).AsyncWaitForCompletion());
                  }
                  
                  await Task.WhenAll(tweenTasks);
                  
                  
                  foreach (var cell in linkSkill.LinkGroup)
                  {
                     cell.Item.DespawnSilent();
                     cell.ToggleFlow(true);
                  }
                  
               
                  var createdItemId = mergeSkill.ItemToCreate;
                  MatchManager.Instance.GenerateItemAt(selectedItem.Cell, createdItemId, true);
               }
               
               else
               {
                  foreach (var cell in linkSkill.LinkGroup)
                  {
                     if (cell.Item != selectedItem)
                     {
                        cell.Item.DespawnSilentAsync(0.4f);
                     }
                  }
                  
                  ((TNTItem)selectedItem).Explode(true);
               }

               didSpendMove = true;

            }
         }

         if (!canMerge && linkSkill != null)
         {
            if (linkSkill.LinkGroup.Count > 1)
            {
               List<Cell> surroundingCells = MatchManager.Instance.Board.GetSurroundingCells(linkSkill.LinkGroup);
               foreach (var cell in surroundingCells)
               {
                  if (cell.Item != null && cell.Item.TryGetSkill(out BlastSkill blastSkill))
                  {
                     blastSkill.Blast(BlastType.WEAK);
                  }
               }

               foreach (var cell in linkSkill.LinkGroup)
               {
                  cell.Item.Despawn();
               }

               didSpendMove = true;
            }
         }
         

         if (didSpendMove)
         {
            MatchManager.Instance.SpendMove();
         }
         else
         {
            selectedItem.PlayInteractAnimation();
         }
         
      }
   }
}
