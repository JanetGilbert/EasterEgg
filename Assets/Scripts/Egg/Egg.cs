using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField]
    private EggVisual eggVisual = null;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    // The size of the egg in world space.
    public Vector2 GetEggWorldSize()
    {
        return eggVisual.GetEggWorldSize();
    }
}
