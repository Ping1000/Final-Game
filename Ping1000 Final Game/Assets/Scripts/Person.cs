using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the Person GameObjects
/// </summary>
public class Person : MonoBehaviour
{
    public PersonFeatures features;
    [HideInInspector]
    public string offByOneFeature = null;

    public static Person activePerson;

    private void Awake() {
        activePerson = this;
    }

    // Start is called before the first frame update
    void Start() {
        // using invoke sucks but i'm keeping it for now
        Invoke("UpdateBasketPerson", Time.deltaTime);
        SFXManager.PlayNewSound("Customer_Enter", volumeType.half);
    }

    private void UpdateBasketPerson() {
        foreach (Basket b in FindObjectsOfType<Basket>()) {
            b.UpdatePerson();
        }
    }

    /// <summary>
    /// For a hidden person, returns a string corresponding to the one feature
    /// that is different from b
    /// </summary>
    /// <param name="b">Basket to compare with</param>
    /// <returns></returns>
    public string GetOffByOneFeature(Basket b) {
        PersonFeatures f = b.basketFeatures;

        if (b.hiddenMatches.Find(item => {
            return item.GetComponent<Person>().features.NonNoneEquals(features);
        }) == null) {
            // not in offbyonelist, don't even consider it.
            // might be a bad way of checking this not sure yet
            return null;
        }

        if (f.earSize != PersonFeatures.FeatureSize.NONE &&
            f.earSize != features.earSize) {
            return "ears";
        }
        if (f.eyeSize != PersonFeatures.FeatureSize.NONE &&
            f.eyeSize != features.eyeSize) {
            return "eye size";
        }
        if (f.eyeColor != PersonFeatures.FeatureColor.NONE &&
            f.eyeColor != features.eyeColor) {
            return "eye color";
        }
        if (f.noseSize != PersonFeatures.FeatureSize.NONE &&
            f.noseSize != features.noseSize) {
            return "nose";
        }
        if (f.hairColor != PersonFeatures.FeatureColor.NONE &&
            f.hairColor != features.hairColor) {
            return "hair color";
        }
        if (f.glasses != PersonFeatures.FeatureBool.NONE &&
            f.glasses != features.glasses) {
            return "glasses";
        }
        if (f.hat != PersonFeatures.FeatureBool.NONE &&
            f.hat != features.hat) {
            return "hat";
        }
        if (f.facialHair != PersonFeatures.FeatureBool.NONE &&
            f.facialHair != features.facialHair) {
            return "facial hair";
        }

        return null;
    }

    /// <summary>
    /// Sets offByOneFeature to GetOffByOneFeature(b)
    /// </summary>
    /// <param name="b"></param>
    public void SetOffByOneFeature(Basket b) {
        offByOneFeature = GetOffByOneFeature(b);
    }
}
