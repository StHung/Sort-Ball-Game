using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorPicker
{
    private static Dictionary<int, Color> colors;

    static ColorPicker()
    {
        colors = new Dictionary<int, Color>();
        colors.Add(0, new Color(1f, 1f, 1f));
        colors.Add(1, new Color(0f, 0f, 0f));
        colors.Add(2, new Color(1f, 0f, 0f));
        colors.Add(3, new Color(1f, 1f, 0f));
        colors.Add(4, new Color(0f, 1f, 0f));
        colors.Add(5, new Color(0f, 0f, 1f));
        colors.Add(6, new Color(1f, 0f, 1f));
        colors.Add(7, new Color(1f, 0.5f, 0f));
        colors.Add(8, new Color(0.7f, 0f, 1f));
        colors.Add(9, new Color(0.5f, 0f, 0f));
        colors.Add(10, new Color(0.5f, 0f, 0f));
    }

    public static Color GetColor(int colorIndex)
    {
        if (colorIndex >= colors.Count)
            return Color.white;
        return colors[colorIndex];
    }

    public static int GetLength()
    {
        return colors.Count;
    }
}
public enum BallColor
{
    White,
    Black,
    Red,
    Yellow,
    Green,
    Blue,
    Pink,
    Orange,
    Violet,
    Brown,
    LightBlue
}
