using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationHelpers
{
    public static Color FeatureColorToColor(PersonFeatures.FeatureColor color) {
        switch (color) {
            case PersonFeatures.FeatureColor.brown:
                return new Color(45f / 255f, 23f / 255f, 14f / 255f);
            case PersonFeatures.FeatureColor.blue:
                return Color.blue;
            case PersonFeatures.FeatureColor.green:
                return Color.green;
            case PersonFeatures.FeatureColor.red:
                return Color.red;
            case PersonFeatures.FeatureColor.black:
                return Color.black;
            default:
                Debug.LogError("Unknown color passed into FeatureColorToColor");
                return Color.black;
        }
    }
}
