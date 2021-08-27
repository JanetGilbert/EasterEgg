using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Contains and controls the individual egg pieces.
public class Board : MonoBehaviour
{
    // Set in editor
    [SerializeField] private Egg eggPrefab = null;
    [SerializeField] private RectTransform gamePanelPosition = null; // The position of the UI panel where the game pieces will be placed.

    // Eggs
    private Egg[,] eggs;
    private List<Egg> eggPool = null;

    // Cached
    private Vector2Int gridSize; // Size of grid, in cells.
    private Vector2 boardWorldSize; // Size of board, in world coordinates.
    private Vector2 boardWorldPos; // Bottom-left of board, in world coordinates.
    private Vector2 eggWorldSize; // Size of egg cell, in world coordinates.

    // Game state
    private int level = 0;


    void Start()
    {
        Assert.IsNotNull(eggPrefab);
        Assert.IsNotNull(gamePanelPosition);

        gridSize = new Vector2Int(GameManager.Instance.BoardSetup.GridWidth, GameManager.Instance.BoardSetup.GridHeight);

        MakeEggPool();
        ClearBoard();
        RandomizeEggs();
    }

    void Update()
    {
        CheckInput();
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

    // Return all eggs to default position and set up cached board position variables.
    private void ClearBoard()
    {
        Assert.IsNotNull(eggPool);
        Assert.AreEqual(eggPool.Count, (gridSize.x * gridSize.y));

        // Get egg prefab world size.
        Vector2 eggPrefabWorldSize  = eggPrefab.GetEggWorldSize();

        // Find the world size and position of the board on screen, via a placeholder panel in the UI.
        Vector3[] boardCorners      = new Vector3[4]; gamePanelPosition.GetWorldCorners(boardCorners); // Bottom left, top left, top right, bottom right
        boardWorldSize              = new Vector2(boardCorners[3].x - boardCorners[0].x, boardCorners[1].y - boardCorners[0].y);
        boardWorldPos               = boardCorners[0]; // Board (0,0) is the bottom left of the board.

        // Scale the eggs to fit in the board space.
        eggWorldSize                = new Vector2(boardWorldSize.x / (float)gridSize.x, boardWorldSize.y / (float)gridSize.y);
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

    // Randomize the colors of both halves of the eggs according to level.
    private void RandomizeEggs()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                ColorType colorA = (ColorType)Random.Range(0,(int)GameManager.Instance.BoardSetup.GetMaxColor(level));
                ColorType colorB = (ColorType)Random.Range(0,(int)GameManager.Instance.BoardSetup.GetMaxColor(level));
                eggs[x, y].SetColor(colorA, colorB);
            }
        }
    }

    private void CheckInput()
    {
        // Mouse
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));

        if (Input.GetMouseButton(0)) // Left click
        {
            Rect boardRect = new Rect(boardWorldPos.x, boardWorldPos.y, boardWorldSize.x, boardWorldSize.y);

            if (boardRect.Contains(mousePosWorld))
            {
                int x = (int)((mousePosWorld.x - boardWorldPos.x) / eggWorldSize.x);
                int y = (int)((mousePosWorld.y - boardWorldPos.y) / eggWorldSize.y);

                Debug.Log("x:" + x + " y:" + y);

                eggs[x, y].SelectEgg(true);
            }
        }


        // Touch

    }
}
