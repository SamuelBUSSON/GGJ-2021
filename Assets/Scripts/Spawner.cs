using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject starPrefab;

    public List<GameObject> spawners;
    private int nbSpawner;

    public int starsCount; 
    // Start is called before the first frame update
    void Start()
    {
        starsCount = CountStar();
        nbSpawner = spawners.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStar()
    {
        
        starsCount= CountStar();
        Debug.Log(starsCount);
        if (starsCount == 1)
        {
            AddStar();
            
        }
    }

    public void AddStar()
    {
        int randomInt;
        for (int i = 0; i < 2; i++)
        {
            randomInt = Random.Range(0, nbSpawner);
            Instantiate(starPrefab, spawners[randomInt].transform.position, quaternion.identity);
            spawners.RemoveAt(randomInt);
            nbSpawner--;
            Debug.Log("Spawned");
        }
    }



    public int CountStar()
    {
        int value;
        value = GameObject.FindGameObjectsWithTag("Star").Length;

        return value;
    }
}
