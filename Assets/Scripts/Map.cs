using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;


public class Map : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] int areaSizeX = 20;
    [SerializeField] [Range(0, 100)] int areaSizeY = 10;
    [SerializeField] [Range(0, 100)] int numberOfImpassable = 15;
    [SerializeField] [Range(0, 100)] int numberOfOvergrow = 30;
    [SerializeField] GameObject tileGameObject;

    [SerializeField] private RawImage sliderPlayer1;
    [SerializeField] private RawImage sliderPlayer2;
    
    [SerializeField] private TextMeshProUGUI textPlayer1;
    [SerializeField] private TextMeshProUGUI textPlayer2;
    
    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] [Range(0, 300)] private float timePerRound = 120;
    [SerializeField] [Range(0, 300)] private float timeBetweenRounds = 5;
    
    private List<TileState> _tileStates = new();
    private readonly List<Vector2Int> _availablePos = new();

    private float _theThing;
    private float _timer;
    private float _smallTimer;

    void Start()
    {
        _timer = timePerRound;
        _smallTimer = timeBetweenRounds;
        
        _theThing = areaSizeX * areaSizeY - numberOfImpassable;
        
        areaSizeX += 2; // add space for
        areaSizeY += 2; // barier around map
        
        GenerateMap();
    }

    private void GenerateMap()
    {
        float tileSize = tileGameObject.transform.localScale.x;
        EState [,] tiles = new EState[areaSizeX, areaSizeY];

        for (int i = 0; i < areaSizeX; i++)
        {
            for (int j = 0; j < areaSizeY; j++)
            {
                tiles[i, j] = EState.Empty;
                if (i == 0 || i == areaSizeX - 1 || j == 0 || j == areaSizeY - 1) // bariers around map
                    tiles[i, j] = EState.Impassable;
                else if ((i > 3 && i < areaSizeX - 4) || !(j <= (areaSizeY / 2) + 1 && j >= (areaSizeY / 2) - 2)) // permanent empty zone
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
            tiles[rndX, rndZ] = EState.Impassable;
            _availablePos[chosenPos] = _availablePos.Last();
            _availablePos.RemoveAt(_availablePos.Count - 1);
        }

        //adding bushes
        for (int i = 0; i < numberOfOvergrow; i++)
        {
            int chosenPos = Random.Range(0, _availablePos.Count);
            int rndX = _availablePos[chosenPos].x;
            int rndZ = _availablePos[chosenPos].y;
            tiles[rndX, rndZ] = EState.Overgrown;
            _availablePos[chosenPos] = _availablePos.Last();
            _availablePos.RemoveAt(_availablePos.Count - 1);
        }

        for (int i = 0; i < areaSizeX; i++)
        {
            for (int j = 0; j < areaSizeY; j++)
            {
                GameObject newTile = Instantiate(tileGameObject, new Vector3(i*tileSize, 0, j*tileSize), Quaternion.identity);
                newTile.transform.SetParent(this.transform);
                TileState newTileState = newTile.GetComponent<TileState>();
                newTileState.SetState(tiles[i, j]);
                _tileStates.Add(newTileState);
            }
        }
    }

    private void ClearMap()
    {
        foreach (TileState tileState in _tileStates)
        {
            Destroy(tileState.gameObject);
        }

        _tileStates = new();
    }
    
    void FixedUpdate()
    {
        if (_timer <= 0)
        {
            _smallTimer -= Time.fixedDeltaTime;
            textTimer.text = "Time's up!";
            if (_smallTimer > 0)
                return;

            _smallTimer = timeBetweenRounds;
            _timer = timePerRound;
            ClearMap();
            GenerateMap();
        }
        
        int p1Score = 0, p2Score = 0;
        foreach (TileState tileState in _tileStates)
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
        
        _timer -= Time.fixedDeltaTime;
        textTimer.text = Mathf.FloorToInt(_timer).ToString();
    }
}
