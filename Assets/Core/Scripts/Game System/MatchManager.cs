using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace Match3
{
    public class MatchManager : SingletonGameSystem<MatchManager>
    {
        #region VARIABLES

        private Board board;
        private LevelJsonData levelJsonData;
        private Dictionary<Vector2Int, string> gridItemIds;
        private int moveCount;
        private Vector3 initialBackgroundPosition;
        private Vector3 initialBoardPosition;
        private bool isMatchEnd;
        #endregion

        #region PROPERTIES

        public Board Board => board;
        public Vector2Int Size { get; private set; }
        
        public Dictionary<string, int> LevelObjectives { get; private set; }

        #endregion

        public override void Initialize()
        {
            LevelObjectives = new Dictionary<string, int>();

            initialBackgroundPosition = Main.Instance.BoardBackground.position;
            initialBoardPosition = Main.Instance.BoardTransform.position;
            
            base.Initialize();
        }

        public void InitializeBoard()
        {
            isMatchEnd = false;
            
            MatchLoop.CreateInstance();
            
            LoadLevelData();

            UpdateBackground();

            PopulateBoard();

            UpdateLevelObjectives();

            MatchUI.Instance.CreateObjectivesUI();

            MatchUI.Instance.UpdateMoveCount(levelJsonData.move_count);
            
            PlayLevelStartAnimation();
            
            MatchLoop.InitializeInstance();

            BoardLink.CreateInstance();
            BoardLink.InitializeInstance();
            BoardLink.Instance.UpdateLinks();

            MergeLink.CreateInstance();
            MergeLink.InitializeInstance();
            MergeLink.Instance.UpdateMergeLinkSprites();
        }

        public void RestartMatch()
        {

            isMatchEnd = false;
            
            RefreshBoard();
            
            UpdateLevelObjectives();
            
            MatchUI.Instance.CreateObjectivesUI();
            
            MatchUI.Instance.UpdateMoveCount(levelJsonData.move_count);
            
            PlayLevelStartAnimation();

            MatchLoop.Instance.Start();
                
            BoardLink.Instance.UpdateLinks();
        }

        private void PlayLevelStartAnimation()
        {
            Main.Instance.BoardTransform.DOMove(initialBoardPosition, 0f);
            Main.Instance.BoardBackground.DOMove(initialBackgroundPosition, 0f);
            
            float targetX = 0;
            if (Size.x % 2 != 0)
            {
                targetX = -0.5f;
            }

            float targetY = -3;
            if (Size.y % 2 == 0)
            {
                targetY = -2.5f;
            }

            Main.Instance.BoardTransform.transform.DOMoveY(targetY, 0);
            Main.Instance.BoardTransform.DOMoveX(targetX, 0.6f).SetEase(Ease.OutQuart);
            Main.Instance.BoardBackground.DOMoveX(0, 0.6f).SetEase(Ease.OutQuart);
        }

        private void PopulateBoard()
        {
            board = new Board(Size, Main.Instance.BoardTransform);

            foreach (var cell in board)
            {
                if (cell.Index.y == board.YBoundaries.y)
                {
                    var mask = GameObject.Instantiate(Main.Instance.GeneratorMaskPrefab, cell.Transform);
                    mask.transform.localPosition = new Vector3(0, 1.34f, 0);
                }
                
                var itemFactory = ItemFactoryManager.Instance.GetItemFactory(gridItemIds[cell.Index]);
                if (itemFactory == null)
                {
                    continue;
                }
                
                Item createdItem = itemFactory.CreateItem();
                createdItem.SetSortingOrder(cell.Index.y);
                createdItem.Transform.SetParent(cell.Transform);
                createdItem.Transform.localPosition = Vector3.zero;
                createdItem.SetCell(cell);
            }
        }

        public void GenerateItemAt(Cell cell, string itemId, bool shouldPlaySpawnAnimation)
        {
            var itemFactory = ItemFactoryManager.Instance.GetItemFactory(itemId);
            if (itemFactory == null) return;
            Item generatedItem = itemFactory.CreateItem();
            generatedItem.SetSortingOrder(cell.Index.y);
            generatedItem.Transform.SetParent(cell.Transform);
            generatedItem.Transform.localPosition = Vector3.zero;
            generatedItem.SetCell(cell);
        }

        private void RefreshBoard()
        {
            foreach (var cell in board)
            {
                if (cell.Item != null)
                {
                    cell.Item.Despawn();
                }
                var itemFactory = ItemFactoryManager.Instance.GetItemFactory(gridItemIds[cell.Index]);
                if (itemFactory == null)
                {
                    continue;
                }
                
                Item createdItem = itemFactory.CreateItem();
                createdItem.SetSortingOrder(cell.Index.y);
                createdItem.Transform.SetParent(cell.Transform);
                createdItem.Transform.localPosition = Vector3.zero;
                createdItem.SetCell(cell);
            }
        }

        private void UpdateLevelObjectives()
        {
            LevelObjectives.Clear();
            
            foreach (string id in levelJsonData.grid)
            {
                var factory = ItemFactoryManager.Instance.GetItemFactory(id);
                if (factory.IsObjective)
                {
                    if (!LevelObjectives.ContainsKey(id))
                    {
                        LevelObjectives[id] = 1;
                    }
                    else
                    {
                        LevelObjectives[id]++;
                    }
                }
            }

            moveCount = levelJsonData.move_count;
        }


        private void LoadLevelData()
        {
            string fileName = "level_" + Util.PadNumber(Player.Instance.PlayerData.CurrentLevel);
            TextAsset jsonTextAsset = Resources.Load<TextAsset>(fileName);
            if (jsonTextAsset != null)
            {
                string json = jsonTextAsset.text;
                levelJsonData = JsonUtility.FromJson<LevelJsonData>(json);

                Size = new Vector2Int(levelJsonData.grid_width, levelJsonData.grid_height);
            }

            int xHalf = Size.x / 2;
            int yHalf = Size.y / 2;

            Vector2Int xBoundaries = new Vector2Int(-xHalf, xHalf - (Size.x + 1) % 2);
            Vector2Int yBoundaries = new Vector2Int(-yHalf, yHalf - (Size.y + 1) % 2);

            gridItemIds = new Dictionary<Vector2Int, string>();

            int i = 0;
            for (int y = yBoundaries.x; y <= yBoundaries.y; y++)
            {
                for (int x = xBoundaries.x; x <= xBoundaries.y; x++)
                {
                    Vector2Int index = new Vector2Int(x, y);
                    gridItemIds[index] = levelJsonData.grid[i];
                    i++;
                }
            }
        }

        private void UpdateBackground()
        {
            var bgTransform = Main.Instance.BoardBackground;
            var spriteRenderer = bgTransform.gameObject.GetComponent<SpriteRenderer>();
            float xSize = Size.x + 0.35f;
            float ySize = Size.y + 0.4f;
            spriteRenderer.size = new Vector2(xSize, ySize);
        }

        public void OnItemDespawn(Item item)
        {
            if (LevelObjectives.TryGetValue(item.Id, out int remainingObjective))
            {
                LevelObjectives[item.Id] = Mathf.Max(remainingObjective-1, 0);
                MatchUI.Instance.UpdateObjectivesUI();
            }

            bool allObjectivesCompleted = true;
            foreach (var kv in LevelObjectives)
            {
                if (kv.Value > 0)
                {
                    allObjectivesCompleted = false;
                }
            }

            if (allObjectivesCompleted && !isMatchEnd)
            {
                isMatchEnd = true;
                Player.Instance.PlayerData.CurrentLevel++;
                PopupManager.Instance.ShowPopup<WinScreenPopup>();
                MetaUI.Instance.UpdateLevelText();
            }
        }

        public void SpendMove()
        {
            moveCount = Mathf.Max(moveCount - 1, 0);
            MatchUI.Instance.UpdateMoveCount(moveCount);

            if (moveCount == 0)
            {
                MatchLoop.Instance.Stop();
                PopupManager.Instance.ShowPopup<LevelFailedPopup>();
            }
            
        }

        public void UnloadMatch()
        {
            board.DestroyBoard();
        }
    }
}
