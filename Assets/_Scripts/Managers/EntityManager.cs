using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public List<Tile> tiles;
    public List<GameObject> plants = new List<GameObject>();

    private GameObject playerCharacter;

    private int width;
    private int height;
    // Use this for initialization
    void Start()
    {
        height = GameObject.FindObjectOfType<GroundGenerator>().islandHeight;
        width = GameObject.FindObjectOfType<GroundGenerator>().islandWidth;
    }


    public void SpawnPlayer()
    {
        if (playerCharacter)
        {
            Debug.LogError("Player is already spawned");
            return;
        }

        playerCharacter = Instantiate(playerPrefab, transform, true);

    }
    public void removeAllTiles()
    {
        if (tiles.Count > 0)
        {
            foreach (Tile tile in tiles)
            {
                Destroy(tile);
            }

        }
    }


    public bool addTile(Tile tile, int x, int y, int i)
    {
        if (!tile) return false;
        tile.setCoordinates(x, y);
        if (x > 0)
        {
            tile.SetNeighbor(Tile.TileDirections.W, tiles[i - width]);
            tiles[i - width].SetNeighbor(Tile.TileDirections.E, tile);
        }
        if (y > 0)
        {
            tile.SetNeighbor(Tile.TileDirections.S, tiles[i - 1]);
            tiles[i - 1].SetNeighbor(Tile.TileDirections.N, tile);
        }
        tiles[i] = tile;
        Debug.Log(tiles);

        //Elevation 

        return true;
    }


    public bool addPlant(GameObject plant)
    {
        if (!plant) return false;
        plants.Add(plant);
        return true;
    }


    public bool growPlants()
    {
        foreach (GameObject plant in plants)
        {
            plant.GetComponent<Plant>().Grow();
        }
        return true;
    }
}
