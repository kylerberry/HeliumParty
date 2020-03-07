using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverRestartButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void RestartGame()
    {
        gameObject.SendMessageUpwards("GameInit");
    }
}
