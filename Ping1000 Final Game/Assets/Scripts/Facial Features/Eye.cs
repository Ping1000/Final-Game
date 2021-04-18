using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour, IColorable
{
    public void SetColor(PersonFeatures.FeatureColor color) {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.color = CustomizationHelpers.FeatureColorToColor(color);
    }
}
