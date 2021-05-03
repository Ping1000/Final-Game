using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

/// <summary>
/// Class for controlling the basket objects
/// </summary>
public class Basket : MonoBehaviour
{
    // The features sought after by this basket
    public PersonFeatures basketFeatures;
    public int basketListIdx { set; private get; }

    /// <summary>
    /// The flowchart object attached to this basket
    /// </summary>
    private Flowchart _fc;

    /// <summary>
    /// The active person in the scene. Could maybe change to static tbh
    /// </summary>
    private Person person;

    // These are hard-coded in the inspector for now; ideally the wouldn't be
    public List<GameObject> trueMatches;
    public List<GameObject> hiddenMatches;
    public List<GameObject> nonMatches;

    // Start is called before the first frame update
    void Start()
    {
        _fc = GetComponentInChildren<Flowchart>();
        _fc.SetStringVariable("BasketDescription", basketFeatures.ToString());
        
        // UpdatePerson();
    }

    /// <summary>
    /// Updates person to the active Person GameObject in the scene, and
    /// updates info in the Flowchart
    /// </summary>
    public void UpdatePerson() {
        UpdatePerson(FindObjectOfType<Person>());
    }

    /// <summary>
    /// Updates person to p, and updates info in the flowchart
    /// </summary>
    /// <param name="p"></param>
    public void UpdatePerson(Person p) {
        person = p;
        _fc.SetGameObjectVariable("Person", p.gameObject);

        DragCompleted dg = _fc.GetComponent<DragCompleted>();
        Collider2D col = person.GetComponent<Collider2D>();

        dg.targetObjects = new List<Collider2D>();
        dg.targetObjects.Add(col);
    }

    /// <summary>
    /// Determines if a person met this basket's description. Used for the
    /// Flowchart's control flow and to determine if we need to increase
    /// correct baskets in GameManager
    /// </summary>
    public void UpdateFlowchartBool() {
        bool fitDesc = WasTrueMatch() || WasHiddenMatch();
        bool wasWolf = LevelController.GetDailyWolfFeatures().
            NonNoneEquals(person.features);
        GameManager.instance.CorrectBaskets += (fitDesc && (!wasWolf) ? 1 : 0);
        person.GetComponent<Collider2D>().enabled = false;
        _fc.SetBooleanVariable("PersonFitDescription", fitDesc);
        _fc.SetBooleanVariable("WasWolf", wasWolf);
    }

    // Used for the Flowchart's control flow
    public bool WasTrueMatch() {
        foreach (GameObject go in trueMatches) {
            if (go.GetComponent<Person>().features.NonNoneEquals(person.features))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if a person was a "hidden match"
    /// </summary>
    /// <returns></returns>
    public bool WasHiddenMatch() {
        foreach (GameObject go in hiddenMatches) {
            if (go.GetComponent<Person>().features.NonNoneEquals(person.features))
                return true;
        }
        return false;
    }

    // mostly used for the flowchart so it can see the method
    public void CreateNewPerson() {
        GameManager.CreateNewPerson();
    }

    // used by the flowchart
    public void GaveToWolf() {
        LevelController.instance.winObj.ShowWolfScene();
    }

    // used by the flowchart
    public void IncreaseQuota(int amt) {
        GameManager.instance.IncreaseQuota(amt);
    }

    /// <summary>
    /// Places a new basket
    /// </summary>
    public void ReplenishBasket() {
        FindObjectOfType<GameManager>().PlaceBasket(basketListIdx);
    }

    public void SetMatchesLists() {
        foreach (GameObject person_go in Resources.LoadAll<GameObject>("Persons/")) {
            PersonFeatures pf = person_go.GetComponent<Person>().features;

            int diff = basketFeatures.NonNoneFeatureDiff(pf);
            switch (diff) {
                case 0:
                    trueMatches.Add(person_go);
                    break;
                case 1:
                    hiddenMatches.Add(person_go);
                    break;
                default:
                    nonMatches.Add(person_go);
                    break;
            }
        }
    }
}
