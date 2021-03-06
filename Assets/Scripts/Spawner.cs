using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject starPrefab;
    public IntVariable score;

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
        if (starsCount == 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Point/Enter");
        }
        else if (starsCount == 1)
        {
            AddStar();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Point/Exit");   
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

        score.Value += 100;

    }



    public int CountStar()
    {
        int value;
        value = GameObject.FindGameObjectsWithTag("Star").Length;

        return value;
    }
}
