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
        CreateNewPerson();

        basketFeatures = new List<PersonFeatures>();
        GenerateBasketList();
        SpawnBaskets();
        peopleBeforeGuaranteed = 3;
        featuresToCheckFor = 1;
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
        // Basket b = instance.baskets
        return null;
    }

    public static void CreateNewPerson() {
        GameObject[] persons = Resources.LoadAll<GameObject>("Persons");
        // randomly pick person body for now
        GameObject person = Instantiate(persons[Random.Range(0, persons.Length)]);

        //instance.peopleSpawned++;
        //if (instance.peopleSpawned == peopleBeforeGuaranteed) {
        //    person.GetComponent<Person>().features = GenerateMatchingFeatures();
        //}
        //instance.peopleSpawned %= peopleBeforeGuaranteed;

        person.transform.position = new Vector3(15, 2, 0);
        LeanTween.move(person, new Vector3(0, 2, 0), 2f);
    }
}
