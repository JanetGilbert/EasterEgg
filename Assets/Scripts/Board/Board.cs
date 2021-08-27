using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Contains and controls the individual egg pieces.
public class Board : MonoBehaviour
{
    // Set in editor
    [SerializeField]
    private Egg eggPrefab = null;

    [SerializeField]
    private RectTransform gamePanelPosition = null; // The position of the UI panel where the game pieces will be placed.

    // Eggs contained in board.
    private Egg[,] eggs;
    private List<Egg> eggPool = null;

    // Cached
    private Vector2Int gridSize;

    // Game state
    private int level = 0;


    void Start()
    {
        Assert.IsNotNull(eggPrefab);
        Assert.IsNotNull(gamePanelPosition);

        gridSize = new Vector2Int(GameManager.Instance.boardData.GridWidth, GameManager.Instance.boardData.GridHeight);

        MakeEggPool();
        ClearBoard();
        RandomizeEggs();
    }

    void Update()
    {
        
    }

    // Create Egg object pool, but do not set up eggs.
    private void MakeEggPool()
    {
        Assert.IsNull(eggPool);

        eggs = new Egg[gridSize.x, gridSize.y];
        eggPool = new List<Egg>();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                eggs[x, y] = Instantiate(eggPrefab, transform.position, Quaternion.identity, transform);
                eggs[x, y].name = "Egg_" + x + "_" + y; // To help debug grid

                eggPool.Add(eggs[x, y]);
            }
        }
    }

    // Return all eggs to default position.
    private void ClearBoard()
    {
        Assert.IsNotNull(eggPool);
        Assert.AreEqual(eggPool.Count, (gridSize.x * gridSize.y));

        // Get egg prefab world size.
        Vector2 eggPrefabWorldSize  = eggPrefab.GetEggWorldSize();

        // Find the world size and position of the board on screen, via a placeholder panel in the UI.
        Vector3[] boardCorners      = new Vector3[4]; gamePanelPosition.GetWorldCorners(boardCorners); // Bottom left, top left, top right, bottom right
        Vector2 boardWorldSize      = new Vector2(boardCorners[3].x - boardCorners[0].x, boardCorners[1].y - boardCorners[0].y);
        Vector2 boardWorldPos       = boardCorners[0]; // Board (0,0) is the bottom left of the board.

        // Scale the eggs to fit in the board space.
        Vector2 eggWorldSize        = new Vector2(boardWorldSize.x / (float)gridSize.x, boardWorldSize.y / (float)gridSize.y);
        float eggScaleY             = eggWorldSize.y / eggPrefabWorldSize.y;
        Vector2 eggScale            = new Vector2(eggScaleY, eggScaleY);

        // Lay out the grid of eggs
        int curEgg = 0;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                eggs[x, y] = eggPool[curEgg]; // Restore egg from pool to grid.

                Vector3 eggWorldPos = new Vector3(((float)x * eggWorldSize.x) + boardWorldPos.x + (eggWorldSize.x / 2.0f), 
                                                  ((float)y * eggWorldSize.y) + boardWorldPos.y + (eggWorldSize.y / 2.0f), 
                                                  transform.position.z);
                eggs[x, y].transform.position = eggWorldPos;
                eggs[x, y].transform.localScale = eggScale;
                eggs[x, y].name = "Egg_" + x + "_" + y; // To help debug grid

                curEgg++;
            }
        }
    }

    private void RandomizeEggs()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                ColorType colorA = (ColorType)Random.Range(0,(int)GameManager.Instance.boardData.GetMaxColor(level));
                ColorType colorB = (ColorType)Random.Range(0,(int)GameManager.Instance.boardData.GetMaxColor(level));
                eggs[x, y].SetColor(colorA, colorB);
            }
        }
    }
}
