using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Controls the appearance of a normal egg
public class EggVisual : MonoBehaviour
{
    // Set in editor
    [SerializeField] private SpriteRenderer topRenderer = null;
    [SerializeField] private SpriteRenderer bottomRenderer = null;
    [SerializeField] private SpriteRenderer wholeRenderer = null;
    [SerializeField] private SpriteRenderer highlightRenderer = null;

    public bool Highlighted { get; private set; } // Is egg selected in chain?

    void Awake()
    {
        Assert.IsNotNull(topRenderer);
        Assert.IsNotNull(bottomRenderer);
        Assert.IsNotNull(wholeRenderer);
        Assert.IsNotNull(highlightRenderer);

        Highlighted = false;
    }

    void Start()
    {

    }


    void Update()
    {

    }

    // Change the display color of the two halves of the egg
    public void Tint(ColorType topColor, ColorType bottomColor)
    {
        topRenderer.color = GameManager.Instance.EggSetup.GetRGB(topColor);
        bottomRenderer.color = GameManager.Instance.EggSetup.GetRGB(bottomColor);
    }

    // Get the total size of both halves of the egg.
    public Vector2 GetEggWorldSize()
    {
        return wholeRenderer.bounds.size;
    }

    // Set highlight on/off.
    public void SetHighlight(bool highlight)
    {
        Highlighted = highlight;
        highlightRenderer.gameObject.SetActive(highlight);
    }
}
