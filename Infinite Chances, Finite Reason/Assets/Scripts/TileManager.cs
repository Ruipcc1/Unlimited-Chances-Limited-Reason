using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float tileLength = 30;

    public int numberOfTiles = 5;

    public Transform playerTransform;

    public List<GameObject> activeTiles = new List<GameObject>();

    public List<int> previousTiles = new List<int>();

    public List<int> recordTiles = new List<int>();

    public List<GameObject> test = new List<GameObject>();

    private bool isRewinding = false;

    public int rewindnumber;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
            {
                SpawnTile(0);

            }
            else
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length - 1));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRewinding)
        {
            if (playerTransform.position.z - 35 > zSpawn - (numberOfTiles * tileLength))
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length - 1));               
                DeleteTile(0);
            }
        }
        if (isRewinding)
        {
            if (playerTransform.position.z < zSpawn - (numberOfTiles * tileLength))
            {
                int previousTile = recordTiles[0];
                SpawnPreviousTile(previousTile);
                DeleteTile(5);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            StopRewind();

 
        rewindnumber = previousTiles.Count;
    }

    private void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    public void SpawnTile(int tileIndex)
    {
        GameObject newTile = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(newTile);
        recordTiles.Add(tileIndex);
        zSpawn += tileLength;
    }

    public void SpawnPreviousTile(int tileIndex)
    {
        GameObject newTile = Instantiate(tilePrefabs[tileIndex], transform.forward * (zSpawn - 6 * tileLength), transform.rotation);
        activeTiles.Insert(0, newTile);
        recordTiles.Insert(0, tileIndex);
        zSpawn -= tileLength;
    }

    private void DeleteTile(int tile)
    {
        Destroy(activeTiles[tile]);
        activeTiles.RemoveAt(tile);
        recordTiles.RemoveAt(tile);
    }

    void Rewind()
    {
        recordTiles[0] = previousTiles[0];
        previousTiles.RemoveAt(0);
    }
    void Record()
    {
        previousTiles.Insert(0, recordTiles[0]);
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }
}
