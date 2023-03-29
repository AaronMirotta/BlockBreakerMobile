using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDebug : MonoBehaviour
{
    //speed up the game\
    [SerializeField] private float scale;
    public void SetTimeScale()
    {
        Time.timeScale = scale;
    }
}
