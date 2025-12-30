//Shape of block pieces to drag
public static class Polyominoes
{
    private static readonly int[][,] polyominoes = new int[][,]
    {
        new int [,]
        {
           {0, 0, 1},
           {0, 0, 1},
           {1, 1, 1}
        }//L shape
    };//array of 2d arrays representing polyomino shapes

    static Polyominoes()
    {
        foreach (var polyomino in polyominoes)
        {
            ReverseRows(polyomino);
        }
    }

    public static int[,] Get(int index) => polyominoes[index];
    public static int Length => polyominoes.Length;

    private static void ReverseRows(int[,] polyomino)
    {
        int polyominoRows = polyomino.GetLength(0);
        int polyominoColumns = polyomino.GetLength(1);

        for (int rows = 0; rows < polyominoRows / 2; rows++)
        {
            int topRow = rows;
            int bottomRow = polyominoRows - 1 - rows;

            for (int columns = 0; columns < polyominoColumns; columns++)
            {
                (polyomino[bottomRow, columns], polyomino[topRow, columns]) = (polyomino[topRow, columns], polyomino[bottomRow, columns]);
            }
        }
    }
}
