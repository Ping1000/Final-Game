using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

/// <summary>
/// Attached to the Character flowcharts and drives the interrogations
/// </summary>
public class InterrogationDirector : MonoBehaviour {
    private Flowchart _fc;

    // Start is called before the first frame update
    void Start() {
        _fc = GetComponent<Flowchart>();
    }

    // flowchart variable
    private string PlayerDialog { get {
            if (_fc == null)
                return null;
            return _fc.GetStringVariable("player_dialog");
        } set {
            _fc.SetStringVariable("player_dialog", value);
        }
    }

    // flowchart variable
    private string ResponseDialog { get {
            if (_fc == null)
                return null;
            return _fc.GetStringVariable("response_dialog");
        } set {
            _fc.SetStringVariable("response_dialog", value);
        }
    }

    // Called by the flowchart when the interrogation was started
    public void OnInterrogationStarted() {
        Person p = transform.GetComponentInParent<Person>();
        Basket offBasket = null;

        // determine if the person was a true or hidden match
        foreach (Basket b in FindObjectsOfType<Basket>()) {
            b.UpdatePerson();
            // if true match
            if (b.WasTrueMatch()) {
                PlayerDialog = "Care to explain some of these discrepancies in your description?"; // temp
                ResponseDialog = true_match_responses[Random.Range(0,
                    true_match_responses.Length)];
                _fc.ExecuteBlock("Play Interrogation Dialog");
                return;
            } else if (b.WasHiddenMatch()) {
                // if hidden match, need to keep looping in case true match
                // for some other basket
                p.SetOffByOneFeature(b);
                offBasket = b;
            }
        }

        if (p.offByOneFeature != null && p.offByOneFeature != "") {
            // hidden match
            PlayerDialog = "Care to explain some of these discrepancies in your description? {wi}" +
                "We're specifically looking at an issue with your " + p.offByOneFeature;
            ResponseDialog = GetGenuineExcuse(p.offByOneFeature);
            ResponseDialog += GetSpecificExcuse(p.offByOneFeature, offBasket);
        } else {
            // non-matcher
            // for now, just dumb excuses and not feature-specific
            PlayerDialog = "Care to explain some of these discrepancies in your description? {wi}" +
                "We've found several inconsistencies.";
            ResponseDialog = non_match_responses[Random.Range(0, non_match_responses.Length)];
        }
        _fc.ExecuteBlock("Play Interrogation Dialog");
    }

    /// <summary>
    /// Returns an additional excuse that is specific to  
    /// </summary>
    /// <param name="featureStr">The feature that is off</param>
    /// <param name="b">The basket containing the true features</param>
    /// <returns></returns>
    private string GetSpecificExcuse(string featureStr, Basket b) {
        if (b == null)
            return "";
        string res = "";

        PersonFeatures pf = b.basketFeatures;

        switch (featureStr) {
            case "ears":
                res = "{wc}My ears are normally " + pf.FeatureSizeToString(pf.earSize) + ".";
                break;
            case "eye size":
                res = "{wc}My eyes are normally " + pf.FeatureSizeToString(pf.eyeSize) + ".";
                break;
            case "eye color":
                res = "{wc}My eyes are normally " + pf.FeatureColorToString(pf.eyeColor) + ".";
                break;
            case "nose":
                res = "{wc}My nose is normally " + pf.FeatureSizeToString(pf.noseSize) + ".";
                break;
            case "hair color":
                res = "{wc}My hair is normally " + pf.FeatureColorToString(pf.hairColor) + ".";
                break;
            case "glasses":
                if (pf.glasses == PersonFeatures.FeatureBool.yes)
                    res = "{wc}I normally wear glasses.";
                else
                    res = "{wc}I don't normally wear glasses.";
                break;
            case "hat":
                if (pf.hat == PersonFeatures.FeatureBool.yes)
                    res = "{wc}I was wearing a hat when they identified me.";
                else
                    res = "{wc}I wasn't wearing this hat when they identified me.";
                break;
            case "facial hair":
                if (pf.facialHair == PersonFeatures.FeatureBool.yes)
                    res = "{wc}I shaved after they identified me.";
                else
                    res = "{wc}I grew out my facial hair.";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// Returns one of the genuine excuses for having a different feature
    /// </summary>
    /// <param name="featureStr"></param>
    /// <returns></returns>
    private string GetGenuineExcuse(string featureStr) {
        switch (featureStr) {
            case "ears":
                return ear_size_responses[Random.Range(0, ear_size_responses.Length)];
            case "eye size":
                return eye_size_responses[Random.Range(0, eye_size_responses.Length)];
            case "eye color":
                return eye_color_responses[Random.Range(0, eye_color_responses.Length)];
            case "nose":
                return nose_size_responses[Random.Range(0, nose_size_responses.Length)];
            case "hair color":
                return hair_color_responses[Random.Range(0, hair_color_responses.Length)];
            case "glasses":
                return glasses_responses[Random.Range(0, glasses_responses.Length)];
            case "hat":
                return hat_responses[Random.Range(0, hat_responses.Length)];
            case "facial hair":
                return facial_hair_responses[Random.Range(0, facial_hair_responses.Length)];
            default:
                return "Unsupported feature string: " + featureStr;
        }
    }

    private readonly string[] true_match_responses = {
        "Who do you think I look like? The big bad wolf? {wc}Are you blind? Come on buddy, take another look at those baskets of yours. {wc}Let's get a move on, I don't have all day.",
        "Are you certain? I know I placed an order. {wc}Maybe try checking your baskets again? I assure you I'm on there. {wc}Please do hurry, I've got to run home.",
        "Is that so? Well let me tell ya, I know I've got an order there. {wc}Maybe open your eyes and take another look at those baskets, pal. {wc}Now stop wasting my time and let's get moving."
    };

    private readonly string[] ear_size_responses = {
        "Oh, I've got this ear infection and now my ears are all messed up."
    };
    private readonly string[] eye_size_responses = {
        "I just had an operation done last night. 'Eye' can see much clearer now!"
    };
    private readonly string[] eye_color_responses = {
        "That's because I'm wearing my colored contacts today!"
    };
    private readonly string[] nose_size_responses = {
        "I wasn't happy with how my nose looked, and I just had surgery to get it reshaped."
    };
    private readonly string[] hair_color_responses = {
        "I just came from the salon! {wi}Do you like my new color?"
    };
    private readonly string[] glasses_responses = {
        "Well, it's not like I wear glasses every day... Who would?"
    };
    private readonly string[] hat_responses = {
        "Why are hats something you look for? Can't you tell that it's me?",
        "I wanted to change my look. Besides, wearing a hat every will make you go bald!"
    };
    private readonly string[] facial_hair_responses = {
        "I shaved!"
    };

    // use these or a dummy feature-based response where they're like
    // "i just got my hair dyed" but it's their eyes that was wrong
    private readonly string[] non_match_responses =
        { "Uh... are you sure? {wc}I swear I placed an order ahead of time.",
          "Please, you must help me. I need these goodies!",
          "Listen... I know you've got quotas to meet... Why don't you just hook me up and we both benefit?"};
}
