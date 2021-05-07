using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextMeshLayer : MonoBehaviour
{
    public string layerToPushTo;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingLayerName = layerToPushTo;
        renderer.sortingOrder = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
