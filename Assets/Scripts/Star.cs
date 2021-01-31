using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Star : MonoBehaviour
{

    public List<Sprite> starsSprite;
    public List<float> starsSize;
    private GameObject spawner;

    private void Awake()
    {
        spawner = GameObject.FindWithTag("Spawner");

        int i = Random.Range(0, starsSprite.Count);
        GetComponent<SpriteRenderer>().sprite = starsSprite[i];

        GetComponent<CircleCollider2D>().radius = starsSize[i];

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
