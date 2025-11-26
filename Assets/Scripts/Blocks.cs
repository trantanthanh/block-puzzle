using System;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField] private Block[] blocks;
    [SerializeField] private Board board;

    private void Start()
    {
        var blockWidth = (float)board.Size / blocks.Length;
        var cellSize = (float)board.Size / (blocks[0].Size * blocks.Length + blocks.Length + 1);// Calculate cell size based on board size and number of blocks
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].transform.localPosition = new Vector3(blockWidth * (i + 0.5f), -0.25f - cellSize * 4.0f, 0);
            blocks[i].transform.localScale = new Vector3(cellSize, cellSize, cellSize);
            blocks[i].Initialize();
        }

        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < blocks.Length; ++i)
        {
            blocks[i].Show(0);
        }
    }
}
