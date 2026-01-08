using System;
using UnityEngine;

// Manages multiple blocks and positions them on the board (attach to Blocks GameObject)
public class Blocks : MonoBehaviour
{
    [SerializeField] private Block[] blocks;
    [SerializeField] private Board board;
    private int blockCount = 0;

    private void Start()
    {
        var blockWidth = (float)board.Size / blocks.Length;// Calculate width allocated for each block
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
            blocks[i].gameObject.SetActive(true);
            blocks[i].Show(0);
            blockCount++;
        }
    }

    public void Remove()
    {
        --blockCount;
        if (blockCount <= 0)
        {
            blockCount = 0;
            Generate();
        }
    }
}
