using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    private float timer;

    void Start()
    {
        timer = timeToDestroy;
    }

    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Destroy(gameObject);
    }
}
