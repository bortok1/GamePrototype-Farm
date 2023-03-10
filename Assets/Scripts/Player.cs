using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        rb.velocity = new Vector3(dirX, rb.velocity.y, dirZ);
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

        }
        else if (name =="Player2" && !Input.anyKey) 
        {
            dirX = 0f;
            dirZ = 0f;
        }
    }
}
