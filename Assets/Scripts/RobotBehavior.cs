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

    private Vector3 _forward = Vector3.forward;
    private int _randomChangeDirection;
    private int _changeAngle;
    private bool _checkState = false; 
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = _forward * movementSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<TileState>() != null)
        {
            if(collision.gameObject.GetComponent<TileState>()._state == EState.Impassable)
                _checkState = true;
        }
        if (collision.collider.GetType() == typeof(BoxCollider) 
            && (collision.gameObject.tag == "Wall" || _checkState))
        {
            _randomChangeDirection = Random.Range(1, 3);
            if (_randomChangeDirection == 1)
            {
                _changeAngle = 90;
            }
            else
            if (_randomChangeDirection == 2)
            {
                _changeAngle = -90;
            }
            _forward = Quaternion.Euler(0, _changeAngle, 0) * _forward;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.transform.rotation * Quaternion.Euler(0, _changeAngle, 0), rotationSpeed);
        }
        _checkState = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<TileState>() != null)
        {
            if (collision.gameObject.GetComponent<TileState>()._state == EState.Growing
                || collision.gameObject.GetComponent<TileState>()._state == EState.Grown
                || collision.gameObject.GetComponent<TileState>()._state == EState.Overgrown)
                    collision.gameObject.GetComponent<TileState>().SetState(EState.Burned);
        }
    }
}
