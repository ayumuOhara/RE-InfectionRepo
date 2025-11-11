using System.Collections.Generic;
using UnityEngine;

public static class DroppedSpriteRegistry
{
    private static HashSet<Sprite> droppedSprites = new HashSet<Sprite>();

    public static bool IsDropped(Sprite sprite)
    {
        return droppedSprites.Contains(sprite);
    }

    public static void Register(Sprite sprite)
    {
        if (sprite != null)
            droppedSprites.Add(sprite);
    }

    public static void Unregister(Sprite sprite)
    {
        if (sprite != null && droppedSprites.Contains(sprite))
        {
            droppedSprites.Remove(sprite);
        }
    }
}