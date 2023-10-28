using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class LinkedGroup
    {
        public List<Cell> Cells { get; } = new List<Cell>();
    }
    
    public class BoardLink : SingletonGameSystem<BoardLink>
    {
        public override void Initialize()
        {
            
        }

        public void UpdateLinks()
        {
            var board = MatchManager.Instance.Board;

            List<LinkedGroup> groups = new List<LinkedGroup>();
            HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int>();

            foreach (var cell in board)
            {
                if(cell.Item == null) continue;
                if (!cell.Item.TryGetSkill(out LinkSkill linkSkill)) continue;
                Vector2Int index = cell.Index;
                
                if (!visitedCells.Contains(index))
                {
                    string id = cell.Item.Id;
                    LinkedGroup group = new LinkedGroup();
                    FindGroup(index, id, group, visitedCells);
                    groups.Add(group);
                }
            }

            foreach (var group in groups)
            {
                foreach (var cell in group.Cells)
                {
                    if (cell.Item.TryGetSkill(out LinkSkill linkSkill))
                    {
                        linkSkill.SetLinkGroup(new List<Cell>(group.Cells));
                    }
                }
            }
            
        }

        private void FindGroup(Vector2Int index, string id, LinkedGroup group, HashSet<Vector2Int> visitedCells)
        {
            if (MatchManager.Instance.Board.TryGetCell(index, out Cell cell))
            {
                if (cell.Item == null)
                    return;
                if (!cell.Item.TryGetSkill(out LinkSkill linkSkill)) return;
                if (!visitedCells.Contains(index) && cell.Item.Id == id)
                {
                    visitedCells.Add(index);
                    group.Cells.Add(cell);
                    
                    FindGroup(new Vector2Int(index.x + 1, index.y), id, group, visitedCells);
                    FindGroup(new Vector2Int(index.x - 1, index.y), id, group, visitedCells);
                    FindGroup(new Vector2Int(index.x, index.y + 1), id, group, visitedCells);
                    FindGroup(new Vector2Int(index.x, index.y - 1), id, group, visitedCells);
                }
            }
        }
    }
}