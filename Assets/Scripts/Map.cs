using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Serialization;


public class Map : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] int areaSizeX;
    [SerializeField] [Range(0, 100)] int areaSizeY;
    [SerializeField] [Range(0, 100)] int numberOfImpassable;
    [SerializeField] [Range(0, 100)] int numberOfOvergrow;
    [SerializeField] GameObject tileGameObject;

    EState [,] tiles;
    List<Vector2Int> availablePos = new List<Vector2Int>();
    float tileSize;

    // Start is called before the first frame update
    void Start()
    {
        areaSizeX += 2; // add space for
        areaSizeY += 2; // barier around map
        
        tileSize = tileGameObject.transform.localScale.x;
        tiles = new EState[areaSizeX, areaSizeY];

        for (int i = 0; i < areaSizeX; i++)
        {
            for (int j = 0; j < areaSizeY; j++)
            {
                tiles[i, j] = EState.Empty;
                if (i == 0 || i == areaSizeX - 1 || j == 0 || j == areaSizeY - 1) // bariers around map
                    tiles[i, j] = EState.Impassable;
                else if ((i > 3 && i < areaSizeX - 4) || !(j <= (areaSizeY / 2) + 1 && j >= (areaSizeY / 2) - 2))
                    // permanent empty zone
                    availablePos.Add(new Vector2Int(i, j));
                //else
                  //   tiles[i, j] = EState.Grown;     // use to debug permanent empty zone
            }
        }

        //adding rocks
        for (int i = 0; i < numberOfImpassable; i++)
        {
            int chosenPos = Random.Range(0, availablePos.Count);
            int rndX = availablePos[chosenPos].x;
            int rndZ = availablePos[chosenPos].y;
            tiles[rndX, rndZ] = EState.Impassable;
            availablePos[chosenPos] = availablePos.Last();
            availablePos.RemoveAt(availablePos.Count - 1);
        }

        //adding bushes
        for (int i = 0; i < numberOfOvergrow; i++)
        {
            int chosenPos = Random.Range(0, availablePos.Count);
            int rndX = availablePos[chosenPos].x;
            int rndZ = availablePos[chosenPos].y;
            tiles[rndX, rndZ] = EState.Overgrown;
            availablePos[chosenPos] = availablePos.Last();
            availablePos.RemoveAt(availablePos.Count - 1);
        }

        for (int i = 0; i < areaSizeX; i++)
        {
            for (int j = 0; j < areaSizeY; j++)
            {
                GameObject newTile = Instantiate(tileGameObject, new Vector3(i*tileSize, 0, j*tileSize), Quaternion.identity);
                newTile.transform.SetParent(this.transform);
                newTile.GetComponent<TileState>().SetState(tiles[i, j]);
            }
        }

    }
}
