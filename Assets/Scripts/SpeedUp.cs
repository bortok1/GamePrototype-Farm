using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : PickUp
{
    public float timer;
    public float speedUpBy;
    private Player[] _players;
    private RobotBehavior _robotBehavior;
    private float _timer;
    private bool _collided;
    private float _oldPlayerSpeed;
    private float _oldRobotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        _robotBehavior = FindAnyObjectByType<RobotBehavior>();
        if (_players.Length > 0)
            _oldPlayerSpeed = _players[0].movementSpeed;
        if(_robotBehavior != null)
            _oldRobotSpeed = _robotBehavior.movementSpeed;
        _collided = false;
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_collided)
        {
            if(_timer > 0)
            {
                _timer -= Time.fixedDeltaTime;
            }
            else
            {
                _players[0].movementSpeed = _oldPlayerSpeed;
                _players[1].movementSpeed = _oldPlayerSpeed;
                _robotBehavior.movementSpeed = _oldRobotSpeed;
                Destroy(gameObject);
            }
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            _timer = timer;
            _collided = true;
            _players[0].movementSpeed *= speedUpBy; 
            _players[1].movementSpeed *= speedUpBy; 
            _robotBehavior.movementSpeed *= speedUpBy;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
        
    }
}
