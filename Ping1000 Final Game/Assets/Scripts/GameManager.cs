using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool toggleHiddenMatch;
    public static int guaranteedMatchingPeople; // matching people before a non-matcher
    private int peopleSpawned = 0;
    // [Tooltip("Non-NONE fields will be checked.")]
    // public PersonFeatures featuresToCheckFor;

    public List<GameObject> basketPrefabs;

    public static GameManager instance;
    private List<GameObject> hiddenMatchers;
    private List<GameObject> nonMatchers;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        // basketFeatures = new List<PersonFeatures>();
        // GenerateBasketList();
        DisableBaskets();
        SpawnBaskets();
        GenerateNonMatchers();
        GenerateHiddenMatchers();
        Debug.Log(hiddenMatchers.Count);
        guaranteedMatchingPeople = 3;

        toggleHiddenMatch = true;
        CreateNewPerson();
    }

    private void GenerateNonMatchers() {
        nonMatchers = new List<GameObject>(Resources.LoadAll<GameObject>("Persons/"));
        int i = 0;
        while (i < nonMatchers.Count) {
            GameObject go = nonMatchers[i];
            PersonFeatures pf = go.GetComponent<Person>().features;
            bool wasDeleted = false;
            foreach (GameObject basket_go in basketPrefabs) {
                Basket b = basket_go.GetComponent<Basket>();
                foreach (GameObject match_go in b.trueMatches) {
                    if (pf.NonNoneEquals(match_go.GetComponent<Person>().features)) {
                        wasDeleted = true;
                        nonMatchers.Remove(go);
                        break;
                    }
                }
                if (wasDeleted)
                    break;
                foreach (GameObject match_go in b.hiddenMatches) {
                    if (pf.NonNoneEquals(match_go.GetComponent<Person>().features)) {
                        wasDeleted = true;
                        nonMatchers.Remove(go);
                        break;
                    }
                }
                if (wasDeleted)
                    break;
            }
            if (!wasDeleted)
                i++;
        }
    }

    private void GenerateHiddenMatchers() {
        HashSet<GameObject> hiddenSet = new HashSet<GameObject>();

        foreach (GameObject basket_go in basketPrefabs) {
            Basket b = basket_go.GetComponent<Basket>();
            foreach (GameObject hidden_go in b.hiddenMatches) {
                hiddenSet.Add(hidden_go);
            }
        }

        foreach (GameObject basket_go in basketPrefabs) {
            Basket b = basket_go.GetComponent<Basket>();
            foreach (GameObject match_go in b.trueMatches) {
                // will this work?
                if (hiddenSet.Contains(match_go)) {
                    hiddenSet.Remove(match_go);
                }
            }
        }

        hiddenMatchers = new List<GameObject>(hiddenSet);
    }

    private GameObject SelectHiddenMatchingPerson() {
        return hiddenMatchers[Random.Range(0, hiddenMatchers.Count)];
    }

    //private void GenerateBasketList() {
    //    bool checkEarSize = featuresToCheckFor.earSize != PersonFeatures.FeatureSize.NONE;
    //    bool checkEyeSize = featuresToCheckFor.eyeSize != PersonFeatures.FeatureSize.NONE;
    //    bool checkEyeColor = featuresToCheckFor.eyeColor != PersonFeatures.FeatureColor.NONE;
    //    bool checkNoseSize = featuresToCheckFor.noseSize != PersonFeatures.FeatureSize.NONE;
    //    bool checkHairColor = featuresToCheckFor.hairColor != PersonFeatures.FeatureColor.NONE;
    //    bool checkGlasses = featuresToCheckFor.glasses != PersonFeatures.FeatureBool.NONE;
    //    bool checkHat = featuresToCheckFor.hat != PersonFeatures.FeatureBool.NONE;
    //    bool checkFacialHair = featuresToCheckFor.facialHair != PersonFeatures.FeatureBool.NONE;

    //    foreach (Transform t in basketPositions) {
    //        PersonFeatures p = new PersonFeatures(PersonFeatures.FeatureSize.NONE);

    //        if (checkEarSize)
    //            p.earSize = PersonFeatures.RandomFeatureSize();
    //        if (checkEyeSize)
    //            p.eyeSize = PersonFeatures.RandomFeatureSize();
    //        if (checkEyeColor)
    //            p.eyeColor = PersonFeatures.RandomFeatureColor();
    //        if (checkNoseSize)
    //            p.noseSize = PersonFeatures.RandomFeatureSize();
    //        if (checkHairColor)
    //            p.hairColor = PersonFeatures.RandomFeatureColor();
    //        if (checkGlasses)
    //            p.glasses = PersonFeatures.RandomFeatureBool();
    //        if (checkHat)
    //            p.hat = PersonFeatures.RandomFeatureBool();
    //        if (checkFacialHair)
    //            p.facialHair = PersonFeatures.RandomFeatureBool();

    //        basketFeatures.Add(p);
    //    }
    //}

    private void DisableBaskets() {
        foreach (GameObject go in basketPrefabs) {
            go.SetActive(false);
        }
    }

    private void SpawnBaskets() {
        for (int i = 0; i < basketPrefabs.Count; i++) {
            PlaceBasket(i);
        }
    }

    public void PlaceBasket(int idx) {
        GameObject basketObj = Instantiate(basketPrefabs[idx]);
        basketObj.SetActive(true);
        Basket basket = basketObj.GetComponent<Basket>();
        basket.basketListIdx = idx;
    }

    /// <summary>
    /// [no longer supported] Generates features that always match one of the baskets.
    /// </summary>
    /// <returns>A PersonFeatures object that should match with one of the baskets.</returns>
    public static PersonFeatures GenerateMatchingFeatures() {
        PersonFeatures basketP = instance.basketPrefabs[Random.Range(0, instance.basketPrefabs.Count)].
            GetComponent<Basket>().basketFeatures;
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

    private GameObject SelectMatchingPerson() {
        int basketIdx = Random.Range(0, basketPrefabs.Count);
        List<GameObject> matchingPeople = basketPrefabs[basketIdx].
            GetComponent<Basket>().trueMatches;
        int personIdx = Random.Range(0, matchingPeople.Count);

        return matchingPeople[personIdx];
    }

    private GameObject SelectNonmatchingPerson() {
        return nonMatchers[Random.Range(0, nonMatchers.Count)];
    }

    public static void CreateNewPerson() {
        //GameObject[] persons = Resources.LoadAll<GameObject>("Persons");
        //// randomly pick person body for now
        //GameObject personObj = Instantiate(persons[Random.Range(0, persons.Length)]);
        //Person person = personObj.GetComponent<Person>();
        GameObject personObj;

        instance.peopleSpawned++;
        if (instance.peopleSpawned % guaranteedMatchingPeople == 0) {
            // spawn either off by one or non-matchers
            // person.useRandom = true;
            if (instance.toggleHiddenMatch) {
                personObj = Instantiate(instance.SelectHiddenMatchingPerson());
            } else {
                personObj = Instantiate(instance.SelectNonmatchingPerson()); // FOR NOW
            }
            instance.toggleHiddenMatch = !instance.toggleHiddenMatch;
        } else {
            // person.useRandom = false;
            personObj = Instantiate(instance.SelectMatchingPerson());
        }

        personObj.transform.position = new Vector3(15, 2, 0);
        LeanTween.move(personObj, new Vector3(0, 2, 0), 2f);
    }
}
