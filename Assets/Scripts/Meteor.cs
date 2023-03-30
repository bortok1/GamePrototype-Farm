using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Meteor : PickUp
{
    public int howManyMeteors;

    private Map _mapManager;
    private Player _owner;
    private int _index;
    private List<TileState> _opponentTiles = new List<TileState>();
    private TileState[] _opponentTilesArrya;

    // Start is called before the first frame update
    void Start()
    {
        _mapManager = FindAnyObjectByType<Map>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            if(collision.collider.name == "Player1")
            {
                _owner = collision.gameObject.GetComponent<Player>();
                setOpponentTiles(EPlayerID.Player2);
            }
            else 
            if(collision.collider.name == "Player2")
            {
                _owner = collision.gameObject.GetComponent<Player>();
                setOpponentTiles(EPlayerID.Player1);
            }
            destroyOpponentCropps();
            Destroy(gameObject);
        }
    }

    // after collision
    private void setOpponentTiles(EPlayerID ePlayer)
    {
        if (_mapManager != null)
        {
            TileState tile;
            for (int i = 0; i < _mapManager.transform.childCount; i++)
            {
                tile = _mapManager.transform.GetChild(i).gameObject.GetComponent<TileState>();
                if (tile.GetOwner() == ePlayer)
                {
                    _opponentTiles.Add(tile);
                }
            }
        }
    }

    void destroyOpponentCropps()
    {
        _opponentTilesArrya = _opponentTiles.ToArray();
        for(int i = 0; i < howManyMeteors; i++)
        {
            _index = Random.Range(0, _opponentTilesArrya.Length);
            _opponentTilesArrya[_index].SetState(EState.Impassable);
            _opponentTilesArrya[_index]._ownerID = EPlayerID.None;
            for (int a = _index; a < _opponentTilesArrya.Length - 1; a++)
            {
                // moving elements downwards, to fill the gap at [index]
                _opponentTilesArrya[a] = _opponentTilesArrya[a + 1];
            }
            // finally, let's decrement Array's size by one
            Array.Resize(ref _opponentTilesArrya, _opponentTilesArrya.Length - 1);
        }

    }

}
