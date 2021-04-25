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
        // more bad spaghetti
        if (basketFeatures.earSize != PersonFeatures.FeatureSize.NONE &&
            basketFeatures.earSize != person.features.earSize)
            return false;

        if (basketFeatures.eyeSize != PersonFeatures.FeatureSize.NONE &&
            basketFeatures.eyeSize != person.features.eyeSize)
            return false;

        if (basketFeatures.eyeColor != PersonFeatures.FeatureColor.NONE &&
            basketFeatures.eyeColor != person.features.eyeColor)
            return false;

        if (basketFeatures.noseSize != PersonFeatures.FeatureSize.NONE &&
            basketFeatures.noseSize != person.features.noseSize)
            return false;

        if (basketFeatures.hairColor != PersonFeatures.FeatureColor.NONE &&
            basketFeatures.hairColor != person.features.hairColor)
            return false;

        if (basketFeatures.glasses != PersonFeatures.FeatureBool.NONE &&
            basketFeatures.glasses != person.features.glasses)
            return false;

        if (basketFeatures.hat != PersonFeatures.FeatureBool.NONE &&
            basketFeatures.hat != person.features.hat)
            return false;

        if (basketFeatures.facialHair != PersonFeatures.FeatureBool.NONE &&
            basketFeatures.facialHair != person.features.facialHair)
            return false;

        return true;
    }

    // mostly used for the flowchart so it can see the method
    public void CreateNewPerson() {
        GameManager.CreateNewPerson();
    }

    public void ReplenishBasket() {
        FindObjectOfType<GameManager>().PlaceBasket(basketListIdx);
    }
}
