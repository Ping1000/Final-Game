using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PersonFeatures {
    // DO NOT assign values to these; keep them as the default 0, 1, ...
    public enum FeatureColor {
        NONE,
        brown,
        blue,
        green,
        red,
        yellow,
        black
    }


    public static int NumColors {
        get {
            var values = Enum.GetValues(typeof(FeatureColor));
            return values.Length;
        }
    }

    private string FeatureColorToString(FeatureColor color) {
        switch (color) {
            case FeatureColor.brown:
                return "brown";
            case FeatureColor.blue:
                return "blue";
            case FeatureColor.green:
                return "green";
            case FeatureColor.red:
                return "red";
            case FeatureColor.yellow:
                return "yellow";
            case FeatureColor.black:
                return "black";
            default:
                Debug.LogError("Unknown color passed into FeatureColorToString");
                return "unknown color.";
        }
    }

    public static FeatureColor RandomFeatureColor() {
        return (FeatureColor)UnityEngine.Random.Range(1, NumColors);
    }

    // DO NOT assign values to these; keep them as the default 0, 1, ...
    public enum FeatureSize {
        NONE,
        small,
        medium,
        large
    }

    public static int NumSizes {
        get {
            var values = Enum.GetValues(typeof(FeatureSize));
            return values.Length;
        }
    }

    private string FeatureSizeToString(FeatureSize size) {
        switch (size) {
            case FeatureSize.small:
                return "small";
            case FeatureSize.medium:
                return "medium";
            case FeatureSize.large:
                return "large";
            default:
                Debug.LogError("Unknown size passed into FeatureSizeToString");
                return "unknown size.";
        }
    }

    public static FeatureSize RandomFeatureSize() {
        return (FeatureSize)UnityEngine.Random.Range(1, NumSizes);
    }

    // DO NOT assign values to these; keep them as the default 0, 1, ...
    public enum FeatureBool {
        NONE,
        yes,
        no
    }

    public static FeatureBool RandomFeatureBool() {
        return (FeatureBool)UnityEngine.Random.Range(1, 3);
    }

    // might do this differently idk yet
    public FeatureSize earSize;
    public FeatureSize eyeSize;
    public FeatureColor eyeColor;
    public FeatureSize noseSize;
    public FeatureColor hairColor;
    public FeatureBool glasses;
    public FeatureBool hat;
    public FeatureBool facialHair;

    /// <summary>
    /// Generates a person with random (non-NONE) features
    /// </summary>
    public PersonFeatures() {
        // ignore NONEs when generating randoms. Uniformly chosen.
        earSize = RandomFeatureSize();
        eyeSize = RandomFeatureSize();
        eyeColor = RandomFeatureColor();
        noseSize = RandomFeatureSize();
        hairColor = RandomFeatureColor();
        glasses = RandomFeatureBool();
        hat = RandomFeatureBool();
        facialHair = RandomFeatureBool();
    }

    /// <summary>
    /// Generates a person with features matching the inputs
    /// </summary>
    public PersonFeatures(FeatureSize earSize = FeatureSize.NONE,
        FeatureSize eyeSize = FeatureSize.NONE,
        FeatureColor eyeColor = FeatureColor.NONE,
        FeatureSize noseSize = FeatureSize.NONE,
        FeatureColor hairColor = FeatureColor.NONE,
        FeatureBool glasses = FeatureBool.NONE,
        FeatureBool hat = FeatureBool.NONE,
        FeatureBool facialHair = FeatureBool.NONE) {
        this.earSize = earSize;
        this.eyeSize = eyeSize;
        this.eyeColor = eyeColor;
        this.noseSize = noseSize;
        this.hairColor = hairColor;
        this.glasses = glasses;
        this.hat = hat;
        this.facialHair = facialHair;
    }

    /// <summary>
    /// Returns the number of shared features across p1 and p2.
    /// </summary>
    /// <param name="p2"></param>
    /// <returns></returns>
    public int NumSharedFeatures(PersonFeatures p2) {
        int result = 0;

        // could maybe do this better than spaghetti

        if (earSize == p2.earSize)
            result++;
        if (eyeSize == p2.eyeSize)
            result++;
        if (eyeColor == p2.eyeColor)
            result++;
        if (noseSize == p2.noseSize)
            result++;
        if (hairColor == p2.hairColor)
            result++;
        if (glasses == p2.glasses)
            result++;
        if (hat == p2.hat)
            result++;
        if (facialHair == p2.facialHair)
            result++;

        return result;
    }

    public override string ToString() {
        List<string> presentFeatures = new List<string>();

        if (earSize != FeatureSize.NONE)
            presentFeatures.Add(FeatureSizeToString(earSize) + " ears");
        if (eyeSize != FeatureSize.NONE && eyeColor != FeatureColor.NONE) {
            presentFeatures.Add(FeatureSizeToString(eyeSize) + " " +
                FeatureColorToString(eyeColor) + " eyes");
        } else {
            if (eyeSize != FeatureSize.NONE)
                presentFeatures.Add(FeatureSizeToString(eyeSize) + " eyes");
            if (eyeColor != FeatureColor.NONE)
                presentFeatures.Add(FeatureColorToString(eyeColor) + " eyes");
        }
        if (noseSize != FeatureSize.NONE)
            presentFeatures.Add(FeatureSizeToString(earSize) + " nose");
        if (hairColor != FeatureColor.NONE)
            presentFeatures.Add(FeatureColorToString(hairColor) + " hair");
        if (glasses != FeatureBool.NONE)
            presentFeatures.Add("glasses");
        if (hat != FeatureBool.NONE)
            presentFeatures.Add("a hat");
        if (facialHair != FeatureBool.NONE)
            presentFeatures.Add("facial hair");

        switch (presentFeatures.Count) {
            case 0:
                return "";
            case 1:
                return presentFeatures[0] + ".";
            default:
                System.Text.StringBuilder res = new System.Text.StringBuilder(250);
                res.Append(presentFeatures[0]);
                for (int i = 1; i < presentFeatures.Count - 1; i++) {
                    res.Append(", " + presentFeatures[i]);
                }
                res.Append(", and " + presentFeatures[presentFeatures.Count - 1]);
                return res.ToString();
        }
    }
}
