using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rain : PickUp
{
    private Map _mapManager;
    private List<TileState> _tiles = new List<TileState>();

    // Start is called before the first frame update
    void Start()
    {
        _mapManager = FindAnyObjectByType<Map>();
        if( _mapManager != null )
            for(int i = 0; i < _mapManager.transform.childCount; i++)
            {
                _tiles.Add(_mapManager.transform.GetChild(i).gameObject.GetComponent<TileState>());
            }
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            for(int j = 0; j < _tiles.Count; j++)
            {
                _tiles[j].Water();
            }

            Destroy(gameObject);
        }
    }
}
