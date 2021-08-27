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

    void Start()
    {
        Assert.IsNotNull(eggPrefab);
        Assert.IsNotNull(gamePanelPosition);

        MakeBoard();
    }

    void Update()
    {
        
    }

    // Create Egg pool
    private void MakeBoard()
    {
        // Get egg prefab world size.
        Vector2 eggPrefabWorldSize = eggPrefab.GetEggWorldSize();

        // Get grid size.
        Vector2Int gridSize     = new Vector2Int(GameManager.Instance.boardData.GridWidth, GameManager.Instance.boardData.GridHeight);

        // Find the world size and position of the board on screen, via a placeholder panel in the UI.
        Vector3[] boardCorners  = new Vector3[4];
        gamePanelPosition.GetWorldCorners(boardCorners); // Bottom left, top left, top right, bottom right
        Vector2 boardWorldSize  = new Vector2(boardCorners[3].x - boardCorners[0].x, 
                                              boardCorners[1].y - boardCorners[0].y);
        Vector2 boardWorldPos = boardCorners[0]; // Board (0,0) is the bottom left of the board.

        // Scale the eggs to fit in the board space.
        Vector2 eggWorldSize    = new Vector2(boardWorldSize.x / (float)gridSize.x, 
                                              boardWorldSize.y / (float)gridSize.y);
        float eggScaleY         = eggWorldSize.y / eggPrefabWorldSize.y;
        Vector2 eggScale        = new Vector2(eggScaleY, eggScaleY);

        // Lay out the grid of eggs
        eggs = new Egg[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 eggWorldPos = new Vector3(((float)x * eggWorldSize.x) + boardWorldPos.x + (eggWorldSize.x / 2.0f), 
                                                  ((float)y * eggWorldSize.y) + boardWorldPos.y + (eggWorldSize.y / 2.0f), 
                                                  transform.position.z);
                eggs[x, y] = Instantiate(eggPrefab);//, eggWorldPos, Quaternion.identity, transform);
                eggs[x, y].transform.localScale = eggScale;
                eggs[x, y].transform.position = eggWorldPos;
                eggs[x, y].transform.parent = transform;
                eggs[x, y].name = "Egg_" + x + "_" + y; // To help debug grid
            }
        }
    }

    
}
