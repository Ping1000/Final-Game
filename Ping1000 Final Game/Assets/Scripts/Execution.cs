using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Execution : MonoBehaviour
{
    private Person p;

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
        fc.SetBooleanVariable("WasWolf",
            LevelController.GetDailyWolfFeatures().
            NonNoneEquals(Person.activePerson.features));
        fc.ExecuteBlock("On Animation Completed");
    }

    public void DialogFinished() {
        Destroy(p.gameObject);
        GameManager.CreateNewPerson();
        Destroy(gameObject);
    }
}
