using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDragonHelper
{
    public static Color SetColor(ColorPart partColor)
    {
        switch (partColor)
        {
            case ColorPart.Green:
                return Color.green;
            
            case ColorPart.Red:
                return  Color.red;
            
            case ColorPart.Pink:
                return  Color.magenta;
        }
        
        return Color.white;
    }

}
