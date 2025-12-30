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

    private void Start()
    {
        for (int row = 0; row < SIZE; row++)
        {
            for (int column = 0; column < SIZE; column++)
            {
                cells[row, column] = Instantiate(cellPrefab, cellsTransform);
                //cells[row, column].SetColor(Color.blue);
                cells[row, column].transform.position = new Vector3(column + 0.5f,row + 0.5f, 0);// Set cell position, pixels to units (1 unit = 1 cell, 1 unit = 100 pixel, 1 cell w = h = 100)
            }
        }
    }
}
