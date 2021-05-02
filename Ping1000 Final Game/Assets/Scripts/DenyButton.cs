using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

/// <summary>
/// Script for the "deny" button
/// </summary>
public class DenyButton : MonoBehaviour
{
    /// <summary>
    /// Flowchart attached to this GameObject
    /// </summary>
    private Flowchart _fc;
    private Person person;

    private void Start() {
        _fc = GetComponentInChildren<Flowchart>();
    }

    // mostly used for the flowchart so it can see the method
    public void CreateNewPerson() {
        GameManager.CreateNewPerson();
    }

    // used for the flowchart
    public void UpdatePerson() {
        UpdatePerson(FindObjectOfType<Person>());
    }

    public void UpdatePerson(Person p) {
        person = p;
        _fc.SetGameObjectVariable("Person", p.gameObject);
    }

    // Used for the Flowchart's control flow
    public void UpdateFlowchartBool() {
        _fc.SetBooleanVariable("WasAble", DidMeetThreshold());
        person.GetComponent<Collider2D>().enabled = false;
    }

    /// <summary>
    /// Returns true if the active person is a true or hidden match
    /// </summary>
    /// <returns></returns>
    private bool DidMeetThreshold() {
        foreach (Basket b in FindObjectsOfType<Basket>()) {
            b.UpdatePerson();
            if (b.WasHiddenMatch() || b.WasTrueMatch())
                return true;
        }
        return false;
    }
}
