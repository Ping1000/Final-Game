using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuotaUI : MonoBehaviour
{
    /// <summary>
    /// String to prepend onto the quota text.
    /// </summary>
    public string quotaPrefix;
    public TextMeshProUGUI quotaTxt;

    // Start is called before the first frame update
    void Start()
    {
        UpdateQuotaText();
    }

    public void UpdateQuotaText() {
        quotaTxt.text = quotaPrefix + GameManager.instance.CorrectBaskets + " / " +
            GameManager.instance.quota;
    }
}
