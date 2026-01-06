using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

// Manages a block of cells that can display different polyomino shapes (attach to block prefab)
public class Block : MonoBehaviour
{
    private const int SIZE = 5;// Size of the block in cells (5x5)
    [SerializeField] private Cell cellPrefab;

    private readonly Cell[,] cells = new Cell[SIZE, SIZE];
    private Vector3 previousMousePosition = Vector3.positiveInfinity;

    private Vector3 originalPosition;//use to reset position on release
    private Vector3 scale;//scale to fix width at bottom.

    private Vector3 inputPoint;//use to track input position
    private const float inputPointMultiple = 1.4f;

    private Camera mainCamera;
    private Vector3 previousMouseWorldPosition = Vector3.positiveInfinity;
    private Vector2Int previousDragPoint;
    private Vector2Int currentDragPoint;//use to caculate position on board
    private Vector2 center;

    [Header("Config")]
    [SerializeField] private Vector3 inputOffset = new Vector3(0.0f, 2.0f, 0.0f);

    public int Size
    {
        get => SIZE;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
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
        originalPosition = transform.localPosition;
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
        inputPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.localPosition = originalPosition + inputOffset;
        transform.localScale = Vector3.one;
        previousMousePosition = Input.mousePosition;
        currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position - center);
        Debug.Log($"Current Drag Point: {currentDragPoint}");
    }

    void OnMouseDrag()
    {
        var currentMousePosition = Input.mousePosition;
        if (previousMousePosition != currentMousePosition)
        {
            Debug.Log("Block dragged!");
            previousMousePosition = currentMousePosition;
            var inputDelta = mainCamera.ScreenToWorldPoint(currentMousePosition) - inputPoint;
            transform.localPosition = originalPosition + inputOffset + inputDelta * inputPointMultiple;

            currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position - center);//update drag point
            if (currentDragPoint != previousDragPoint)
            {
                Debug.Log($"Current Drag Point: {currentDragPoint}");
                previousDragPoint = currentDragPoint;
            }
        }
    }
    void OnMouseUp()
    {
        Debug.Log("Block released!");
        transform.localPosition = originalPosition;
        transform.localScale = scale;
        previousMousePosition = Vector3.positiveInfinity;
    }
}
