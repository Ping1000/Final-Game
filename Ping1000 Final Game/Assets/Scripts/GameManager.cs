using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int guaranteedMatchingPeople; // matching people before a non-matcher
    private int peopleSpawned = 0;
    [Tooltip("Non-NONE fields will be checked.")]
    public PersonFeatures featuresToCheckFor;

    public List<Transform> basketPositions;
    [HideInInspector]
    public List<PersonFeatures> basketFeatures;

    public static GameManager instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        basketFeatures = new List<PersonFeatures>();
        GenerateBasketList();
        SpawnBaskets();
        guaranteedMatchingPeople = 3;

        CreateNewPerson();
    }

    private void GenerateBasketList() {
        bool checkEarSize = featuresToCheckFor.earSize != PersonFeatures.FeatureSize.NONE;
        bool checkEyeSize = featuresToCheckFor.eyeSize != PersonFeatures.FeatureSize.NONE;
        bool checkEyeColor = featuresToCheckFor.eyeColor != PersonFeatures.FeatureColor.NONE;
        bool checkNoseSize = featuresToCheckFor.noseSize != PersonFeatures.FeatureSize.NONE;
        bool checkHairColor = featuresToCheckFor.hairColor != PersonFeatures.FeatureColor.NONE;
        bool checkGlasses = featuresToCheckFor.glasses != PersonFeatures.FeatureBool.NONE;
        bool checkHat = featuresToCheckFor.hat != PersonFeatures.FeatureBool.NONE;
        bool checkFacialHair = featuresToCheckFor.facialHair != PersonFeatures.FeatureBool.NONE;

        foreach (Transform t in basketPositions) {
            PersonFeatures p = new PersonFeatures(PersonFeatures.FeatureSize.NONE);

            if (checkEarSize)
                p.earSize = PersonFeatures.RandomFeatureSize();
            if (checkEyeSize)
                p.eyeSize = PersonFeatures.RandomFeatureSize();
            if (checkEyeColor)
                p.eyeColor = PersonFeatures.RandomFeatureColor();
            if (checkNoseSize)
                p.noseSize = PersonFeatures.RandomFeatureSize();
            if (checkHairColor)
                p.hairColor = PersonFeatures.RandomFeatureColor();
            if (checkGlasses)
                p.glasses = PersonFeatures.RandomFeatureBool();
            if (checkHat)
                p.hat = PersonFeatures.RandomFeatureBool();
            if (checkFacialHair)
                p.facialHair = PersonFeatures.RandomFeatureBool();

            basketFeatures.Add(p);
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
        PersonFeatures basketP = instance.basketFeatures[Random.Range(0, instance.basketFeatures.Count)];
        PersonFeatures res = new PersonFeatures();

        if (basketP.earSize != PersonFeatures.FeatureSize.NONE)
            res.earSize = basketP.earSize;
        if (basketP.eyeSize != PersonFeatures.FeatureSize.NONE)
            res.eyeSize = basketP.eyeSize;
        if (basketP.eyeColor != PersonFeatures.FeatureColor.NONE)
            res.eyeColor = basketP.eyeColor;
        if (basketP.noseSize != PersonFeatures.FeatureSize.NONE)
            res.noseSize = basketP.noseSize;
        if (basketP.hairColor != PersonFeatures.FeatureColor.NONE)
            res.hairColor = basketP.hairColor;
        if (basketP.glasses != PersonFeatures.FeatureBool.NONE)
            res.glasses = basketP.glasses;
        if (basketP.hat != PersonFeatures.FeatureBool.NONE)
            res.hat = basketP.hat;
        if (basketP.facialHair != PersonFeatures.FeatureBool.NONE)
            res.facialHair = basketP.facialHair;

        return res;
    }

    public static void CreateNewPerson() {
        GameObject[] persons = Resources.LoadAll<GameObject>("Persons");
        // randomly pick person body for now
        GameObject personObj = Instantiate(persons[Random.Range(0, persons.Length)]);
        Person person = personObj.GetComponent<Person>();

        instance.peopleSpawned++;
        if (instance.peopleSpawned % guaranteedMatchingPeople == 0) {
            person.useRandom = true;
        } else {
            person.useRandom = false;
        }



        personObj.transform.position = new Vector3(15, 2, 0);
        LeanTween.move(personObj, new Vector3(0, 2, 0), 2f);
    }
}
