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

    // this is so bad and so dangerous, also accessories are not supported yet
    private void GenerateBasketList() {
        foreach (Transform t in basketPositions) {
            PersonFeatures p = new PersonFeatures(PersonFeatures.FeatureColor.NONE,
                PersonFeatures.FeatureColor.NONE, PersonFeatures.FeatureSize.NONE,
                null);
            bool checkEyes = featuresToCheckFor.eyes != PersonFeatures.FeatureColor.NONE;
            bool checkHair = featuresToCheckFor.hair != PersonFeatures.FeatureColor.NONE;
            bool checkEars = featuresToCheckFor.ears != PersonFeatures.FeatureSize.NONE;

            List<PersonFeatures.FeatureColor> eyeSet = new List<PersonFeatures.FeatureColor>();
            for (int i = 1; i < PersonFeatures.NumColors; i++) {
                eyeSet.Add((PersonFeatures.FeatureColor)i);
            }
            List<PersonFeatures.FeatureColor> hairSet = new List<PersonFeatures.FeatureColor>(eyeSet);
            List<PersonFeatures.FeatureSize> earSet = new List<PersonFeatures.FeatureSize>();
            for (int i = 1; i < PersonFeatures.NumSizes; i++) {
                earSet.Add((PersonFeatures.FeatureSize)i);
            }

            if (checkEyes && eyeSet.Count > 0) {
                int eyeIdx = Random.Range(0, eyeSet.Count);
                p.eyes = eyeSet[eyeIdx];
                eyeSet.RemoveAt(eyeIdx);
            }
            if (checkHair && hairSet.Count > 0) {
                int hairIdx = Random.Range(0, hairSet.Count);
                p.hair = hairSet[hairIdx];
                hairSet.RemoveAt(hairIdx);
            }
            if (checkEars && earSet.Count > 0) {
                int earIdx = Random.Range(0, earSet.Count);
                p.ears = earSet[earIdx];
                earSet.RemoveAt(earIdx);
            }

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

        if (basketP.eyes != PersonFeatures.FeatureColor.NONE)
            res.eyes = basketP.eyes;
        if (basketP.hair != PersonFeatures.FeatureColor.NONE)
            res.hair = basketP.hair;
        if (basketP.ears != PersonFeatures.FeatureSize.NONE)
            res.ears = basketP.ears;

        res.accessories = basketP.accessories;

        //switch (featuresToCheckFor) {
        //    case 0:
        //        // already randomized
        //        break;
        //    case 1:
        //        res.eyes = basketP.eyes;
        //        break;
        //    case 2:
        //        res.hair = basketP.hair;
        //        goto case 1;
        //    case 3:
        //        res.ears = basketP.ears;
        //        goto case 2;
        //    default:
        //        // accessories not implemented
        //        goto case 3;
        //}

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
