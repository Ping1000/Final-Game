using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Basket : MonoBehaviour
{
    public PersonFeatures basketFeatures;
    public int basketListIdx { set; private get; } // not sure if we'll need yet

    private Flowchart _fc;

    private Person person;

    public List<GameObject> trueMatches;
    public List<GameObject> hiddenMatches;
    public List<GameObject> nonMatches;

    // Start is called before the first frame update
    void Start()
    {
        _fc = GetComponentInChildren<Flowchart>();
        _fc.SetStringVariable("BasketDescription", basketFeatures.ToString());
        
        UpdatePerson();
    }

    public void UpdatePerson() {
        person = FindObjectOfType<Person>();
        _fc.SetGameObjectVariable("Person", person.gameObject);

        DragCompleted dg = _fc.GetComponent<DragCompleted>();
        Collider2D col = person.GetComponent<Collider2D>();

        dg.targetObjects = new List<Collider2D>();
        dg.targetObjects.Add(col);
    }

    public void UpdatePerson(Person p) {
        person = p;
        _fc.SetGameObjectVariable("Person", p.gameObject);

        DragCompleted dg = _fc.GetComponent<DragCompleted>();
        Collider2D col = person.GetComponent<Collider2D>();

        dg.targetObjects = new List<Collider2D>();
        dg.targetObjects.Add(col);
    }

    public void UpdateFlowchartBool() {
        _fc.SetBooleanVariable("PersonFitDescription", WasTrueMatch() || WasHiddenMatch());
    }

    public bool WasTrueMatch() {
        foreach (GameObject go in trueMatches) {
            if (go.GetComponent<Person>().features.NonNoneEquals(person.features))
                return true;
        }
        return false;
    }

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

    public void ReplenishBasket() {
        FindObjectOfType<GameManager>().PlaceBasket(basketListIdx);
    }
}
