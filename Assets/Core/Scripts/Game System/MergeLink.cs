using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Match3
{
    public class MergeLink : SingletonGameSystem<MergeLink>
    {
        public void UpdateMergeLinkSprites()
        {
            var board = MatchManager.Instance.Board;
            foreach (var cell in board)
            {
                if(cell.Item == null) continue;
                if(!cell.Item.TryGetSkill(out MergeSkill mergeSkill)) continue;

                if (!cell.Item.TryGetSkill(out LinkSkill linkSkill))
                {
                    mergeSkill.UpdateMergeSprite(false);
                    continue;
                }

                if (linkSkill.LinkGroup.Count < mergeSkill.MergeCount)
                {
                    mergeSkill.UpdateMergeSprite(false);
                    continue;
                }
                
                mergeSkill.UpdateMergeSprite(true);
            }
        }

        public void ResetMergeLinkSprites(List<int> columns)
        {
            var board = MatchManager.Instance.Board;

            foreach (var index in columns)
            {
                var cells = board.GetColumnCells(index);
            
                foreach (var cell in cells)
                {
                    if(cell.Item == null) continue;
                    if(!cell.Item.TryGetSkill(out MergeSkill mergeSkill)) continue;
                    mergeSkill.UpdateMergeSprite(false);
                }
            }
        
        }
    }
}
