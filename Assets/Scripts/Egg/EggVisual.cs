using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Controls the appearance of a normal egg
public class EggVisual : MonoBehaviour
{
     // Set in editor
    [SerializeField]
    private SpriteRenderer topRenderer = null;

    [SerializeField]
    private SpriteRenderer bottomRenderer = null;

    [SerializeField]
    private SpriteRenderer wholeRenderer = null;

    void Start()
    {
        Assert.IsNotNull(topRenderer);
        Assert.IsNotNull(bottomRenderer);
        Assert.IsNotNull(wholeRenderer);

        //Tint(ColorType.Red, ColorType.Blue);
    }


    void Update()
    {

    }

    // Change the color of the two halves of the egg
    public void Tint(ColorType topColor, ColorType bottomColor)
    {
        topRenderer.color = GameManager.Instance.eggData.GetRGB(topColor);
        bottomRenderer.color = GameManager.Instance.eggData.GetRGB(bottomColor);
    }

    // Get the total size of both halves of the egg.
    public Vector2 GetEggWorldSize()
    {
        //float eggHalvesDistance = topRenderer.bounds.center.y - bottomRenderer.bounds.center.y;
        // float worldHeight = eggHalvesDistance + topRenderer.bounds.extents.y + bottomRenderer.bounds.extents.y;

        //return new Vector2(topRenderer.bounds.size.x, worldHeight);

        return wholeRenderer.bounds.size;
    }
}