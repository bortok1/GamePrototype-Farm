using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public bool afterHitPlayer1 = false;
    public bool afterHitPlayer2 = false;
    [SerializeField] private float timer;
    private float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        afterHitPlayer1 = false;
        afterHitPlayer2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckHit())
        {
            StopTime();
        }
    }

    public void StunP1()
    {
        afterHitPlayer1 = true;
        afterHitPlayer2 = false;
        SetTime();
    }

    public void StunP2()
    {
        afterHitPlayer1 = false;
        afterHitPlayer2 = true;
        SetTime();
    }

    public void ResetTime()
    {
        afterHitPlayer1 = false;
        afterHitPlayer2 = false;
    }


    public bool CheckHit() 
    {
        if(afterHitPlayer1)
            return true;
        if(afterHitPlayer2)
            return true;
        return false;

    }

    public bool CheckHitPlayer1()
    {
        if (afterHitPlayer1)
            return true;
        return false;

    }

    public bool CheckHitPlayer2()
    {
        if (afterHitPlayer2)
            return true;
        return false;

    }

    void StopTime()
    {
        _timer -= Time.fixedDeltaTime;
        if(_timer <= 0)
        {
            afterHitPlayer1 = false;
            afterHitPlayer2 = false;
        }
    }

    void SetTime()
    {
        _timer = timer;
    }
}
