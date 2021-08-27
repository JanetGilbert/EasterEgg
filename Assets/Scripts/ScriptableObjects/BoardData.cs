using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;



[CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/BoardData")]
public class BoardData : ScriptableObject
{
    // Grid

    [SerializeField]
    private int gridWidth = 10;

    [SerializeField]
    private int gridHeight = 10;

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;

    // Level data

    [System.Serializable]
    private struct LevelDef
    {
        public ColorType maxColor;
    }

    [SerializeField]
    private LevelDef[] levelData;

    public int MaxLevel => levelData.Length;

    public ColorType GetMaxColor(int level)
    {
        Assert.IsTrue(level < levelData.Length);

        return levelData[level].maxColor;
    }




}
