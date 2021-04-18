using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonFeatures {
    // DO NOT assign values to these; keep them as the default 0, 1, ...
    public enum FeatureColor {
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

    // DO NOT assign values to these; keep them as the default 0, 1, ...
    public enum FeatureSize {
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

    // might do this differently idk yet
    public FeatureColor eyes;
    public FeatureColor hair;
    public FeatureSize ears;

    /// <summary>
    /// Additional distinguishing items (hat, etc)
    /// </summary>
    public List<string> accessories;

    public PersonFeatures() {
        eyes = (FeatureColor)UnityEngine.Random.Range(0, NumColors - 1);
        hair = (FeatureColor)UnityEngine.Random.Range(0, NumColors - 1);
        ears = (FeatureSize)UnityEngine.Random.Range(0, NumSizes - 1);
    }

    public PersonFeatures(FeatureColor eyes, FeatureColor hair,
        FeatureSize ears, List<string> accessories) {
        this.eyes = eyes;
        this.hair = hair;
        this.ears = ears;
        this.accessories = accessories;
    }

    /// <summary>
    /// Returns the number of shared features across p1 and p2.
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public int NumSharedFeatures(PersonFeatures p2) {
        int result = 0;

        // could maybe do this better than spaghetti

        if (eyes == p2.eyes)
            result++;
        if (hair == p2.hair)
            result++;
        if (ears == p2.ears)
            result++;

        if (accessories == null)
            return result;

        foreach (string elem in accessories) {
            if (p2.accessories.Contains(elem))
                result++;
        }

        return result;
    }

    public override string ToString() {
        string res = "";

        switch (GameManager.featuresToCheckFor) {
            case 1:
                res += FeatureColorToString(eyes) + " eyes.";
                goto default;
            case 2:
                res += FeatureColorToString(hair) + " hair, ";
                goto case 1;
            case 3:
                res += FeatureSizeToString(ears) + " ears, ";
                goto case 2;
            default:
                break;
        }

        if (accessories == null)
            return res;

        res += "{wc}";
        foreach (string elem in accessories) {
            res += elem + ", ";
        }
        res += ".";

        return res;
    }
}
