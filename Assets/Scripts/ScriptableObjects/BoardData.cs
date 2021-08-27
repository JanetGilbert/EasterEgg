using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/BoardData")]
public class BoardData : ScriptableObject
{
    [SerializeField]
    private int gridWidth = 10;

    [SerializeField]
    private int gridHeight = 10;

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
}
