using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    // private bool toggleHiddenMatch; // yes ==> hidden match, no ==> non match
    public static int guaranteedMatchingPeople; // matching people before a non-matcher
    public static int peopleBeforeWolf;
    private int peopleSpawned = 0;

    private PersonFeatures featuresToCheckFor;
    // List of basket objects in the scene
    public List<GameObject> basketPrefabs;

    public QuotaUI quotaUI;
    public LevelController levelController;
    public Image refreshPanel;
    [HideInInspector]
    public int quota;

    private int _correctBaskets;
    [HideInInspector]
    public int CorrectBaskets { get { return _correctBaskets; } set {
            _correctBaskets = value;
            quotaUI.UpdateQuotaText();
            if (_correctBaskets >= quota && OnQuotaMet != null)
                OnQuotaMet();
        } }

    /// <summary>
    /// List of the people who are hidden matchers
    /// </summary>
    private List<GameObject> hiddenMatchers;
    /// <summary>
    /// List of the people who are non-matchers
    /// </summary>
    private List<GameObject> nonMatchers;

    private bool isPlaying;

    public static GameManager instance;
    public static event Action OnQuotaMet;

    private void Awake() {
        instance = this;
        isPlaying = true;
    }

    private void Start() {
        featuresToCheckFor = LevelController.GetDailyFeatures();
        quota = LevelController.GetDailyQuota();
        DisableBaskets(); // make baskets inactive
        SpawnBaskets(); // spawn clones of the (now inactive) baskets
        GenerateNonMatchers(); // build up the list of non-matching people
        GenerateHiddenMatchers(); // build up the list of hidden matching people
        guaranteedMatchingPeople = 3; // we can change
        peopleBeforeWolf = 8;

        OnQuotaMet += (() => {
            instance.isPlaying = false;
            Timer.StopCountdown();
        });
        Timer.OnCountdownComplete += (() => instance.isPlaying = false);

        // toggleHiddenMatch = true; // we can change, this system is basic anyways
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
            foreach (Basket b in FindObjectsOfType<Basket>()) {
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
        foreach (Basket b in FindObjectsOfType<Basket>()) {
            foreach (GameObject hidden_go in b.hiddenMatches) {
                hiddenSet.Add(hidden_go);
            }
        }

        // remove people who appeared in other baskets' true matches
        foreach (Basket b in FindObjectsOfType<Basket>()) {
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
        if (!instance.isPlaying)
            return;
        GameObject basketObj = Instantiate(basketPrefabs[idx]);
        basketObj.transform.position = basketPrefabs[idx].transform.position;
        basketObj.SetActive(true);
        Basket basket = basketObj.GetComponent<Basket>();
        basket.basketListIdx = idx;

        basket.basketFeatures = GenerateRandomFeatures(featuresToCheckFor);
        basket.SetMatchesLists();

        GenerateNonMatchers();
        GenerateHiddenMatchers();
    }

    /// <summary>
    /// [deprecated] Generates features that always match one of the baskets.
    /// </summary>
    /// <returns>A PersonFeatures object that should match with one of the baskets.</returns>
    public static PersonFeatures GenerateMatchingFeatures() {
        PersonFeatures basketP = instance.basketPrefabs[UnityEngine.Random.Range(0, instance.basketPrefabs.Count)].
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
        Basket[] active_baskets = FindObjectsOfType<Basket>();
        int basketIdx = UnityEngine.Random.Range(0, active_baskets.Length);
        List<GameObject> matchingPeople = active_baskets[basketIdx].trueMatches;
        int personIdx = UnityEngine.Random.Range(0, matchingPeople.Count);

        try {
            // pick a non-wolf
            GameObject person_go = matchingPeople[personIdx];
            if (matchingPeople.Count < 2)
                throw new IndexOutOfRangeException();
            while (LevelController.GetDailyWolfFeatures().
                NonNoneEquals(person_go.GetComponent<Person>().features)) {
                personIdx = UnityEngine.Random.Range(0, matchingPeople.Count);
                person_go = matchingPeople[personIdx];
            }
            return matchingPeople[personIdx];
        } catch {
            return SelectHiddenMatchingPerson(true);
        }
    }

    /// <summary>
    /// Returns a refernce to one of a random person from hiddenMatchers
    /// </summary>
    /// <returns>A reference to a random person prefab in hiddenMatchers</returns>
    private GameObject SelectHiddenMatchingPerson(bool calledFromMatching = false) {
        try {
            // pick a non-wolf
            GameObject person_go = hiddenMatchers[UnityEngine.Random.Range(0, hiddenMatchers.Count)];
            if (hiddenMatchers.Count < 2)
                throw new IndexOutOfRangeException();
            while (LevelController.GetDailyWolfFeatures().
                NonNoneEquals(person_go.GetComponent<Person>().features)) {
                person_go = hiddenMatchers[UnityEngine.Random.Range(0, hiddenMatchers.Count)];
            }
            return person_go;
        } catch {
            if (calledFromMatching) {
                // no matchers, no hidden matchers, should refresh the list
                ShowRefreshMessage();
                RefreshBaskets();
                return SelectMatchingPerson();
            } else {
                // just take a non-matcher
                return SelectNonmatchingPerson();
            }
        }
    }

    public void IncreaseQuota(int amount) {
        quota += amount;
        quotaUI.UpdateQuotaText();
    }

    public void RefreshBaskets() {
        foreach (Basket b in FindObjectsOfType<Basket>()) {
            Destroy(b.gameObject);
        }
        SpawnBaskets();
    }

    private void ShowRefreshMessage() {
        refreshPanel.gameObject.SetActive(true);
        Color noalpha = refreshPanel.color;
        noalpha.a = 0;
        refreshPanel.color = noalpha;
        LeanTween.alpha(refreshPanel.rectTransform, 1, 1).setOnComplete(
            () => LeanTween.alpha(refreshPanel.rectTransform, 0, 1).setOnComplete(
                () => refreshPanel.gameObject.SetActive(false)));
    }

    /// <summary>
    /// Returns a refernce to one of a random person from nonMatchers
    /// </summary>
    /// <returns>A reference to a random person prefab in nonMatchers</returns>
    private GameObject SelectNonmatchingPerson() {
        try {
            // pick a non-wolf
            GameObject person_go = nonMatchers[UnityEngine.Random.Range(0, nonMatchers.Count)];
            if (nonMatchers.Count < 2)
                throw new IndexOutOfRangeException();
            while (LevelController.GetDailyWolfFeatures().
                NonNoneEquals(person_go.GetComponent<Person>().features)) {
                person_go = nonMatchers[UnityEngine.Random.Range(0, nonMatchers.Count)];
            }
            return person_go;
        } catch (IndexOutOfRangeException) {
            return SelectMatchingPerson();
        }
    }

    private GameObject SelectWolf() {
        // we should pick level specs such that there's always a wolf person
        GameObject[] person_objects = Resources.LoadAll<GameObject>("Persons/");
        List<GameObject> obj_list = new List<GameObject>(person_objects);
        PersonFeatures levelWolf = LevelController.GetDailyWolfFeatures();

        int i = 0;
        while (i < obj_list.Count) {
            PersonFeatures pf = obj_list[i].GetComponent<Person>().features;
            if (!levelWolf.NonNoneEquals(pf))
                obj_list.Remove(obj_list[i]);
            else
                i++;
        }

        return obj_list[UnityEngine.Random.Range(0, obj_list.Count)];
    }

    /// <summary>
    /// Instantiate a new person
    /// </summary>
    public static void CreateNewPerson()  {
        if (!instance.isPlaying) {
            instance.levelController.OnDayCompleted();
            return;
        }

        GameObject personObj;

        instance.peopleSpawned++;
        if (instance.peopleSpawned % guaranteedMatchingPeople == 0) {
            // spawn either off by one or non-matchers
            // person.useRandom = true;
            //if (instance.toggleHiddenMatch) {
            //    personObj = Instantiate(instance.SelectHiddenMatchingPerson());
            //} else {
            //    personObj = Instantiate(instance.SelectNonmatchingPerson()); // FOR NOW
            //}
            //instance.toggleHiddenMatch = !instance.toggleHiddenMatch;

            // so many nonmatches anyways
            // personObj = Instantiate(instance.SelectNonmatchingPerson());
            personObj = Instantiate(instance.SelectMatchingPerson());
        } else if (instance.peopleSpawned % peopleBeforeWolf == 0) {
            // spawn wolf
            personObj = Instantiate(instance.SelectWolf());
        } else {
            // person.useRandom = false;
            personObj = Instantiate(instance.SelectMatchingPerson());
        }
        // This is bad, it would be better to have start position and move vector as an input with a default value that you can change around. This is working for now though...
        personObj.transform.position = new Vector3(15, -.5f, 0); 
        LeanTween.move(personObj, new Vector3(1, -.5f, 0), 2f);
    }

    /// <summary>
    /// Generates a random set of features. If featuresToHave is null, the features
    /// are totally random. Else, the non-NONE features are random, with the rest
    /// being none.
    /// </summary>
    /// <param name="featuresToHave">A template of features to have</param>
    /// <returns></returns>
    public PersonFeatures GenerateRandomFeatures(PersonFeatures featuresToHave = null) {
        if (featuresToHave == null)
            return new PersonFeatures();

        PersonFeatures res = new PersonFeatures(PersonFeatures.FeatureSize.NONE);

        if (featuresToHave.earSize != PersonFeatures.FeatureSize.NONE)
            res.earSize = PersonFeatures.RandomFeatureSize();
        if (featuresToHave.eyeSize != PersonFeatures.FeatureSize.NONE)
            res.eyeSize = PersonFeatures.RandomFeatureSize();
        if (featuresToHave.eyeColor != PersonFeatures.FeatureColor.NONE)
            res.eyeColor = PersonFeatures.RandomFeatureColor();
        if (featuresToHave.noseSize != PersonFeatures.FeatureSize.NONE)
            res.noseSize = PersonFeatures.RandomFeatureSize();
        if (featuresToHave.hairColor != PersonFeatures.FeatureColor.NONE)
            res.hairColor = PersonFeatures.RandomFeatureColor();
        if (featuresToHave.glasses != PersonFeatures.FeatureBool.NONE)
            res.glasses = PersonFeatures.RandomFeatureBool();
        if (featuresToHave.hat != PersonFeatures.FeatureBool.NONE)
            res.hat = PersonFeatures.RandomFeatureBool();
        if (featuresToHave.facialHair != PersonFeatures.FeatureBool.NONE)
            res.facialHair = PersonFeatures.RandomFeatureBool();
        return res;
    }
}
