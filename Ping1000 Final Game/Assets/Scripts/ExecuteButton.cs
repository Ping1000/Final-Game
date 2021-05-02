using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteButton : MonoBehaviour
{
    public void Execute() {
        Vector3 pos = FindObjectOfType<Person>().transform.position;
        GameObject axe = Instantiate(Resources.Load<GameObject>("Axe Throw"));
        axe.transform.position = pos;
    }
}
