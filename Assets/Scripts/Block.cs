using UnityEngine;

public class Block : MonoBehaviour
{
    private const int SIZE = 5;// Size of the block in cells (5x5)
    [SerializeField] private Cell cellPrefab;

    private readonly Cell[,] cells = new Cell[SIZE, SIZE];

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
    }

    public void Show(int polyominoIndex)
    {
        this.Hide();
        int[,] polyomino = Polyominoes.Get(polyominoIndex);
        int polyominoRows = polyomino.GetLength(0);
        int polyominoColumns = polyomino.GetLength(1);

        for (int row = 0; row < polyominoRows; ++row)
        {
            for (int column = 0; column < polyominoColumns; ++column)
            {
                if (polyomino[row, column] > 0)
                {
                    cells[row, column].transform.localPosition = new Vector3(column, row, 0.0f);
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
}
