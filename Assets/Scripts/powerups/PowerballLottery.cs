using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerballLottery : MonoBehaviour
{

    float maxProbability = 0.2f;

    public bool PlayLottery(int numLives, int numEnemies)
    {           
        float probability = Mathf.Min((maxProbability / numLives) * numEnemies, maxProbability);
        return Random.value <= probability;
    }
}
