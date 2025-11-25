
public static class Polyominoes
{
    private static readonly int[][,] polyominoes = new int[][,]
    {
        new int [,]
        {
           {0, 0, 1},
           {0, 0, 1},
           {1, 1, 1}
        }
    };

    public static int[,] Get(int index) => polyominoes[index];
    public static int Length => polyominoes.Length;
}
