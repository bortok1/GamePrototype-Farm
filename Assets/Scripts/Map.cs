using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;


public class Map : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] int areaSizeX;
    [SerializeField] [Range(0, 100)] int areaSizeY;
    [SerializeField] [Range(0, 100)] int numberOfImpassable;
    [SerializeField] [Range(0, 100)] int numberOfOvergrow;
    [SerializeField] GameObject tileGameObject;

    [SerializeField] private RawImage sliderPlayer1;
    [SerializeField] private RawImage sliderPlayer2;
    
    [SerializeField] private TextMeshProUGUI textPlayer1;
    [SerializeField] private TextMeshProUGUI textPlayer2;
    
    private EState [,] _tiles;
    private List<TileState> _tileStates = new();
    private readonly List<Vector2Int> _availablePos = new();
    private float _tileSize;

    private float _theThing;

    void Start()
    {
        _theThing = areaSizeX * areaSizeY - numberOfImpassable;
        
        areaSizeX += 2; // add space for
        areaSizeY += 2; // barier around map
        
        _tileSize = tileGameObject.transform.localScale.x;
        _tiles = new EState[areaSizeX, areaSizeY];

        for (int i = 0; i < areaSizeX; i++)
        {
            for (int j = 0; j < areaSizeY; j++)
            {
                _tiles[i, j] = EState.Empty;
                if (i == 0 || i == areaSizeX - 1 || j == 0 || j == areaSizeY - 1) // bariers around map
                    _tiles[i, j] = EState.Impassable;
                else if ((i > 3 && i < areaSizeX - 4) || !(j <= (areaSizeY / 2) + 1 && j >= (areaSizeY / 2) - 2))
                    // permanent empty zone
                    _availablePos.Add(new Vector2Int(i, j));
                //else
                  //   tiles[i, j] = EState.Grown;     // use to debug permanent empty zone
            }
        }

        //adding rocks
        for (int i = 0; i < numberOfImpassable; i++)
        {
            int chosenPos = Random.Range(0, _availablePos.Count);
            int rndX = _availablePos[chosenPos].x;
            int rndZ = _availablePos[chosenPos].y;
            _tiles[rndX, rndZ] = EState.Impassable;
            _availablePos[chosenPos] = _availablePos.Last();
            _availablePos.RemoveAt(_availablePos.Count - 1);
        }

        //adding bushes
        for (int i = 0; i < numberOfOvergrow; i++)
        {
            int chosenPos = Random.Range(0, _availablePos.Count);
            int rndX = _availablePos[chosenPos].x;
            int rndZ = _availablePos[chosenPos].y;
            _tiles[rndX, rndZ] = EState.Overgrown;
            _availablePos[chosenPos] = _availablePos.Last();
            _availablePos.RemoveAt(_availablePos.Count - 1);
        }

        for (int i = 0; i < areaSizeX; i++)
        {
            for (int j = 0; j < areaSizeY; j++)
            {
                GameObject newTile = Instantiate(tileGameObject, new Vector3(i*_tileSize, 0, j*_tileSize), Quaternion.identity);
                newTile.transform.SetParent(this.transform);
                TileState newTileState = newTile.GetComponent<TileState>();
                newTileState.SetState(_tiles[i, j]);
                _tileStates.Add(newTileState);
            }
        }
    }

    void FixedUpdate()
    {
        int p1Score = 0, p2Score = 0;
        foreach (var tileState in _tileStates)
        {
            EPlayerID owner = tileState.GetOwner();
            switch (owner)
            {
                case EPlayerID.Player1:
                    p1Score++;
                    break;
                case EPlayerID.Player2:
                    p2Score++;
                    break;
            }
        }

        textPlayer1.text = p1Score.ToString();
        textPlayer2.text = p2Score.ToString();
        sliderPlayer1.transform.localScale = new Vector3(p1Score/_theThing, 1, 1);
        sliderPlayer2.transform.localScale = new Vector3(p2Score/_theThing, 1, 1);
    }
}
