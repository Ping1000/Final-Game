using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Basket : MonoBehaviour
{
    [HideInInspector]
    public PersonFeatures basketFeatures;
    public int basketListIdx { set; private get; } // not sure if we'll need yet

    private Flowchart _fc;

    private Person person;

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
        _fc.SetBooleanVariable("PersonFitDescription", DidMeetThreshold());
    }

    public bool DidMeetThreshold() {
        switch (GameManager.featuresToCheckFor) {
            case 0:
                return true;
            case 1:
                return (person.features.eyes == basketFeatures.eyes) ? true :  false;
            case 2:
                if (person.features.hair == basketFeatures.hair)
                    goto case 1;
                else
                    return false;
            case 3:
                if (person.features.ears == basketFeatures.ears)
                    goto case 2;
                else
                    return false;
            default:
                // should be checking for accessories here, should just be a list.contains
                return false;
        }

        // return person.features.NumSharedFeatures(basketFeatures) >= GameManager.correctFeaturesThreshold;
    }

    // mostly used for the flowchart so it can see the method
    public void CreateNewPerson() {
        GameManager.CreateNewPerson();
    }

    public void ReplenishBasket() {
        FindObjectOfType<GameManager>().PlaceBasket(basketListIdx);
    }
}
