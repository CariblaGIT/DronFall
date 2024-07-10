using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPointUI : MonoBehaviour
{

    public GameObject containerHP;
    private float containerFillValue;
    private int healthValue;

    // Update is called once per frame
    void Update()
    {
        healthValue = GameManager.Health;
        switch (healthValue)
        {
            case 10:
                containerFillValue = 1f;
                break;
            case 9:
                containerFillValue = 0.745f;
                break;
            case 8:
                containerFillValue = 0.665f;
                break;
            case 7:
                containerFillValue = 0.585f;
                break;
            case 6:
                containerFillValue = 0.505f;
                break;
            case 5:
                containerFillValue = 0.425f;
                break;
            case 4:
                containerFillValue = 0.345f;
                break;
            case 3:
                containerFillValue = 0.265f;
                break;
            case 2:
                containerFillValue = 0.185f;
                break;
            case 1:
                containerFillValue = 0.105f;
                break;
            case 0:
                containerFillValue = 0f;
                break;
        }

        containerHP.GetComponent<Image>().fillAmount = containerFillValue;
    }
}
