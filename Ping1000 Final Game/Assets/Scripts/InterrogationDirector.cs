using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class InterrogationDirector : MonoBehaviour {
    private Flowchart _fc;

    // Start is called before the first frame update
    void Start() {
        _fc = GetComponent<Flowchart>();
    }

    private string PlayerDialog { get {
            if (_fc == null)
                return null;
            return _fc.GetStringVariable("player_dialog");
        } set {
            _fc.SetStringVariable("player_dialog", value);
        }
    }

    private string ResponseDialog { get {
            if (_fc == null)
                return null;
            return _fc.GetStringVariable("response_dialog");
        } set {
            _fc.SetStringVariable("response_dialog", value);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnInterrogationStarted() {
        Person p = FindObjectOfType<Person>();
        /*
         * pseudocode:
         * switch (p.type):
         *   case true_match:
         *     responsedialog = random true match response
         *   case hidden_match:
         *     responsedialog = genuine excuse based on the feature that is wrong
         *   case non_match:
         *     responsedialog is either
         *       random non_match response OR
         *       excuse based on the feature that is different but it's the wrong feature
         */
    }



    private readonly string[] true_match_responses =
        { "Who do you think I look like? The big bad wolf? {wc}Are you blind? Come on buddy, take another look at those baskets of yours. {wc}Let's get a move on, I don't have all day.",
          "Are you certain? I know I placed an order. {wc}Maybe try checking your baskets again? I assure you I'm on there. {wc}Please do hurry, I've got to run home.",
          "Is that so? Well let me tell ya, I know I've got an order there. {wc}Maybe open your eyes and take another look at those baskets, pal. {wc}Now stop wasting my time and let's get moving."};

    // use these or a dummy feature-based response where they're like
    // "i just got my hair dyed" but it's their eyes that was wrong
    private readonly string[] non_match_responses =
        { "Uh... are you sure? {wc}I swear I placed an order ahead of time.",
          "Please, you must help me. I need these goodies!",
          "Listen... I know you've got quotas to meet... Why don't you just hook me up and we both benefit?"};
}
