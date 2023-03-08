using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    private EState _state;
    
    [SerializeField] public Mesh emptyMesh;
    [SerializeField] public Mesh growingMesh;
    [SerializeField] public Mesh grownMesh;
    [SerializeField] public Mesh impassableMesh;
    [SerializeField] public Mesh overgrownMesh;
    
    private Mesh _myMesh;
    
    [SerializeField] public GameObject seedPrefab;
    [SerializeField] public float swingStrength = 2;    // How far seed will fly

    void Start()
    {
        _myMesh = this.GetComponent<Mesh>();
        _ownerID = 0;
    }
    
    public void SetState(EState newState)
    {
        _state = newState;
        switch (_state)
        { 
            case EState.Empty: _myMesh = emptyMesh; break;
            case EState.Growing: _myMesh = growingMesh; break;
            case EState.Grown: _myMesh = grownMesh; break;
            case EState.Impassable: _myMesh = impassableMesh; break;
            case EState.Overgrown: _myMesh = overgrownMesh; break;
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
            Vector3 randomVector = new Vector3(Random.Range(0, halfMaxRange * 2), halfMaxRange, Random.Range(0, halfMaxRange * 2)).normalized;
            newSeedRigidbody.AddForce(randomVector * swingStrength);
        }
    }
}
