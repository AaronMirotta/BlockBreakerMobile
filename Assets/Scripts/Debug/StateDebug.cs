using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateDebug : MonoBehaviour
{
    [SerializeField]
    private Text stateText;
    
    [SerializeField]
    private GameController state;

    private void Update()
    {
        stateText.text = state.GetState().ToString();
    }
}
