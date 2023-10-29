using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace  Match3
{
    public class BoardFlow : MonoBehaviour
    {
        #region VARIABLES

        private bool IsAnyItemFalling;
        private ItemFactory randomItemFactory;
        #endregion

        private void Awake()
        {
            
        }

        private void Start()
        {
            if (ItemFactoryManager.Instance != null)
            {
                randomItemFactory = ItemFactoryManager.Instance.GetItemFactory("rand");
            }
        }


        public void LateUpdate()
        {
            if (MatchLoop.Instance.IsMatchActive)
            {
                GenerateNewItems();
                
                MarkCellsForFlow();
            }
        }
        

        private void GenerateNewItems()
        {
            bool generated = false;
            int yIndex = MatchManager.Instance.Board.YBoundaries.y;
            for (int x = MatchManager.Instance.Board.XBoundaries.x; x <= MatchManager.Instance.Board.XBoundaries.y; x++)
            {
                if (MatchManager.Instance.Board.TryGetCell(new Vector2Int(x, yIndex), out Cell cell))
                {
                    if (cell.Item == null && !cell.FlowBlocked)
                    {
                        var item = randomItemFactory.CreateItem();
                        item.SetSortingOrder(cell.Index.y);
                        item.Transform.SetParent(cell.Transform);
                        item.SetCell(cell);
                        item.Transform.localPosition = new Vector3(0, 1.2f, 0);
                        item.Transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.InOutQuad);
                        generated = true;
                    }
                }
            }

            if (generated)
            {
                BoardLink.Instance.UpdateLinks();
                MergeLink.Instance.UpdateMergeLinkSprites();
            }
        }

        public void Update()
        {
            var shouldFlow = MatchManager.Instance.Board.Where(x => x.Item != null && x.Item.FlowTarget != null).ToList();
            if (IsAnyItemFalling == false && shouldFlow.Count > 0)
            {
                IsAnyItemFalling = true;
                Debug.Log("[BoardFlow] Fall Start");
                BoardLink.Instance.UpdateLinks();
                MergeLink.Instance.UpdateMergeLinkSprites();
            } else if (IsAnyItemFalling && shouldFlow.Count == 0)
            {
                IsAnyItemFalling = false;
                
                Debug.Log("[BoardFlow] Fall End");
                BoardLink.Instance.UpdateLinks();
                MergeLink.Instance.UpdateMergeLinkSprites();
            }
            
            foreach (var cell in shouldFlow)
            {
                var item = cell.Item;
                item.LeaveCell();
                item.Transform.parent = item.FlowTarget.Transform;
                item.SetCell(item.FlowTarget);
                item.GameObject.transform.DOLocalMoveY(0, 0.25f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        item.SetSortingOrder(item.Cell.Index.y);
                        item.SetFlowTarget(null);
                    });
            }
        }

        private void MarkCellsForFlow()
        {
            for (var index = 0; index < MatchManager.Instance.Board.Columns.Count; index++)
            {
                var column = MatchManager.Instance.Board.Columns[index];
                int columnIndex = MatchManager.Instance.Board.GetColumnIndex(index);
                Vector2Int blockedIndex = new Vector2Int(columnIndex, column[0].Index.y - 1);
                foreach (Cell cell in column)
                {
                    if(cell.Item == null) continue;
                    Cell target = null;
                    for (int i = cell.Index.y; i > blockedIndex.y; i--)
                    {
                        if (MatchManager.Instance.Board.TryGetCell(new Vector2Int(columnIndex, i),
                                out Cell potentialTarget))
                        {
                            if (potentialTarget.Item != null && !potentialTarget.Item.CanFall && !potentialTarget.FlowBlocked)
                            {
                                blockedIndex = new Vector2Int(columnIndex, i);
                            } else if (potentialTarget.Item == null)
                            {
                                target = potentialTarget;
                            }
                        }
                    }

                    if (target != null)
                    {
                        blockedIndex = target.Index;
                        cell.Item.SetFlowTarget(target);
                    }
                }
            }
        }
    }
}
