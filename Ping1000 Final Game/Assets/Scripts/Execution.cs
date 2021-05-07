using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Execution : MonoBehaviour
{
    private Person p;
    private bool wasWolf = false;

    // Start is called before the first frame update
    void Start()
    {
        p = FindObjectOfType<Person>();
        p.GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = p.transform.position;
    }

    public void OnAnimationCompleted() {
        Flowchart fc = GetComponentInChildren<Flowchart>();
        bool wasWolf = LevelController.GetDailyWolfFeatures().
            NonNoneEquals(Person.activePerson.features);
        fc.SetBooleanVariable("WasWolf", wasWolf);
        this.wasWolf = wasWolf;
        fc.ExecuteBlock("On Animation Completed");
    }

    public void DialogFinished() {
        // TODO Need to check if p was innocent villager or not to give time bonus
        Destroy(p.gameObject);
        if (wasWolf) {
            Timer.instance.daySeconds += 60;
            Timer.instance.SetTimerUI();
        }
        ExecuteButton.canExecute = true;
        GameManager.CreateNewPerson();
        Destroy(gameObject);
    }
}
