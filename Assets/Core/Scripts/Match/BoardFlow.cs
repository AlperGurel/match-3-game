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

        private List<Item> fallingItems;
        private List<Item> newGeneratedItems;
        private ItemFactory randomItemFactory;
        private bool isFallBegin;
        #endregion

        private void Awake()
        {
            fallingItems = new List<Item>();
            newGeneratedItems = new List<Item>();
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
          
            }
        }
        

        private void GenerateNewItems()
        {
            int yIndex = MatchManager.Instance.Board.YBoundaries.y;
            for (int x = MatchManager.Instance.Board.XBoundaries.x; x <= MatchManager.Instance.Board.XBoundaries.y; x++)
            {
                if (MatchManager.Instance.Board.TryGetCell(new Vector2Int(x, yIndex), out Cell cell))
                {
                    if (cell.Item == null && !cell.FlowBlocked && cell.IncomingItem == null)
                    {
                        var item = randomItemFactory.CreateItem();
                        item.SetFlowTarget(cell);
                        item.Transform.position = cell.Transform.position + new Vector3(0, 1.2f, 0);
                        newGeneratedItems.Add(item);
                    }
          
                }
            }


            
            // bool generated = false;
            // int yIndex = MatchManager.Instance.Board.YBoundaries.y;
            // for (int x = MatchManager.Instance.Board.XBoundaries.x; x <= MatchManager.Instance.Board.XBoundaries.y; x++)
            // {
            //     if (MatchManager.Instance.Board.TryGetCell(new Vector2Int(x, yIndex), out Cell cell))
            //     {
            //         if (cell.Item == null && !cell.FlowBlocked)
            //         {
            //             
            //             
            //             
            //             Cell lastTarget = null;
            //             for (int y = MatchManager.Instance.Board.YBoundaries.x; y < yIndex; y++)
            //             {
            //                 if (MatchManager.Instance.Board.TryGetCell(new Vector2Int(x, y), out Cell pt))
            //                 {
            //                     if (pt.Item == null && pt.IncomingItem == null)
            //                     {
            //                         
            //                         lastTarget = pt;
            //                     }
            //                     //cell should be empty
            //                     //cell should not have incoming object
            //                 }
            //             }
            //
            //             if (lastTarget != null)
            //             {
            //                 var item = randomItemFactory.CreateItem();
            //                 item.SetSortingOrder(lastTarget.Index.y);
            //                 item.Transform.SetParent(lastTarget.Transform);
            //                 item.SetCell(lastTarget);
            //                 item.Transform.position =   new Vector3(0, 1.2f, 0);
            //                 item.SetFlowTarget(lastTarget);
            //                 generated = true;
            //             }
            //             
            //     
            //         }
            //     }
            // }
            //
            // if (generated)
            // {
            //     BoardLink.Instance.UpdateLinks();
            //     MergeLink.Instance.UpdateMergeLinkSprites();
            // }
        }

        public void Update()
        {
            if (MatchLoop.Instance.IsMatchActive)
            {
                UpdateFallingItems();
           

                if (!isFallBegin)
                {
                    if (fallingItems.Count > 0)
                    {
                        OnFallBegin();
                    }
                }
                else
                {
                    if (fallingItems.Count == 0)
                    {
                        OnFallEnd();
                    }
                }
            }
        }

        private void OnFallBegin()
        {
            Debug.Log("On Fall Begin");
            isFallBegin = true;
            
            
            
            BoardLink.Instance.UpdateLinks();

            var falllingColumns = fallingItems.Select(item => item.FlowTarget.Index.x).Distinct().ToList();
            MergeLink.Instance.ResetMergeLinkSprites(falllingColumns);
        }

        private void OnFallEnd()
        {
            Debug.Log("On Fall End");
            isFallBegin = false;
                  
                
            BoardLink.Instance.UpdateLinks();
            MergeLink.Instance.UpdateMergeLinkSprites();
        }

        private void UpdateFallingItems()
        {
            var shouldStartFlow = MatchManager.Instance.Board.Where(x => x.Item != null && x.Item.FlowTarget != null && !x.Item.IsFalling).ToList();

            foreach (var item in newGeneratedItems)
            {
                item.IsFalling = true;
                item.SetSortingOrder(item.FlowTarget.Index.y);
                if (!fallingItems.Contains(item))
                {
                    fallingItems.Add(item);
                }
            }
            newGeneratedItems.Clear();
            
            foreach (var cell in shouldStartFlow)
            {
                cell.Item.IsFalling = true;
                cell.Item.SetSortingOrder(cell.Item.FlowTarget.Index.y);
                if (!fallingItems.Contains(cell.Item))
                {
                    fallingItems.Add(cell.Item);
                }
                
                cell.Item.LeaveCell();
            }

            for (int index = fallingItems.Count - 1; index >= 0; index--)
            {
                Item item = fallingItems[index];
                if (item == null)
                {
                    fallingItems.Remove(item);
                    continue;
                }
                

                item.Velocity = Time.deltaTime * 13;
                
                Vector3 step = new Vector3(0, item.Velocity, 0);
                item.Transform.position -= step;

                if (item.Transform.position.y < item.FlowTarget.Transform.position.y)
                {
                    item.Velocity = 0;
                    item.SetCell(item.FlowTarget);
                    item.GameObject.transform.SetParent(item.FlowTarget.Transform);
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

                    Cell target = null;
                    for (int i = cell.Index.y; i > blockedIndex.y; i--)
                    {
                        if (MatchManager.Instance.Board.TryGetCell(new Vector2Int(columnIndex, i),
                                out Cell potentialTarget))
                        {
                            if (potentialTarget.Item != null && !potentialTarget.Item.CanFall)
                            {
                                blockedIndex = new Vector2Int(columnIndex, i);
                            }else if (potentialTarget.FlowBlocked || potentialTarget.IncomingItem != null)
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
                    }
                        
                }
            }
        }
    }
}
