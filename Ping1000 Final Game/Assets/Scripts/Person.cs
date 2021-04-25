using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Person : MonoBehaviour {
    public PersonFeatures features;
    public bool useRandom;

    private void Start() {
        if (useRandom)
            features = new PersonFeatures();
        else
            features = GameManager.GenerateMatchingFeatures();

        CreateFeatureGameObjects();
        Invoke("UpdateBasketPerson", Time.deltaTime);
        //FindObjectOfType<DenyButton>().UpdatePerson(this);
    }

    // THIS IS TERRIBLE but i'm keeping it for now
    private void UpdateBasketPerson() {
        foreach (Basket b in FindObjectsOfType<Basket>()) {
            b.UpdatePerson();
        }
    }

    private void CreateFeatureGameObjects() {
        // might need to store positions for hair/eyes/ears
        // if we have multiple persons of potentially different sizes
        CreateHair();
        CreateEars();
        CreateEyes();
    }

    private void CreateHair() {
        GameObject[] hairs = Resources.LoadAll<GameObject>("Hair");
        // randomly assign hair for now
        GameObject hairObj = Instantiate(hairs[Random.Range(0, hairs.Length)], transform);
        IColorable hair = hairObj.GetComponent<IColorable>();
        hair.SetColor(features.hairColor);
    }

    private void CreateEars() {
        GameObject[] ears = Resources.LoadAll<GameObject>("Ears");
        // randomly assign ear for now
        GameObject earObj = Instantiate(ears[Random.Range(0, ears.Length)], transform);
        ISizeable ear1 = earObj.GetComponent<ISizeable>();
        ear1.SetSize(features.earSize);

        DuplicateOnOppositeSide(earObj, transform);
    }

    private void CreateEyes() {
        GameObject[] eyes = Resources.LoadAll<GameObject>("Eyes");
        // randomly assign eye for now
        GameObject eyeObj = Instantiate(eyes[Random.Range(0, eyes.Length)], transform);
        IColorable eye = eyeObj.GetComponent<IColorable>();
        eye.SetColor(features.eyeColor);

        DuplicateOnOppositeSide(eyeObj, transform);
    }

    private void DuplicateOnOppositeSide(GameObject obj, Transform parent) {
        GameObject clone = Instantiate(obj, parent);
        Vector3 cloneRot = obj.transform.rotation.eulerAngles;
        cloneRot.y += 180;
        clone.transform.rotation = Quaternion.Euler(cloneRot);

        Vector3 clonePos = obj.transform.localPosition;
        clonePos.x *= -1;
        clone.transform.localPosition = clonePos;
    }
}
