using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField]
    private float duration;

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}
