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
    private Vector2 eggWorldSize; // Size of egg cell, in world coordinates.
    private Rect boardRect; // Position and size of board

    // Game state
    private int level = 0; // Current game level
    private int prevSelectedX =-1; // Keep track of which was the most recent selected grid square.
    private int prevSelectedY = -1;

    // Selection chain
    private List<Vector2Int> chain = new List<Vector2Int>();


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
        Vector2 boardWorldSize      = new Vector2(boardCorners[3].x - boardCorners[0].x, boardCorners[1].y - boardCorners[0].y);
        Vector2 boardWorldPos       = boardCorners[0]; // Board (0,0) is the bottom left of the board.
        boardRect                   = new Rect(boardWorldPos.x, boardWorldPos.y, boardWorldSize.x, boardWorldSize.y);

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
        bool pressedThisFrame = Input.GetMouseButtonDown(0);
        bool releasedThisFrame = Input.GetMouseButtonUp(0);
        bool mouseDown = Input.GetMouseButton(0);
        
        // No input this frame.
        if (!releasedThisFrame && !mouseDown)
        {
            return; 
        }

        // Mouse Released anywhere: check if there is a valid chain of eggs.
        if (releasedThisFrame)
        {
            RemoveChain();

            return; 
        }

        // Mouse/touch position is not on the board.
        Vector2 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
        if (!boardRect.Contains(position))
        {
            return;
        }

        // Calculate grid position of click/tap.
        int x = (int)((position.x - boardRect.x) / eggWorldSize.x);
        int y = (int)((position.y - boardRect.y) / eggWorldSize.y);


        Vector2Int lastEggPos = chain.Count >= 1 ? chain[chain.Count - 1]:new Vector2Int(-1, -1);
        Vector2Int prevEggPos = chain.Count>=2?chain[chain.Count - 2]:new Vector2Int(-1,-1);

        if (chain.Count == 0) // Start the chain.
        {
            eggs[x, y].SelectEgg(true);
            chain.Add(new Vector2Int(x, y));
        }
        else if (prevEggPos.x == x && prevEggPos.y == y) // We are going back on the chain: remove the last egg.
        {
            eggs[lastEggPos.x, lastEggPos.y].SelectEgg(false);
            chain.RemoveAt(chain.Count - 1);

        }
        else // Add the egg at the mouse position to the chain.
        {     
            Egg lastEgg = eggs[lastEggPos.x, lastEggPos.y];

            if (lastEgg != null && (prevSelectedX != x || prevSelectedY != y))
            {
                if (CanLink(lastEggPos, new Vector2Int(x, y))) // Add egg to chain.
                {
                    eggs[x, y].SelectEgg(true);
                    chain.Add(new Vector2Int(x, y));
                }
            }
        }

        prevSelectedX = x;
        prevSelectedY = y;
    }

    private void RemoveChain()
    {
        if (chain.Count > GameManager.Instance.BoardSetup.ChainLengthMin)
        {
            foreach (Vector2Int pos in chain)
            {
                // eggs[pos.x, pos.y] = null; /// TODO fix this
            }
        }

        foreach (Vector2Int pos in chain)
        {
            eggs[pos.x, pos.y].SelectEgg(false);
        }

        chain.Clear();

  
    }

    private bool CanLink(Vector2Int start, Vector2Int end)
    {
        Egg egg1 = eggs[start.x, start.y];
        Egg egg2 = eggs[end.x, end.y];

        // The eggs must not be null
        if (egg1 == null || egg2 == null)
        {
            return false;
        }

        // The eggs must be horizontally or vertically adjacent.
        Vector2Int dist = start - end;

        if (dist.sqrMagnitude != 1)
        {
            return false;
        }

        // Is there a color match?
        return (egg1.TopColor == egg2.TopColor ||
                egg1.TopColor == egg2.BottomColor ||
                egg1.BottomColor == egg2.TopColor ||
                egg1.BottomColor == egg2.BottomColor);
    }
}
