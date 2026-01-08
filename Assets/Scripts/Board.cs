using System.Collections.Generic;
using UnityEngine;
// Manages the game board made up of cells (attach to Board GameObject)
public class Board : MonoBehaviour
{
    private const int SIZE = 8;
    public int Size
    {
        get => SIZE;
    }

    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;// Parent transform for cells
    private readonly Cell[,] cells = new Cell[SIZE, SIZE];// 2D array to hold cell references
    private readonly int[,] data = new int[SIZE, SIZE];// 0 : Empty, 1 : Hover, 2 : Normal

    #region Manage Hover Points
    private readonly List<Vector2Int> hoverPoints = new();
    private readonly List<int> fullLineColumns = new();
    private readonly List<int> fullLineRows = new();
    #endregion

    private void Start()
    {
        for (int row = 0; row < SIZE; row++)
        {
            for (int column = 0; column < SIZE; column++)
            {
                cells[row, column] = Instantiate(cellPrefab, cellsTransform);
                //cells[row, column].SetColor(Color.blue);
                cells[row, column].transform.position = new Vector3(column + 0.5f, row + 0.5f, 0);// Set cell position, pixels to units (1 unit = 1 cell, 1 unit = 100 pixel, 1 cell w = h = 100)
                cells[row, column].Hide();
            }
        }
    }

    private void GetHoverPoints(Vector2Int point, int polyominoRows, int polyominoColumns, int[,] polyomino)
    {
        for (int row = 0; row < polyominoRows; row++)
        {
            for (int column = 0; column < polyominoColumns; column++)
            {
                if (polyomino[row, column] > 0)
                {
                    var hoverPoint = point + new Vector2Int(column, row);
                    if (!IsValidHover(hoverPoint))
                    {
                        hoverPoints.Clear();
                        return;
                    }
                    hoverPoints.Add(hoverPoint);
                }
            }
        }
    }

    private bool IsValidHover(Vector2Int point)
    {
        if (point.x < 0
            || point.x >= SIZE
            || point.y < 0
            || point.y >= SIZE
            || data[point.y, point.x] > 0)
        {
            return false;
        }
        return true;
    }

    // Show hover effect for a polyomino at a specific board point
    public void Hover(Vector2Int point, int polyominoIndex)
    {
        var polyomino = Polyominoes.Get(polyominoIndex);
        int polyominoRows = polyomino.GetLength(0);
        int polyominoColumns = polyomino.GetLength(1);

        UnHover();
        GetHoverPoints(point, polyominoRows, polyominoColumns, polyomino);
        if (hoverPoints.Count > 0)
        {
            Hover();
        }
    }

    private void Hover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 1;// Mark as hover
            cells[hoverPoint.y, hoverPoint.x].Hover();
        }
    }

    private void UnHover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 0;// Mark as empty
            cells[hoverPoint.y, hoverPoint.x].Hide();
        }
        hoverPoints.Clear();
    }

    public bool Place(Vector2Int point, int polyominoIndex)
    {
        var polyomino = Polyominoes.Get(polyominoIndex);
        int polyominoRows = polyomino.GetLength(0);//dimensions of polyomino
        int polyominoColumns = polyomino.GetLength(1);//dimensions of polyomino

        UnHover();
        GetHoverPoints(point, polyominoRows, polyominoColumns, polyomino);
        if (hoverPoints.Count > 0)
        {
            Place(point, polyominoRows, polyominoColumns);
            return true;
        }
        return false;
    }

    public void Place(Vector2Int point, int polyominoRows, int polyominoColumn)
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 2;// Mark as normal
            cells[hoverPoint.y, hoverPoint.x].Normal();
        }
        hoverPoints.Clear();
        ClearFullLines(point, polyominoRows, polyominoColumn);
    }

    private void ClearFullLines(Vector2Int point, int polyominoRows, int polyominoColumn)
    {
        FullLinesColumn(point.x, point.x + polyominoColumn);
        FullLinesRow(point.y, point.y + polyominoRows);

        ClearFullLines();
    }

    private void FullLinesColumn(int fromColum, int toColumn)
    {
        fullLineColumns.Clear();
        for (int column = fromColum; column < toColumn; column++)
        {
            bool isFull = true;
            for (int row = 0; row < SIZE; row++)
            {
                if (data[row, column] != 2)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                fullLineColumns.Add(column);
            }
        }
    }

    private void FullLinesRow(int fromRow, int toRow)
    {
        fullLineRows.Clear();
        for (int row = fromRow; row < toRow; row++)
        {
            bool isFull = true;
            for (int column = 0; column < SIZE; column++)
            {
                if (data[row, column] != 2)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                fullLineRows.Add(row);
            }
        }
    }

    private void ClearFullLines()
    {
        //Clear full rows
        foreach (var row in fullLineRows)
        {
            for (int column = 0; column < SIZE; column++)
            {
                data[row, column] = 0;
                cells[row, column].Hide();
            }
        }
        // Clear full columns
        foreach (var column in fullLineColumns)
        {
            for (int row = 0; row < SIZE; row++)
            {
                data[row, column] = 0;
                cells[row, column].Hide();
            }
        }
    }

    //private void ClearFullLines()
    //{
    //    fullLineRows.Clear();
    //    fullLineColumns.Clear();
    //    // Check for full rows
    //    for (int row = 0; row < SIZE; row++)
    //    {
    //        bool isFull = true;
    //        for (int column = 0; column < SIZE; column++)
    //        {
    //            if (data[row, column] != 2)
    //            {
    //                isFull = false;
    //                break;
    //            }
    //        }
    //        if (isFull)
    //        {
    //            fullLineRows.Add(row);
    //        }
    //    }
    //    // Check for full columns
    //    for (int column = 0; column < SIZE; column++)
    //    {
    //        bool isFull = true;
    //        for (int row = 0; row < SIZE; row++)
    //        {
    //            if (data[row, column] != 2)
    //            {
    //                isFull = false;
    //                break;
    //            }
    //        }
    //        if (isFull)
    //        {
    //            fullLineColumns.Add(column);
    //        }
    //    }
    //    // Clear full rows
    //    foreach (var row in fullLineRows)
    //    {
    //        for (int column = 0; column < SIZE; column++)
    //        {
    //            data[row, column] = 0;
    //            cells[row, column].Hide();
    //        }
    //    }
    //    // Clear full columns
    //    foreach (var column in fullLineColumns)
    //    {
    //        for (int row = 0; row < SIZE; row++)
    //        {
    //            data[row, column] = 0;
    //            cells[row, column].Hide();
    //        }
    //    }
    //}
}
