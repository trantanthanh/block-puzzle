using UnityEngine;

// Manages a block of cells that can display different polyomino shapes (attach to block prefab)
public class Block : MonoBehaviour
{
    private const int SIZE = 5;// Size of the block in cells (5x5)
    [SerializeField] private Cell cellPrefab;

    private readonly Cell[,] cells = new Cell[SIZE, SIZE];
    private Vector3 previousMousePosition = Vector3.positiveInfinity;

    private Vector3 position;//use to move while dragging
    private Vector3 scale;//use to zoom while dragging

    public int Size
    {
        get => SIZE;
    }

    public void Initialize()
    {
        for (int row = 0; row < SIZE; ++row)
        {
            for (int column = 0; column < SIZE; ++column)
            {
                cells[row, column] = Instantiate(cellPrefab, transform);
            }
        }
        position = transform.localPosition;
        scale = transform.localScale;
    }

    public void Show(int polyominoIndex)
    {
        this.Hide();
        int[,] polyomino = Polyominoes.Get(polyominoIndex);
        int polyominoRows = polyomino.GetLength(0);// number of rows in the polyomino
        int polyominoColumns = polyomino.GetLength(1);// number of columns in the polyomino

        Vector2 center = new Vector2(polyominoColumns * 0.5f, polyominoRows * 0.5f);

        for (int row = 0; row < polyominoRows; ++row)
        {
            for (int column = 0; column < polyominoColumns; ++column)
            {
                if (polyomino[row, column] > 0)
                {
                    cells[row, column].transform.localPosition = new Vector3(column - center.x + 0.5f, row - center.y + 0.5f, 0.0f);
                    cells[row, column].Normal();
                }
            }
        }
    }

    private void Hide()
    {
        for (int row = 0; row < SIZE; ++row)
        {
            for (int column = 0; column < SIZE; ++column)
            {
                cells[row, column].Hide();
            }
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Block clicked!");
        previousMousePosition = Input.mousePosition;
        transform.localPosition = position + new Vector3(0.0f, 2.0f, 0.0f);
    }

    void OnMouseDrag()
    {
        var currentMousePosition = Input.mousePosition;
        if (previousMousePosition != currentMousePosition)
        {
            Debug.Log("Block dragged!");
            previousMousePosition = currentMousePosition;
        }
    }
    void OnMouseUp()
    {
        Debug.Log("Block released!");
        previousMousePosition = Vector3.positiveInfinity;
    }
}
