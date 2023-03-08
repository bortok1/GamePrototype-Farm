using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum EPlayerID
{
    None,
    Player1,
    Player2,
    RobotDestroyer
}

public enum EState
{
    Empty,
    Growing,
    Grown,
    Impassable,
    Overgrown,
    Burned
}

public class TileState : MonoBehaviour
{
    private EPlayerID _ownerID;
    [SerializeField] private EState _state;
    [SerializeField] private float _timer;
    
    [SerializeField] public Mesh emptyMesh;
    [SerializeField] public Mesh growingMesh;
    [SerializeField] public Mesh grownMesh;
    [SerializeField] public Mesh impassableMesh;
    [SerializeField] public Mesh overgrownMesh;
    [SerializeField] public Mesh burnedMesh;
    
    private MeshFilter _myMesh;
    
    [SerializeField] public GameObject seedPrefab;
    [SerializeField] public float swingStrength = 100;    // How far seed will fly

    void Start()
    {
        _ownerID = 0;
        _timer = 2;
    }

    private void FixedUpdate()
    {
        if (_timer > 0)
        {
            _timer -= Time.fixedTime;
            if (_timer < 0)
            {
                switch (_state)
                {
                    case EState.Burned:
                        SetState(EState.Empty);
                        break;
                    case EState.Growing:
                        SetState(EState.Grown);
                        break;
                    default:
                        Debug.Log("<color=yellow> Warning: Timer ended with odd state!");
                        break;
                }
            }
        }
    }

    public void SetState(EState newState)
    {
        if (_myMesh == null)
            _myMesh = GetComponentInChildren<MeshFilter>();

        _state = newState;
        switch (_state)
        { 
            case EState.Empty: _myMesh.sharedMesh  = emptyMesh; break;
            case EState.Growing: _myMesh.sharedMesh = growingMesh; SetTimerGrowing(); break;
            case EState.Grown: _myMesh.sharedMesh = grownMesh; break;
            case EState.Impassable: _myMesh.sharedMesh = impassableMesh; break;
            case EState.Overgrown: _myMesh.sharedMesh = overgrownMesh; break;
            case EState.Burned: _myMesh.sharedMesh = burnedMesh; SetTimerBurned(); break;
        }
    }
    
    public void ChangeTileState(EPlayerID playerID)
    {
        if (_ownerID == EPlayerID.None) // Tile has no owner.
        {
            switch (playerID)
            {
                case EPlayerID.Player1:
                case EPlayerID.Player2:
                    if (_state == EState.Overgrown)
                    {
                        SetState(EState.Empty);
                    }
                    else
                    {
                        _ownerID = playerID;
                        SetState(EState.Growing);                        
                    }
                    break;
            }
        }
        else if (playerID == _ownerID)   // Tile has owner. Owner of tile called.
        {
            if (_state == EState.Grown)
            {
                SlingSeed();
                SetState(EState.Growing);
            }
        }
        else if (playerID is EPlayerID.Player1 or EPlayerID.Player2)  // Tile has owner. Enemy player called.
        {
            SetState(EState.Burned);
        }
        else if (playerID == EPlayerID.RobotDestroyer) // Tile has owner. Robot called.
        {
            SetState(EState.Burned);
        }
    }

    private void SlingSeed()
    {
        GameObject newSeed = Instantiate(seedPrefab, this.transform.position + new Vector3(0,3,0), Quaternion.identity);
        Rigidbody newSeedRigidbody = newSeed.GetComponent<Rigidbody>();
        if (newSeedRigidbody != null)
        {
            int halfMaxRange = 50;
            Vector3 randomVector = new Vector3(Random.Range(-halfMaxRange * 2, halfMaxRange * 2), halfMaxRange, Random.Range(-halfMaxRange * 2, halfMaxRange * 2)).normalized;
            newSeedRigidbody.AddForce(randomVector * swingStrength);
        }
    }

    private void SetTimerGrowing()
    {
        _timer = 10f;
    }
    
    private void SetTimerBurned()
    {
        _timer = 10f;
    }
}