using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressPie : MonoBehaviour
{

    Image pie;
    TextMeshProUGUI piePercent;

    // Start is called before the first frame update
    void Start()
    {
        pie = transform.Find("PieGraph").Find("PieSlice").GetComponent<Image>();
        piePercent = transform.Find("PiePercent").GetComponent<TextMeshProUGUI>();
        pie.fillAmount = 0;
        piePercent.text = "0%";
    }

    public void SetProgress(float percent)
    {
       /* Debug.Log(percent);*/
        pie.fillAmount = percent;
        piePercent.text = Mathf.Min(100, Mathf.Floor(percent * 100)) + "%";
    }
}
