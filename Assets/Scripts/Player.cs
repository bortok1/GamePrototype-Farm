using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    //Time
    public TimeStop timeManager;
    public int randomKey = 0;
    public bool flagPlayer1 = true;
    public bool flagPlayer2 = true;
    [SerializeField] private TextMeshProUGUI textTimePlayer1;
    [SerializeField] private TextMeshProUGUI textTimePlayer2;

    [SerializeField] public TextMeshProUGUI seedsText;

    //Seeds
    public int seedsp1 = 5;
    public int seedsp2 = 5;

    public PlayerHit hitManager;
    float hitTimer;
    float plantTimer;
    float destroyTimer;
    public bool flagPlayer1Hit = true;
    public bool flagPlayer2Hit = true;
    public bool canPlayer1Hit = false;
    public bool canPlayer2Hit = false;

    public RobotBehavior robot;
    public bool canPlayer1HitRobot = false;
    public bool canPlayer2HitRobot = false;

    private Rigidbody rb;
    public float movementSpeed;
    private float dirX, dirZ;
    public float ID;
    public TileState tileState;

    public Tool tool;
    public EPlayerID playerID;

    public int seeds = 5;

    // Start is called before the first frame update
    void Start()
    {
        seedsText.text = "5";
        rb = GetComponent<Rigidbody>();
        hitTimer = Time.time;
        plantTimer = Time.time;
        destroyTimer = Time.time;
    }

    void FixedUpdate() 
    {

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
            else if (hitManager.GetComponent<PlayerHit>().CheckHitPlayer1())
            {
                if(tool) tool.DropTool();
                tool = null;
                rb.velocity = new Vector3(0, 0, 0);
                if (flagPlayer2Hit)
                {
                    flagPlayer2Hit = false;
                }
            }
            else
            {
                rb.velocity = new Vector3(dirX, rb.velocity.y, dirZ);
                flagPlayer1 = true;
                flagPlayer2Hit = true;
                textTimePlayer1.text = "";
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
            else if (hitManager.GetComponent<PlayerHit>().CheckHitPlayer2())
            {
                if(tool) tool.DropTool();
                tool = null;
                rb.velocity = new Vector3(0, 0, 0);
                if (flagPlayer1Hit)
                {
                    flagPlayer1Hit = false;
                }
            }
            else
            {
                rb.velocity = new Vector3(dirX, rb.velocity.y, dirZ);
                flagPlayer2 = true;
                flagPlayer1Hit = true;
                textTimePlayer2.text = "";
            }
        }
    
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.GetComponent<TileState>())
            tileState = collision.gameObject.GetComponent<TileState>();

        if (collision.gameObject.GetComponent<Tool>())
        {
            GameObject toolHit = collision.gameObject;
            Debug.Log("toolhit");
            if (toolHit.GetComponent<Tool>().owner == EPlayerID.None)
            {
                if (tool)
                {
                    tool.DropTool();
                }
                tool = toolHit.GetComponent<Tool>();
                tool.TakeTool(gameObject);
            }
        }

        if (collision.gameObject.tag == "Seed1" && playerID == EPlayerID.Player1)
        {
            Destroy(collision.gameObject);
            seeds++;
            seedsText.text = seeds.ToString();
        }

        if (collision.gameObject.tag == "Seed2" && playerID == EPlayerID.Player2)
        {
            Destroy(collision.gameObject);
            seeds++;
            seedsText.text = seeds.ToString();
        }
    }

    void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            canPlayer1Hit = true;
            canPlayer2Hit = true;
        }
        if (collider.gameObject.tag == "Robot") 
        {
            canPlayer1HitRobot = true;
            canPlayer2HitRobot = true;
            Debug.Log("ROBOT");
            //Debug.Log(collider.gameObject.transform.position);
            //Debug.Log(collider.gameObject.GetComponent<Rigidbody>().velocity);
        }
    }

    void OnTriggerExit(Collider collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            canPlayer1Hit = false;
            canPlayer2Hit = false;
        }
        if (collider.gameObject.tag == "Robot") 
        {
            canPlayer1HitRobot = false;
            canPlayer2HitRobot = false;
            Debug.Log("ROBOT EXIT");
        }
    }

    void Update()
    {
        if (name == "Player1" && Input.anyKey) 
        {

            if (Input.GetKey(KeyCode.UpArrow))
            {
                dirZ = movementSpeed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                dirZ = -movementSpeed;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                dirX = -movementSpeed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                dirX = movementSpeed;
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                dirZ = 0;
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                dirZ = 0;
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                dirX = 0;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                dirX = 0;
            }
            
            if (Input.GetKey(KeyCode.N) && tileState != null) 
            {
                if (tileState._state == EState.Empty && (Time.time - plantTimer > 0.7f || 
                    (tool && tool.toolType == ETool.Hoe && Time.time - plantTimer > 0.1f)))
                {
                    if (seeds > 0)
                    {
                        plantTimer = Time.time;
                        tileState.ChangeTileState(EPlayerID.Player1);
                        seeds--;
                        seedsText.text = seeds.ToString();
                    }
                }
                if ((Time.time - destroyTimer > 0.7f || (tool && tool.toolType == ETool.Shovel && Time.time - destroyTimer > 0.1f)) &&
                    (tileState._state == EState.Growing || tileState._state == EState.Grown) &&
                    tileState.GetOwner() == EPlayerID.Player2)
                {
                    destroyTimer = Time.time;
                    tileState.ChangeTileState(EPlayerID.Player1);
                }
                if (tileState._state == EState.Overgrown && (Time.time - destroyTimer > 0.7f || 
                    (tool && tool.toolType == ETool.Shovel && Time.time - destroyTimer > 0.1f)))
                {
                    destroyTimer = Time.time;
                    tileState.ChangeTileState(EPlayerID.Player1);
                }
                if ((tileState._state == EState.Grown && tileState.GetOwner() == EPlayerID.Player1))
                {
                    tileState.ChangeTileState(EPlayerID.Player1);
                }
                if (tool && tool.toolType == ETool.WateringCan) tileState.Water();
            }

            if (Input.GetKey(KeyCode.L) && canPlayer1Hit == true && (Time.time - hitTimer > 2.0f) && !hitManager.GetComponent<PlayerHit>().CheckHitPlayer1())
            {
                hitTimer = Time.time;
                Debug.Log("HIT 1");
                hitManager.GetComponent<PlayerHit>().StunP2();
            } 

            if (Input.GetKey(KeyCode.L) && canPlayer1HitRobot == true && (Time.time - hitTimer > 2.0f))
            {
                hitTimer = Time.time;
                robot.GetComponent<RobotBehavior>().OnHitByPlayer(transform.position);
                Debug.Log("HIT ROBOT");
            } 
            //Time

            if (Input.GetKey(KeyCode.M) && !timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer1() && !timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer2() && seeds >= 5)
            {
                timeManager.GetComponent<TimeStop>().StopTimePlayer1();
                seeds -= 5;
                seedsText.text = seeds.ToString();
                //seedsp1 = seeds;
            }
            
        }
        else if (name =="Player1" && !Input.anyKey) 
        {
            dirX = 0f;
            dirZ = 0f;
        }


        if (name == "Player2" && Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W))
            {
                dirZ = movementSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                dirZ = -movementSpeed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                dirX = -movementSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                dirX = movementSpeed;
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                dirZ = 0;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                dirZ = 0;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                dirX = 0;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                dirX = 0;
            }

            if (Input.GetKey(KeyCode.E) && tileState !=null) 
            {
                if (tileState._state == EState.Empty && (Time.time - plantTimer > 0.7f ||
                    (tool && tool.toolType == ETool.Hoe && Time.time - plantTimer > 0.1f)))
                {
                    if (seeds > 0)
                    {
                        seeds--;
                        seedsText.text = seeds.ToString();
                        plantTimer = Time.time;
                        tileState.ChangeTileState(EPlayerID.Player2);
                    }
                }
                if ((Time.time - destroyTimer > 0.7f || (tool && tool.toolType == ETool.Shovel && Time.time - destroyTimer > 0.1f)) &&
                    (tileState._state == EState.Growing || tileState._state == EState.Grown) &&
                    tileState.GetOwner() == EPlayerID.Player1)
                {
                    destroyTimer = Time.time;
                    tileState.ChangeTileState(EPlayerID.Player2);
                }
                if (tileState._state == EState.Overgrown && (Time.time - destroyTimer > 0.7f ||
                    (tool && tool.toolType == ETool.Shovel && Time.time - destroyTimer > 0.1f)))
                {
                    destroyTimer = Time.time;
                    tileState.ChangeTileState(EPlayerID.Player2);
                }
                if ((tileState._state == EState.Grown && tileState.GetOwner() == EPlayerID.Player2))
                {
                    tileState.ChangeTileState(EPlayerID.Player2);
                }
                if (tool && tool.toolType == ETool.WateringCan) tileState.Water();
            }

            if (Input.GetKey(KeyCode.R) && canPlayer2Hit == true && (Time.time - hitTimer > 2.0f) && !hitManager.GetComponent<PlayerHit>().CheckHitPlayer2())
            {
                hitTimer = Time.time;
                Debug.Log("HIT 2");
                hitManager.GetComponent<PlayerHit>().StunP1();
            } 

            if (Input.GetKey(KeyCode.R) && canPlayer2HitRobot == true && (Time.time - hitTimer > 2.0f))
            {
                hitTimer = Time.time;
                robot.GetComponent<RobotBehavior>().OnHitByPlayer(transform.position);
                Debug.Log("HIT ROBOT");
            }

            //Time
            if (Input.GetKey(KeyCode.Q) && !timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer1() && !timeManager.GetComponent<TimeStop>().CheckTimeStopPlayer2() && seeds >= 5)
            {
                timeManager.GetComponent<TimeStop>().StopTimePlayer2();
                seeds -= 5;
                seedsText.text = seeds.ToString();
                //seedsp2 = seeds;
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
                    textTimePlayer1.text = "UP";
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
                    textTimePlayer1.text = "Down";
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
                    textTimePlayer1.text = "Left";
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
                    textTimePlayer1.text = "Right";
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
        //I 1 W
        //J 2 A
        //K 3 S
        //L 4 D
        int i = 0;
        bool done = false;
        while (i < 3)
        {
            randomKey = Random.Range(1, 5);
            switch (randomKey)
            {
                case 1:
                    textTimePlayer2.text = "W";
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.W))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
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
                    textTimePlayer2.text = "A";
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.A))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
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
                    textTimePlayer2.text = "S";
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.S))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
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
                    textTimePlayer2.text = "D";
                    done = false;
                    while (!done)
                    {
                        if (Input.GetKey(KeyCode.D))
                        {
                            i++;
                            done = true;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
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
