using UnityEngine;

public class Board : MonoBehaviour
{
    private const int SIZE = 8;
    public int Size
    {
        get => SIZE;
    }

    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;
    private readonly Cell[,] cells = new Cell[SIZE, SIZE];

    private void Start()
    {
        for (int row = 0; row < SIZE; row++)
        {
            for (int column = 0; column < SIZE; column++)
            {
                cells[row, column] = Instantiate(cellPrefab, cellsTransform);
                cells[row, column].transform.position = new Vector3(row, column, 0);// Set cell position, pixels to units (1 unit = 1 cell, 1 unit = 100 pixel, 1 cell w = h = 100)
            }
        }
    }
}
