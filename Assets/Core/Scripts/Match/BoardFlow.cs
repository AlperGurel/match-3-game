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

        [SerializeField] private float MaxVelocity = 1;
        [SerializeField] private float MaxAcceleration = 0.3f;
        [SerializeField] private float AccelerationIncrease = 0.2f;
        
        private List<Item> fallingItems;
        private bool IsAnyItemFalling;
        private ItemFactory randomItemFactory;
        #endregion

        private void Awake()
        {
            fallingItems = new List<Item>();
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
                
                MarkCellsForFlow();

                GenerateNewItems();
                
                
                BoardLink.Instance.UpdateLinks();
                MergeLink.Instance.UpdateMergeLinkSprites();
            }
        }
        

        private void GenerateNewItems()
        {
            return;
            bool generated = false;
            int yIndex = MatchManager.Instance.Board.YBoundaries.y;
            for (int x = MatchManager.Instance.Board.XBoundaries.x; x <= MatchManager.Instance.Board.XBoundaries.y; x++)
            {
                if (MatchManager.Instance.Board.TryGetCell(new Vector2Int(x, yIndex), out Cell cell))
                {
                    if (cell.Item == null && !cell.FlowBlocked)
                    {
                        
                        
                        
                        Cell lastTarget = null;
                        for (int y = MatchManager.Instance.Board.YBoundaries.x; y < yIndex; y++)
                        {
                            if (MatchManager.Instance.Board.TryGetCell(new Vector2Int(x, y), out Cell pt))
                            {
                                if (pt.Item == null && pt.IncomingItem == null)
                                {
                                    
                                    lastTarget = pt;
                                }
                                //cell should be empty
                                //cell should not have incoming object
                            }
                        }

                        if (lastTarget != null)
                        {
                            var item = randomItemFactory.CreateItem();
                            item.SetSortingOrder(lastTarget.Index.y);
                            item.Transform.SetParent(lastTarget.Transform);
                            item.SetCell(lastTarget);
                            item.Transform.position =   new Vector3(0, 1.2f, 0);
                            item.SetFlowTarget(lastTarget);
                            generated = true;
                        }
                        
                
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
            
            var shouldStartFlow = MatchManager.Instance.Board.Where(x => x.Item != null && x.Item.FlowTarget != null && !x.Item.IsFalling).ToList();
            // if (IsAnyItemFalling == false && shouldFlow.Count > 0)
            // {
            //     IsAnyItemFalling = true;
            //     Debug.Log("[BoardFlow] Fall Start");
            //     BoardLink.Instance.UpdateLinks();
            //     MergeLink.Instance.UpdateMergeLinkSprites();
            // } else if (IsAnyItemFalling && shouldFlow.Count == 0)
            // {
            //     IsAnyItemFalling = false;
            //     
            //     Debug.Log("[BoardFlow] Fall End");
            //     BoardLink.Instance.UpdateLinks();
            //     MergeLink.Instance.UpdateMergeLinkSprites();
            // }
            

            foreach (var cell in shouldStartFlow)
            {
                cell.Item.IsFalling = true;
                cell.Item.SetSortingOrder(cell.Item.FlowTarget.Index.y);
                if (!fallingItems.Contains(cell.Item))
                {
                    fallingItems.Add(cell.Item);
                }

                var item = cell.Item;
                cell.Item.LeaveCell();
                item.SetCell(item.FlowTarget);
                item.GameObject.transform.SetParent(item.FlowTarget.Transform);
            }

            for (int index = fallingItems.Count - 1; index >= 0; index--)
            {
                Item item = fallingItems[index];
                if (item == null)
                {
                    fallingItems.Remove(item);
                    continue;
                }

                item.Acceleration += Time.deltaTime * AccelerationIncrease;
                item.Acceleration = Mathf.Min(item.Acceleration, MaxAcceleration);
                item.Velocity += Time.deltaTime * item.Acceleration;
                item.Velocity = Mathf.Min(item.Velocity, MaxVelocity);
                
                //Find the velocity of below item
                Vector2Int belowIndex = item.Cell.Index + Vector2Int.down;
                if (MatchManager.Instance.Board.TryGetCell(belowIndex, out Cell belowCell))
                {
                    if (belowCell.Item != null && belowCell.Item.IsFalling)
                    {
                        item.Velocity = Mathf.Max(item.Velocity, belowCell.Item.Velocity);
                    }
                }
                
                Vector3 step = new Vector3(0, item.Velocity, 0);
                item.Transform.localPosition -= step;

                if (item.Transform.localPosition.y < 0)
                {
                    item.Velocity = 0;
                    item.Acceleration = 0;
                    item.Transform.localPosition = Vector3.zero;
    
                    item.IsFalling = false;
                    item.SetFlowTarget(null);
                    fallingItems.Remove(item);
                    item.Cell.IncomingItem = null;
                }
                
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

                    // if (!cell.Item.IsFalling)
                    {
                        Cell target = null;
                        for (int i = cell.Index.y; i > blockedIndex.y; i--)
                        {
                            if (MatchManager.Instance.Board.TryGetCell(new Vector2Int(columnIndex, i),
                                    out Cell potentialTarget))
                            {
                                if (potentialTarget.Item != null && !potentialTarget.Item.CanFall)
                                {
                                    blockedIndex = new Vector2Int(columnIndex, i);
                                }else if (potentialTarget.FlowBlocked)
                                {
                                    blockedIndex = new Vector2Int(columnIndex, i);
                                }
                                else if (potentialTarget.Item == null)
                                {
                                    target = potentialTarget;
                                }
                            }
                        }

                        if (target != null)
                        {
                            blockedIndex = target.Index;
                            cell.Item.SetFlowTarget(target);
                            cell.Item.UpdatedFlowTarget = target;
                        }
                    }

                }
            }
        }
    }
}
