using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool toggleHiddenMatch; // yes ==> hidden match, no ==> non match
    public static int guaranteedMatchingPeople; // matching people before a non-matcher
    private int peopleSpawned = 0;

    // List of basket objects in the scene
    public List<GameObject> basketPrefabs;

    /// <summary>
    /// List of the people who are hidden matchers
    /// </summary>
    private List<GameObject> hiddenMatchers;
    /// <summary>
    /// List of the people who are non-matchers
    /// </summary>
    private List<GameObject> nonMatchers;

    public static GameManager instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        DisableBaskets(); // make baskets inactive
        SpawnBaskets(); // spawn clones of the (now inactive) baskets
        GenerateNonMatchers(); // build up the list of non-matching people
        GenerateHiddenMatchers(); // build up the list of hidden matching people
        guaranteedMatchingPeople = 3; // we can change

        toggleHiddenMatch = true; // we can change, this system is basic anyways
        CreateNewPerson(); // spawn the first NPC
    }

    /// <summary>
    /// Sets nonMatchers to contain all of the prefabs that are non-matchers
    /// </summary>
    private void GenerateNonMatchers() {
        // start with all possible people, then remove if they are a true or
        // hidden match
        nonMatchers = new List<GameObject>(Resources.LoadAll<GameObject>("Persons/"));

        int i = 0;
        while (i < nonMatchers.Count) {
            GameObject go = nonMatchers[i];
            PersonFeatures pf = go.GetComponent<Person>().features;
            bool wasDeleted = false;
            foreach (GameObject basket_go in basketPrefabs) {
                Basket b = basket_go.GetComponent<Basket>();
                // remove person if true match
                foreach (GameObject match_go in b.trueMatches) {
                    if (pf.NonNoneEquals(match_go.GetComponent<Person>().features)) {
                        wasDeleted = true;
                        nonMatchers.Remove(go);
                        break;
                    }
                }
                if (wasDeleted)
                    break;
                // remove person if hidden match
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

    /// <summary>
    /// Sets hiddenMatchesrs to contain all of the prefabs that are non-matchers
    /// </summary>
    private void GenerateHiddenMatchers() {
        // start with each basket's hiddenMatches, then remove true matchers
        HashSet<GameObject> hiddenSet = new HashSet<GameObject>();

        // add all people in hiddenMatches
        foreach (GameObject basket_go in basketPrefabs) {
            Basket b = basket_go.GetComponent<Basket>();
            foreach (GameObject hidden_go in b.hiddenMatches) {
                hiddenSet.Add(hidden_go);
            }
        }

        // remove people who appeared in other baskets' true matches
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

    /// <summary>
    /// Returns a refernce to one of a random person from hiddenMatchers
    /// </summary>
    /// <returns>A reference to a random person prefab in hiddenMatchers</returns>
    private GameObject SelectHiddenMatchingPerson() {
        return hiddenMatchers[Random.Range(0, hiddenMatchers.Count)];
    }

    /// <summary>
    /// Set the basket prefabs to inactive. Used at the start
    /// </summary>
    private void DisableBaskets() {
        foreach (GameObject go in basketPrefabs) {
            go.SetActive(false);
        }
    }

    /// <summary>
    /// Spawn in the set of baskets. Used at the start
    /// </summary>
    private void SpawnBaskets() {
        for (int i = 0; i < basketPrefabs.Count; i++) {
            PlaceBasket(i);
        }
    }

    /// <summary>
    /// Spawns a clone of basketPrefabs[idx]
    /// </summary>
    /// <param name="idx"></param>
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

    /// <summary>
    /// Returns a refernce to one of a random person from trueMatchers
    /// </summary>
    /// <returns>A reference to a random person prefab in trueMatchers</returns>
    private GameObject SelectMatchingPerson() {
        int basketIdx = Random.Range(0, basketPrefabs.Count);
        List<GameObject> matchingPeople = basketPrefabs[basketIdx].
            GetComponent<Basket>().trueMatches;
        int personIdx = Random.Range(0, matchingPeople.Count);

        return matchingPeople[personIdx];
    }

    /// <summary>
    /// Returns a refernce to one of a random person from nonMatchers
    /// </summary>
    /// <returns>A reference to a random person prefab in nonMatchers</returns>
    private GameObject SelectNonmatchingPerson() {
        return nonMatchers[Random.Range(0, nonMatchers.Count)];
    }

    /// <summary>
    /// Instantiate a new person
    /// </summary>
    public static void CreateNewPerson() {
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
