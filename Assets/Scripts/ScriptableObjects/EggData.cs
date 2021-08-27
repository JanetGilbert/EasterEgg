using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data about eggs.

[System.Serializable]
public struct ColorData
{
    public ColorType colorType;
    public Color rgba;
}

[CreateAssetMenu(fileName = "EggData", menuName = "ScriptableObjects/EggData")]
public class EggData : ScriptableObject
{
    [SerializeField] private ColorData [] colors = null;

    // Translate from Color ID to RGBA data
    public Color GetRGB(ColorType type)
    {
        foreach (ColorData colorData in colors)
        {
            if (colorData.colorType == type)
            {
                return colorData.rgba;
            }
        }

        return Color.magenta;
    }
}
