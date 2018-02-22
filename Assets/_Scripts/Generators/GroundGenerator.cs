using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GroundGenerator : MonoBehaviour
{

    public EntityManager entityManager;
    public int randomSeed = 1;
    public Tile groundTile;
    public Tile treeTile;
    public Tile rockTile;
    public Tile waterTile;
    public float groundProbability;
    public float forestProbability;
    public float waterProbability;
    public float rockProbability;
    public int xToGenerate;
    public int yToGenerate;

    List<Tile> tiles = new List<Tile>();


    void Start()
    {
        RandomizeMap();
    }

    void floatyRotate()
    {
        transform.rotation = Quaternion.Euler(0f, -.5f, 0f);
        LeanTween.rotateY(gameObject, 0.5f, 15f).setEaseInOutQuad().setLoopPingPong();
        LeanTween.moveY(gameObject, -1f, 20f).setEaseInOutQuad().setLoopPingPong();
    }

    public void RandomizeMap()
    {
        // Remove previous map
        entityManager.removeAllTiles();
        if (randomSeed != 0)
        {
            Random.InitState(randomSeed);
        }
        else
        {
            Debug.Log("Init random with time");
            Random.InitState(System.DateTime.Now.Millisecond);
        }
        Vector3 currentPos = new Vector3(0f, 0f, 0f);
        int i = 0;
        for (int x = 0; x < xToGenerate; ++x)
        {
            for (int y = 0; y < yToGenerate; ++y)
            {
                currentPos = GenerateTile(currentPos, x, y, i);
                ++i;
            }

            currentPos = currentPos + new Vector3(3f, 0f);
            currentPos.z = 0;
            currentPos.y = 0;
        }

        floatyRotate();
        // GenerateElevation();
    }

    void GenerateElevation()
    {
        int i = 0;
        Vector3 currentPos = new Vector3(0, 0);

        for (int x = 0; x < xToGenerate; ++x)
        {
            for (int y = 0; y < yToGenerate; ++y)
            {
                currentPos = setElevation(tiles[i], currentPos);
                ++i;
            }
            currentPos = new Vector3(0f, 0f);
        }

    }

    Vector3 GenerateTile(Vector3 currentPos, int x, int y, int i)
    {
        // TODO: instantiate tile types based on neighbors
        // For now, only the last generated applies
        float chances = Random.Range(0f, 1f);

        if(entityManager.getLastTile() is WaterTile)
        {
        
            createTileOfType(waterTile, currentPos, x, y, i);
        }

        if (chances < forestProbability)
        {
            createTileOfType(groundTile, currentPos, x, y, i);

        }
        else if (forestProbability < chances && chances < waterProbability)
        {
            createTileOfType(treeTile, currentPos, x, y, i);

        }
        else if (chances > waterProbability)
        {
            createTileOfType(waterTile, currentPos, x, y, i);
        }


        return currentPos = currentPos + new Vector3(0f, 0f, 3f);
    }

    Vector3 createTileOfType(Tile tile, Vector3 currentPosition, int x, int y, int i)
    {
        Tile obj = Instantiate(tile, transform);
        obj.transform.position = currentPosition;
        addTile(obj, x, y, i);
        return currentPosition;

    }


    public bool addTile(Tile tile, int x, int y, int i)
    {
        if (!tile) return false;
        tiles.Add(tile);
        //Debug.Log(tiles.Count);
        tile.setCoordinates(x, y);
        if (x > 0)
        {
            tile.SetNeighbor(Tile.TileDirections.W, tiles[i - xToGenerate]);
            tiles[i - xToGenerate].SetNeighbor(Tile.TileDirections.E, tile);
        }
        if (y > 0)
        {
            tile.SetNeighbor(Tile.TileDirections.S, tiles[i - 1]);
            tiles[i - 1].SetNeighbor(Tile.TileDirections.N, tile);
        }
        return true;
    }

    Vector3 setElevation(Tile tile, Vector3 currentPosition)
    {
        if (tile.x < 1 || tile.y < 1 || tile.x == xToGenerate - 1 || tile.y == yToGenerate - 1)
        {
            return currentPosition;
        }
        float heightChance = Random.Range(0f, 1f);
        if (heightChance > 0.01f)
        {
            bool canRise = checkNeighborElevation(tile, currentPosition.y + TileConstants.heightStep);
            if (canRise)
            {
                tile.setElevation(currentPosition.y + TileConstants.heightStep);
                return currentPosition = currentPosition + new Vector3(0, TileConstants.heightStep);
            }
        }
        else
        {
            bool canRise = checkNeighborElevation(tile, currentPosition.y - TileConstants.heightStep);
            if (canRise)
            {
                tile.setElevation(currentPosition.y - TileConstants.heightStep);
                return currentPosition = currentPosition + new Vector3(0, -TileConstants.heightStep);
            }
        }

        return currentPosition;
    }

    bool checkNeighborElevation(Tile tile, float suggested)
    {
        int diff = (int)(suggested / TileConstants.heightStep);
      //  Debug.Log(diff);
        foreach (Tile neighbor in tile.neighbors)
        {

            if (Mathf.Abs(neighbor.elevation - diff)> 1)
            {
                Debug.Log("Was false");
                Debug.Log(neighbor.elevation - diff);
                return false;
            }
            Debug.Log("Was true");
            Debug.Log(neighbor.elevation - diff);

        }
        return true;
    }



    // Update is called once per frame
    void Update()
    {

    }


}

#if UNITY_EDITOR
[CustomEditor(typeof(GroundGenerator))]
public class GroundGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GroundGenerator generator = (GroundGenerator)target;
        if (GUILayout.Button("Regenerate map"))
        {
            generator.RandomizeMap();
        }

    }
}
#endif
