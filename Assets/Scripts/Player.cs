using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Time
    public TimeStop timeManager;
    public int randomKey = 0;
    public bool flagPlayer1 = true;
    public bool flagPlayer2 = true;


    private Rigidbody rb;
    public float movementSpeed;
    private float dirX, dirZ;
    public float ID;
    public TileState tileState;
    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() 
    {
        //rb.velocity = new Vector3(dirX, rb.velocity.y, dirZ);

        //Time
        if (name == "Player1")
        {
            if (timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer2())
            {
                rb.velocity = new Vector3(0, 0, 0);
                if (flagPlayer1)
                {
                    StartCoroutine(Player1TimeEvent());
                    flagPlayer1 = false;
                }   
            }
            else
            {
                rb.velocity = new Vector3(dirX, rb.velocity.y, dirZ);
                flagPlayer1 = true;
            }
                
        }

        if (name == "Player2")
        {
            if (timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer1())
            {
                rb.velocity = new Vector3(0, 0, 0);
                if (flagPlayer2)
                {
                    StartCoroutine(Player2TimeEvent());
                    flagPlayer2 = false;
                }
            }
            else
            {
                rb.velocity = new Vector3(dirX, rb.velocity.y, dirZ);
                flagPlayer2 = true;
            }
        }
    
    }

    void OnCollisionEnter(Collision collision) 
    {

        tileState = collision.gameObject.GetComponent<TileState>();
    }

    void Update()
    {
        if (name == "Player1") 
        {
            dirX = Input.GetAxis("Horizontal") * movementSpeed;
            dirZ = Input.GetAxis("Vertical") * movementSpeed;
            
            if (Input.GetKey(KeyCode.E) && tileState !=null) 
            {
                tileState.ChangeTileState(EPlayerID.Player1);
            }
            //Time

            if (Input.GetKey(KeyCode.M) && !timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer2())
            {
                timeManager.GetComponent<TimeStop>().StopTimePlayer1();
            }
            
        }
        if (name == "Player2" && Input.anyKey)
        {
            if (Input.GetKey(KeyCode.I))
            {
                dirZ = movementSpeed;
            }
            if (Input.GetKey(KeyCode.K))
            {
                dirZ = -movementSpeed;
            }
            if (Input.GetKey(KeyCode.J))
            {
                dirX = -movementSpeed;
            }
            if (Input.GetKey(KeyCode.L))
            {
                dirX = movementSpeed;
            }

            if (Input.GetKeyUp(KeyCode.I))
            {
                dirZ = 0;
            }
            if (Input.GetKeyUp(KeyCode.K))
            {
                dirZ = 0;
            }
            if (Input.GetKeyUp(KeyCode.J))
            {
                dirX = 0;
            }
            if (Input.GetKeyUp(KeyCode.L))
            {
                dirX = 0;
            }

            if (Input.GetKey(KeyCode.O) && tileState !=null) 
            {
                tileState.ChangeTileState(EPlayerID.Player2);
            }

            //Time
            if (Input.GetKey(KeyCode.N) && !timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer1())
            {
                timeManager.GetComponent<TimeStop>().StopTimePlayer2();
            }
        }
        else if (name =="Player2" && !Input.anyKey) 
        {
            dirX = 0f;
            dirZ = 0f;
        }
    }

    //Time
    private IEnumerator Player1TimeEvent()
    {
        //UpArrow 1
        //DownArrow 2
        //LeftArrow 3
        //RightArrow 4
        int i = 0;
        bool done = false;
        while (i < 3)
        {
            randomKey = Random.Range(1, 5);
            switch (randomKey)
            {
                case 1:
                    Debug.Log("UP");
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.UpArrow))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
                        {
                            i = 0;
                            yield return new WaitForSeconds(0.2f);
                        }
                        if (!timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer2())
                        {
                            done = true;
                            i = 4;
                        }
                        yield return null;
                    }
                    break;
                case 2:
                    Debug.Log("Down");
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.DownArrow))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                        {
                            i = 0;
                            yield return new WaitForSeconds(0.2f);
                        }

                        if (!timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer2())
                        {
                            done = true;
                            i = 4;
                        }
                        yield return null;
                    }
                    break;
                case 3:
                    Debug.Log("Left");
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.LeftArrow))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
                        {
                            i = 0;
                            yield return new WaitForSeconds(0.2f);
                        }
                        if (!timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer2())
                        {
                            done = true;
                            i = 4;
                        }
                        yield return null;
                    }
                    break;
                case 4:
                    Debug.Log("Right");
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.RightArrow))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow))
                        {
                            i = 0;
                            yield return new WaitForSeconds(0.2f);
                        }
                        if (!timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer2())
                        {
                            done = true;
                            i = 4;
                        }
                        yield return null;
                    }
                    break;

            }
        }
        Debug.Log("--------Time Reset--------");
        timeManager.GetComponent<TimeStop>().ResetTime();
    }

    private IEnumerator Player2TimeEvent()
    {
        //I 1
        //J 2
        //K 3
        //L 4
        int i = 0;
        bool done = false;
        while (i < 3)
        {
            randomKey = Random.Range(1, 5);
            switch (randomKey)
            {
                case 1:
                    Debug.Log("I");
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.I))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.K))
                        {
                            i = 0;
                            yield return new WaitForSeconds(0.2f);
                        }
                        if (!timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer1())
                        {
                            done = true;
                            i = 4;
                        }
                        yield return null;
                    }
                    break;
                case 2:
                    Debug.Log("J");
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.J))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.I))
                        {
                            i = 0;
                            yield return new WaitForSeconds(0.2f);
                        }
                        if (!timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer1())
                        {
                            done = true;
                            i = 4;
                        }
                        yield return null;
                    }
                    break;
                case 3:
                    Debug.Log("K");
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.K))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.I))
                        {
                            i = 0;
                            yield return new WaitForSeconds(0.2f);
                        }
                        if (!timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer1())
                        {
                            done = true;
                            i = 4;
                        }
                        yield return null;
                    }
                    break;
                case 4:
                    Debug.Log("L");
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.L))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.I))
                        {
                            i = 0;
                            yield return new WaitForSeconds(0.2f);
                        }
                        if (!timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer1())
                        {
                            done = true;
                            i = 4;
                        }
                        yield return null;
                    }
                    break;

            }
        }
        Debug.Log("--------Time Reset--------");
        timeManager.GetComponent<TimeStop>().ResetTime();
    }
}
