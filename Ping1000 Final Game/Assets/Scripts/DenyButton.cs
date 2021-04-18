using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DenyButton : MonoBehaviour
{
    private Flowchart _fc;
    private Person person;

    private void Start() {
        _fc = GetComponentInChildren<Flowchart>();
    }

    public void UpdatePerson() {
        person = FindObjectOfType<Person>();
        _fc.SetGameObjectVariable("Person", person.gameObject);
    }

    public void UpdatePerson(Person p) {
        person = p;
        _fc.SetGameObjectVariable("Person", p.gameObject);
    }

    // mostly used for the flowchart so it can see the method
    public void CreateNewPerson() {
        GameManager.CreateNewPerson();
    }

    public void UpdateFlowchartBool() {
        _fc.SetBooleanVariable("WasAble", DidMeetThreshold());
    }

    private bool DidMeetThreshold() {
        foreach (Basket b in FindObjectsOfType<Basket>()) {
            b.UpdatePerson();
            if (b.DidMeetThreshold())
                return true;
        }
        return false;
    }
}
