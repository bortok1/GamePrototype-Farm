using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float movementSpeed = 2.0f;
    public float rotationSpeed = 2.0f;
    public float tileLength = 10.0f;
    //Time
    public TimeStop timeManager;

    private Vector3 _forward = Vector3.forward;
    private int _changeAngle;
    private GameObject _arrow;

    //Hit
    private float differenceInXPosition;
    private float differenceInZPosition;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        _changeAngle = 90;
        if(transform.childCount > 0)
        {
            _arrow = transform.Find("Arrow").gameObject;
            _arrow.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.transform.rotation * Quaternion.Euler(0, _changeAngle, 0), rotationSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.GetComponent<TimeStop>().CheckTimeStop())
            rb.velocity = _forward * 0;
        else
            rb.velocity = _forward * movementSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetType() == typeof(BoxCollider)
            && (collision.gameObject.CompareTag("Wall")))
        {
            _forward = Quaternion.Euler(0, _changeAngle, 0) * _forward;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.transform.rotation * Quaternion.Euler(0, _changeAngle, 0), rotationSpeed);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        TileState tileState = collision.gameObject.GetComponent<TileState>();
        if (tileState != null)
        {
            tileState.ChangeTileState(EPlayerID.RobotDestroyer);
        }
    }

    public void OnHitByPlayer(Vector3 playerLocation)
    {
        Debug.Log("IDK");
        Debug.Log(playerLocation.x);
        differenceInZPosition = Mathf.Abs(playerLocation.z - transform.position.z);
        differenceInXPosition = Mathf.Abs(playerLocation.x - transform.position.x);
        
        if (playerLocation.x > transform.position.x && differenceInXPosition>differenceInZPosition)
        {
            Debug.Log("Player Prawo"); //robot ma isc w lewo 
        }

        if (playerLocation.x < transform.position.x && differenceInXPosition>differenceInZPosition)
        {
            Debug.Log("Player Lewo"); //robot ma isc w prawo
        }

        if (playerLocation.z < transform.position.z && differenceInZPosition>differenceInXPosition)
        {
            Debug.Log("Player Pod"); //robot ma isc w gore
        }

        if (playerLocation.z > transform.position.z && differenceInZPosition>differenceInXPosition)
        {
            Debug.Log("Player Nad"); //robot ma isc w dol
        }

    }
}
