using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int peopleBeforeGuaranteed;
    private int peopleSpawned = 0;
    public static int featuresToCheckFor;

    public List<Transform> basketPositions;
    public List<PersonFeatures> basketFeatures;

    public static GameManager instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        basketFeatures = new List<PersonFeatures>();
        GenerateBasketList();
        SpawnBaskets();
        peopleBeforeGuaranteed = 3;
        featuresToCheckFor = 2;

        CreateNewPerson();
    }

    private void GenerateBasketList() {
        foreach (Transform t in basketPositions) {
            basketFeatures.Add(new PersonFeatures());
        }
    }

    private void SpawnBaskets() {
        for (int i = 0; i < basketPositions.Count; i++) {
            PlaceBasket(i);
        }
    }

    public void PlaceBasket(int idx) {
        GameObject basketObj = Instantiate(Resources.Load<GameObject>("Basket"));
        basketObj.transform.position = basketPositions[idx].position;
        Basket basket = basketObj.GetComponent<Basket>();
        basket.basketFeatures = basketFeatures[idx];
        basket.basketListIdx = idx;
    }

    /// <summary>
    /// Generates features that always match one of the baskets.
    /// </summary>
    /// <returns>A PersonFeatures object that should match with one of the baskets.</returns>
    public static PersonFeatures GenerateMatchingFeatures() {
        PersonFeatures randomP = instance.basketFeatures[Random.Range(0, instance.basketFeatures.Count)];
        PersonFeatures res = new PersonFeatures();

        switch (featuresToCheckFor) {
            case 0:
                // already randomized
                break;
            case 1:
                res.eyes = randomP.eyes;
                break;
            case 2:
                res.hair = randomP.hair;
                goto case 1;
            case 3:
                res.ears = randomP.ears;
                goto case 2;
            default:
                // accessories not implemented
                goto case 3;
        }

        return res;
    }

    public static void CreateNewPerson() {
        GameObject[] persons = Resources.LoadAll<GameObject>("Persons");
        // randomly pick person body for now
        GameObject personObj = Instantiate(persons[Random.Range(0, persons.Length)]);
        Person person = personObj.GetComponent<Person>();

        instance.peopleSpawned++;
        if (instance.peopleSpawned % peopleBeforeGuaranteed == 0) {
            person.useRandom = false;
        }



        personObj.transform.position = new Vector3(15, 2, 0);
        LeanTween.move(personObj, new Vector3(0, 2, 0), 2f);
    }
}
