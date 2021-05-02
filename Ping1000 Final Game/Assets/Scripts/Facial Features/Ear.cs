using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ear : MonoBehaviour, ISizeable
{
    public void SetSize(PersonFeatures.FeatureSize size) {
        Vector3 curScale = transform.localScale;
        switch (size) {
            case PersonFeatures.FeatureSize.small:
                // nothing for now
                break;
            //case PersonFeatures.FeatureSize.medium:
            //    curScale *= 2;
            //    break;
            case PersonFeatures.FeatureSize.large:
                curScale *= 4;
                break;
        }
        curScale.z = 1;
        transform.localScale = curScale;
    }
}
