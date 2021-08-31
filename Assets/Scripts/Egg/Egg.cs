using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    // Set in editor
    [SerializeField] private EggVisual eggVisual = null;

    public ColorType TopColor { get; private set; }
    public ColorType BottomColor { get; private set; }
    public bool Selected { get; private set; } // Is it in a match chain?

    void Awake()
    {
        TopColor = ColorType.Red;
        BottomColor = ColorType.Red;
        Selected = false;
    }

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
        TopColor = newTopColor;
        BottomColor = newBottomColor;

        eggVisual.Tint(TopColor, BottomColor);
    }

    // Show that the egg is selected.
    public void SelectEgg(bool select)
    {
        eggVisual.SetHighlight(select);
        Selected = select;
    }
}
