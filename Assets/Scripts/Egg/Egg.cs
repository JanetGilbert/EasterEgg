using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField]
    private EggVisual eggVisual = null;

    private ColorType topColor;
    private ColorType bottomColor;

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

    // Change the logical color of the two halves of the egg.
    public void SetColor(ColorType newTopColor, ColorType newBottomColor)
    {
        topColor = newTopColor;
        bottomColor = newBottomColor;

        eggVisual.Tint(topColor, bottomColor);
    }
}
