using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton that manages overall game state.
public class GameManager : Singleton<GameManager>
{
    // Scriptable objects
    [SerializeField] private EggData eggData = null; // Information about egg setup
    [SerializeField] private BoardData boardData = null; // Information about board setup

    public EggData EggSetup => eggData;
    public BoardData BoardSetup => boardData;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
