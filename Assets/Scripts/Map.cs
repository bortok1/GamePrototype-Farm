using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;


public class Map : MonoBehaviour
{
    [SerializeField] int areaSize;
    [SerializeField] int numberOfImpassable;
    [SerializeField] int numberOfOvergrow;
    [SerializeField] GameObject tileGameObject;

    EState [,] tiles;
    List<Vector2Int> availablePos = new List<Vector2Int>();
    float tileSize;

    // Start is called before the first frame update
    void Start()
    {
        tileSize = tileGameObject.transform.localScale.x;
        tiles = new EState[areaSize, areaSize];
        for (int i = 0; i < areaSize; i++)
        {
            for (int j = 0; j < areaSize; j++)
            {
                tiles[i, j] = EState.Empty;
                if (i != 0 && i != 1 && i != areaSize - 1 && i != areaSize - 2)
                {
                    availablePos.Add(new Vector2Int(i, j));
                }
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

        for (int i = 0; i < areaSize; i++)
        {
            for (int j = 0; j < areaSize; j++)
            {
                GameObject newTile = Instantiate(tileGameObject, new Vector3(i*tileSize, 0, j*tileSize), Quaternion.identity);
                newTile.transform.SetParent(this.transform);
                newTile.GetComponent<TileState>().SetState(tiles[i, j]);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
