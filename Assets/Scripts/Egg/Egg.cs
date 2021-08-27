using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    // Set in editor
    [SerializeField] private EggVisual eggVisual = null;

    
    private ColorType topColor;
    private ColorType bottomColor;
    private bool selected; // Is it in a match chain?

    void Start()
    {
        topColor = ColorType.Red;
        bottomColor = ColorType.Red;
        selected = false;
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

    // Show that the egg is selected.
    public void SelectEgg(bool select)
    {
        eggVisual.Highlight(select);
    }
}
