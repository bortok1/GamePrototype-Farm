using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour

{
    public bool stopTimePlayer1 = false;
    public bool stopTimePlayer2 = false;
    [SerializeField] private float timer;
    private float _timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        stopTimePlayer1 = false;
        stopTimePlayer2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckTimeStop())
        {
            StopTime();
        }
        
    }

    public void ResetTime()
    {
        stopTimePlayer1 = false;
        stopTimePlayer2 = false;
    }

    public void StopTimePlayer1()
    {
        stopTimePlayer1 = true;
        stopTimePlayer2 = false;
        SetTime();
    }

    public void StopTimePlayer2()
    {
        stopTimePlayer1 = false;
        stopTimePlayer2 = true;
        SetTime();
    }

    public bool CheckTimeStop() 
    {
        if(stopTimePlayer1)
            return true;
        if(stopTimePlayer2)
            return true;
        return false;

    }

    public bool CheckTimeStopPlayer1()
    {
        if (stopTimePlayer1)
            return true;
        return false;

    }

    public bool CheckTimeStopPlayer2()
    {
        if (stopTimePlayer2)
            return true;
        return false;

    }

    void StopTime()
    {
        _timer -= Time.fixedTime;
        if(_timer <= 0)
        {
            stopTimePlayer1 = false;
            stopTimePlayer2 = false;
        }
    }

    void SetTime()
    {
        _timer = timer;
    }
}
