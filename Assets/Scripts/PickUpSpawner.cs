using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    public GameObject pickUp;
    public float howOftenSpawn;
    public float radius;

    private float _timer;

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
                Spawning();
            }
        }
    }

    GameObject Spawning()
    { 
        GameObject spawning = GameObject.Instantiate(pickUp);
        float x, y;
        x = Random.Range(-radius, radius);
        y = Random.Range(-radius, radius);
        Vector3 vec = new Vector3(x,0.0f,y);
        spawning.transform.SetLocalPositionAndRotation(vec, Quaternion.identity);
        spawning.transform.SetParent(transform, false);

        return spawning;
    } 
}


