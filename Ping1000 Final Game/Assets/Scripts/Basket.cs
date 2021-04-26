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
    /// Used for the Flowchart's control flow
    /// </summary>
    public void UpdateFlowchartBool() {
        _fc.SetBooleanVariable("PersonFitDescription", WasTrueMatch() || WasHiddenMatch());
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

    /// <summary>
    /// Places a new basket
    /// </summary>
    public void ReplenishBasket() {
        FindObjectOfType<GameManager>().PlaceBasket(basketListIdx);
    }
}
