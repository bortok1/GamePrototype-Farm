using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickUpSpawner : MonoBehaviour
{
    public GameObject[] pickUps;
    public float howOftenSpawn;
    public float radius;

    private float _timer;
    private int _index;

    // Start is called before the first frame update
    void Start()
    {
        _timer = howOftenSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        SpawningTimer();
    }

    void SpawningTimer()
    {
        if (this.transform.childCount < 1)
        {
            if (_timer > 0.0f )
            {
                _timer -= Time.fixedDeltaTime;
            }
            else
            {
                _timer = howOftenSpawn;
                _index = Random.Range(0, pickUps.Length);
                Spawning(pickUps[_index]);
            }
        }
    }

    GameObject Spawning(GameObject pickup)
    {
        Debug.Log(pickup);
        GameObject spawning = GameObject.Instantiate(pickup);
        float x, y;
        x = Random.Range(-radius, radius);
        y = Random.Range(-radius, radius);
        Vector3 vec = new Vector3(x,0.0f,y);
        spawning.transform.SetLocalPositionAndRotation(vec, Quaternion.identity);
        spawning.transform.SetParent(transform, false);

        return spawning;
    } 
}


