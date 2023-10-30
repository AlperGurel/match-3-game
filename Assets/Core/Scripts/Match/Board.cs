using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match3
{
    public class Cell
    {
        #region VARIABLES

        private Item item;
        private Board board;
        #endregion

        #region PROPERTIES

        public Vector2Int Index { get; }
        public Item Item => item;
        public Vector2 Position { get; private set; }
        public Transform Transform { get; private set; }
        public GameObject GameObject { get; private set; }
        public bool FlowBlocked { get; private set; }
        public Item IncomingItem { get; set; }

        #endregion

        public Cell(Vector2Int index, Board board)
        {
            this.board = board;
            Index = index;
            Position = new Vector2(index.x + 0.5f, index.y);

            GameObject = new GameObject(index.ToString());
            GameObject.transform.parent = board.Transform;
            GameObject.transform.localPosition = Position;
            Transform = GameObject.transform;

        }

        public void SetItem(Item item)
        {
            this.item = item;
        }

        public void ToggleFlow(bool toggle)
        {
            FlowBlocked = !toggle;
        }
    }


    public class Board : IEnumerable<Cell>
    {
        #region COMPONENTS

        private Grid grid;
        #endregion
        
        #region VARIABLES

        private Dictionary<Vector2Int, Cell> boardGrid;
        private GameObject gameObject;
        private Transform transform;
        private Vector2Int size;
 
        #endregion

        #region PROPERTIES

        public Transform Transform => transform;
        public Vector2Int XBoundaries { get; private set; }
        public Vector2Int YBoundaries { get; private set; }
        public List<List<Cell>> Columns { get; private set; }
        #endregion

        public Board(Vector2Int size, Transform boardTransform)
        {
            gameObject = boardTransform.gameObject;
            transform = boardTransform;
            grid = transform.GetComponent<Grid>();
            boardGrid = new Dictionary<Vector2Int, Cell>();
            this.size = size;
            int xHalf = size.x / 2;
            int yHalf = size.y / 2;

            XBoundaries = new Vector2Int(-xHalf, xHalf - (size.x + 1) % 2);
            YBoundaries = new Vector2Int(-yHalf, yHalf - (size.y + 1) % 2);

            for (int x = XBoundaries.x; x <= XBoundaries.y; x++)
            {
                for (int y = YBoundaries.x; y <= YBoundaries.y; y++)
                {
                    Vector2Int index = new Vector2Int(x, y);
                    Cell cell = new Cell(index, this);
                    boardGrid[index] = cell;
                }
            }

            Columns = new List<List<Cell>>();
            
            for (int x = XBoundaries.x; x <= XBoundaries.y; x++)
            {
                var list = new List<Cell>();
                for (int y = YBoundaries.x; y <= YBoundaries.y; y++)
                {
                    list.Add(boardGrid[new Vector2Int(x, y)]);
                } 
                Columns.Add(list);
            }
        }

        public void DestroyBoard()
        {
            foreach (Cell cell in this)
            {
                if (cell.Item != null)
                {
                    cell.Item.Despawn();
                }
                GameObject.Destroy(cell.GameObject);
                
            }
            boardGrid.Clear();
        }
        
        public bool TryGetCell(Vector3 touchPosition, out Cell cell)
        {
            float xOffset = size.x % 2 == 0 ? 0 : 0.5f;
            float yOffset = size.y % 2 == 0 ? 3 : 3.5f;
            var index = (Vector2Int)grid.LocalToCell(new Vector3(touchPosition.x + xOffset, touchPosition.y + yOffset, 0));
            if (boardGrid.TryGetValue(index, out var value))
            {
                cell = value;
                return true;
            }
            cell = null;
            return false;
        }

        public bool TryGetCell(Vector2Int index, out Cell cell)
        {
            if (boardGrid.TryGetValue(index, out var value))
            {
                cell = value;
                return true;
            }
            cell = null;
            return false;
        }
        

        public int GetColumnIndex(int index)
        {
            return index + XBoundaries.x;
        }
        
        public IEnumerator<Cell> GetEnumerator()
        {
            return boardGrid.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<Cell> GetSurroundingCells(List<Cell> linkedCells)
        {
            Vector2Int[] offsets = new[]
            {
                new Vector2Int(1, 0), 
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),  
                new Vector2Int(0, -1)  
            };
            
            List<Cell> surroundingCells = new List<Cell>();
            foreach (var cell in linkedCells)
            {
              
                foreach (Vector2Int offset in offsets)
                {
                    Vector2Int neighborIndex = cell.Index + offset;

                    if (TryGetCell(neighborIndex, out Cell neighborCell))
                    {
                        if (!surroundingCells.Contains(neighborCell) && !linkedCells.Contains(neighborCell))
                        {
                            surroundingCells.Add(neighborCell);
                        }
                    }
                }  
            }

            return surroundingCells;
        }

        public List<Cell> GetCellsAtRadius(Vector2Int centerIndex, int radius)
        {
            //https://www.geeksforgeeks.org/bresenhams-circle-drawing-algorithm/
            
            List<Vector2Int> indexesOnPerimeter = new List<Vector2Int>();

            int x = radius;
            int y = 0;
            int radiusError = 1 - x;

            while (x >= y)
            {
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x + x, centerIndex.y + y));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x - x, centerIndex.y + y));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x - x, centerIndex.y - y));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x + x, centerIndex.y - y));

                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x + y, centerIndex.y + x));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x - y, centerIndex.y + x));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x - y, centerIndex.y - x));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x + y, centerIndex.y - x));

                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x + x, centerIndex.y + y + 1));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x - x, centerIndex.y + y + 1));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x - x, centerIndex.y - y - 1));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x + x, centerIndex.y - y - 1));

                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x + y + 1, centerIndex.y + x));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x - y - 1, centerIndex.y + x));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x - y - 1, centerIndex.y - x));
                indexesOnPerimeter.Add(new Vector2Int(centerIndex.x + y + 1, centerIndex.y - x));

                
                y++;

                if (radiusError < 0)
                {
                    radiusError += 2 * y + 1;
                }
                else
                {
                    x--;
                    radiusError += 2 * (y - x) + 1;
                }
            }
            

            return indexesOnPerimeter.Select(index =>
            {
                if (TryGetCell(index, out Cell cell))
                {
                    return cell;
                }
                else
                {
                    return null;
                }
            }).Where(cell => cell != null).Distinct().ToList();
        }
        
    }
}