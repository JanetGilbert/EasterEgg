using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Data about board setup and levels.

[CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/BoardData")]
public class BoardData : ScriptableObject
{
    // Set in editor
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private int chainLengthMin = 3;

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
    public int ChainLengthMin => chainLengthMin;

    // Level data
    [System.Serializable]
    private class LevelDef
    {
        public ColorType maxColor = ColorType.Red;
    }

    [SerializeField]
    private LevelDef[] levelData = null;

    public int MaxLevel => levelData.Length;

    public ColorType GetMaxColor(int level)
    {
        Assert.IsTrue(level < levelData.Length);

        return levelData[level].maxColor;
    }




}
