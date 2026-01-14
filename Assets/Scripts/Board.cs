using System.Collections.Generic;
using UnityEngine;

// Manages the game board made up of cells (attach to Board GameObject)
public class Board : MonoBehaviour
{
    private const int BOARD_SIZE = 8;
    public int Size
    {
        get => BOARD_SIZE;
    }

    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;// Parent transform for cells
    private readonly Cell[,] cells = new Cell[BOARD_SIZE, BOARD_SIZE];// 2D array to hold cell references
    private readonly CellState[,] data = new CellState[BOARD_SIZE, BOARD_SIZE];// CellState

    #region Manage Hover Points
    private readonly List<Vector2Int> hoverPoints = new();
    private readonly List<int> highlightPolymominoColums = new();
    private readonly List<int> highlightPolymominoRows = new();
    private readonly List<int> fullLineColumns = new();
    private readonly List<int> fullLineRows = new();
    #endregion

    private void Start()
    {
        for (int row = 0; row < BOARD_SIZE; row++)
        {
            for (int column = 0; column < BOARD_SIZE; column++)
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
            || point.x >= BOARD_SIZE
            || point.y < 0
            || point.y >= BOARD_SIZE
            || data[point.y, point.x] != CellState.Empty)
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

        UnHover();// Clear previous hover
        UnHighlight();
        highlightPolymominoColums.Clear();
        highlightPolymominoRows.Clear();

        GetHoverPoints(point, polyominoRows, polyominoColumns, polyomino);

        if (hoverPoints.Count > 0)
        {
            Hover();// Show new hover
            Highlight(point, polyominoColumns, polyominoRows);// Highlight full lines

            foreach (var column in fullLineColumns)
            {
                highlightPolymominoColums.Add(column - point.x);
            }

            foreach (var row in fullLineRows)
            {
                highlightPolymominoRows.Add(row - point.y);
            }
        }
    }

    private void Hover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = CellState.Hover;
            cells[hoverPoint.y, hoverPoint.x].Hover();
        }
    }

    private void UnHover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = CellState.Empty;
            cells[hoverPoint.y, hoverPoint.x].Hide();
        }
        hoverPoints.Clear();
    }

    // Place a polyomino at a specific board point after mouse up
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
            data[hoverPoint.y, hoverPoint.x] = CellState.Normal;
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

    //check full lines column in the given range
    private void FullLinesColumn(int fromColum, int toColumn)
    {
        fullLineColumns.Clear();
        for (int column = fromColum; column < toColumn; column++)
        {
            bool isFull = true;
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                if (data[row, column] != CellState.Normal)
                {
                    //it's empty cell or hover cell found
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

    //check full lines row in the given range
    private void FullLinesRow(int fromRow, int toRow)
    {
        fullLineRows.Clear();
        for (int row = fromRow; row < toRow; row++)
        {
            bool isFull = true;
            for (int column = 0; column < BOARD_SIZE; column++)
            {
                if (data[row, column] != CellState.Normal)
                {
                    //it's empty cell or hover cell found
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
            for (int column = 0; column < BOARD_SIZE; column++)
            {
                data[row, column] = CellState.Empty;
                cells[row, column].Hide();
            }
        }
        // Clear full columns
        foreach (var column in fullLineColumns)
        {
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                data[row, column] = CellState.Empty;
                cells[row, column].Hide();
            }
        }
    }

    private void UnHighlight()
    {
        // Reset previously highlighted cells
        foreach (var row in fullLineRows)
        {
            for (int column = 0; column < BOARD_SIZE; column++)
            {
                if (data[row, column] == CellState.Normal)
                {
                    cells[row, column].Normal();
                }
            }
        }

        foreach (var column in fullLineColumns)
        {
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                if (data[row, column] == CellState.Normal)
                {
                    cells[row, column].Normal();
                }

            }
        }
    }

    private void Highlight(Vector2Int point, int polyominoColumn, int polyominoRow)
    {
        PredictHighlightRow(point.y, point.y + polyominoRow);
        PredictHighlightColumn(point.x, point.x + polyominoColumn);

        HighlightLinesPredict();
    }

    private void HighlightLinesPredict()
    {
        foreach (var row in fullLineRows)
        {
            for (int column = 0; column < BOARD_SIZE; column++)
            {
                if (data[row, column] == CellState.Normal)
                {
                    cells[row, column].Highlight();
                }
            }
        }

        foreach (var column in fullLineColumns)
        {
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                if (data[row, column] == CellState.Normal)
                {
                    cells[row, column].Highlight();
                }
            }
        }
    }

    private void PredictHighlightColumn(int fromColum, int toColumn)
    {
        fullLineColumns.Clear();
        for (int column = fromColum; column < toColumn; column++)
        {
            bool isFull = true;
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                if (data[row, column] != CellState.Hover
                    && data[row, column] != CellState.Normal)
                {
                    //it's empty cell found
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

    private void PredictHighlightRow(int fromRow, int toRow)
    {
        fullLineRows.Clear();
        for (int row = fromRow; row < toRow; row++)
        {
            bool isFull = true;
            for (int column = 0; column < BOARD_SIZE; column++)
            {
                if (data[row, column] != CellState.Hover
                    && data[row, column] != CellState.Normal)
                {
                    //it's empty cell
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

    public bool CheckPlace(int polyominoIndex)
    {
        var polyomino = Polyominoes.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoColumns = polyomino.GetLength(1);

        for (int row = 0; row < BOARD_SIZE - polyominoRows; ++row)
        {
            for (int column = 0; column < BOARD_SIZE - polyominoColumns; ++column)
            {
                if (CanPlace(column, row, polyominoColumns, polyominoRows, polyomino))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CanPlace(int column, int row, int polyominoColumns, int polyominoRows, int[,] polyomino)
    {
        for (int r = 0; r < polyominoRows; ++r)
        {
            for (int c = 0; c < polyominoColumns; ++c)
            {
                if (polyomino[r, c] > 0 && data[row + r, column + c] == CellState.Normal)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public List<int> HightlightPoloymominoColums => highlightPolymominoColums;
    public List<int> HightlightPoloymominoRows => highlightPolymominoRows;
}
