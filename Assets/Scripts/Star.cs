using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private GameObject spawner;

    private void Awake()
    {
        spawner = GameObject.FindWithTag("Spawner");
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.CompareTag("Player"))
        {
            co.gameObject.GetComponent<Moving>().GetStar();
            spawner.GetComponent<Spawner>().UpdateStar();
            Destroy(gameObject);

        }
    }
}
