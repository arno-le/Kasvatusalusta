using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GroundGenerator : MonoBehaviour
{

    public Tile.Season season;
    public List<Tile> tiles;

    public Texture2D placeholder;
    public int islandWidth;
    public int islandHeight;

    public int bottomWidth;
    public int bottomHeight;

    public float noiseScale = 0.05f;
    public int octaves;
    public float persistence;
    public float lacunarity;

    public int randomSeed = 1;

    public EntityManager entityManager;
    public Tile groundTile;
    public Tile treeTile;
    public Tile rockTile;
    public Tile waterTile;
    public float groundProbability;
    public float forestProbability;
    public float waterProbability;
    public float rockProbability;


    private int initialIslandWidth;
    private int initialIslandHeight;

    private Dropdown m_Dropdown;

    void Start()
    {
        m_Dropdown = FindObjectOfType<Dropdown>();
       //@Todo: jos tämä ei muuttaisi vuodenaikaa suoraan takaisin kesäksi -.-
       // m_Dropdown.value = (int)season;
        m_Dropdown.onValueChanged.AddListener(delegate {
            SetSeason(m_Dropdown.value);
        });
        initialIslandHeight = islandHeight;
        initialIslandWidth = islandWidth;
        StartCoroutine(RandomizeEveryXSeconds(5f));
        floatyRotate();
    }

    public void EditorGenerate(int width, int height)
    {
        initialIslandHeight = height;
        initialIslandWidth = width;
        RandomizeTiles();
    }

    IEnumerator RandomizeEveryXSeconds(float seconds)
    {
        while (true)
        {
            yield return StartCoroutine(RandomizeMap());
            yield return new WaitForSecondsRealtime(5f);
        }
    }

    void floatyRotate()
    {
        Debug.Log("Rotating");
        LeanTween.moveY(gameObject, -1.5f, 20f).setEaseInOutQuad().setLoopPingPong();
    }


    public void GenerateIsland()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(bottomWidth, bottomHeight, noiseScale, octaves, persistence, lacunarity);
        MeshData newIsland = IslandGenerator.GenerateIslandMesh(noiseMap);
        GetComponent<IslandDisplay>().DrawMesh(newIsland, placeholder);
    }

    public IEnumerator RandomizeMap()
    {
        yield return StartCoroutine(RemoveAllTiles());
        //LeanTween.rotateAroundLocal(gameObject, new Vector3(0f, 1f), Random.Range(0f, 360f), 0.0001f);
        yield return StartCoroutine(RandomizeTiles());
    }

    public IEnumerator RemoveAllTiles()
    {
        Debug.Log("Starting to remove");
        if (tiles.Count > 0)
        {
            foreach (Tile tile in tiles)
            { 
                    LeanTween.move(tile.gameObject, new Vector3(0f, 20f), 1f).setEaseInBack();
                    yield return new WaitForSecondsRealtime(.1f);
            }
            yield return new WaitForSecondsRealtime(1f);

            foreach (Tile tile in tiles)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(tile.gameObject);

                }
                else
                {
                    Destroy(tile.gameObject);
                }

            }
        }


        tiles.Clear();
        Debug.Log("Remove done");

    }

    public IEnumerator RandomizeTiles()
    {
        // Remove previous map
        islandWidth = initialIslandWidth;
        islandHeight = initialIslandHeight;
        //gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f));

        if (randomSeed != 0)
        {
            Random.InitState(randomSeed);
        }
        else
        {
            Debug.Log("Init random with time");
            Random.InitState(System.DateTime.Now.Millisecond);
        }
        // float topLeftX = (islandWidth - 1) / -2f;
        // float topLeftZ = (islandHeight - 1) / 2f;
        Vector3 currentPos = new Vector3(0f, 0f, 0f);

        int i = 0;
        for (int x = 0; x < islandWidth; ++x)
        {
            for (int y = 0; y < islandHeight; ++y)
            {
                currentPos = GenerateTile(currentPos, x, y, i);
                ++i;
                yield return new WaitForSecondsRealtime(0.1f);
            }
            if (Random.Range(0f, 1f) > 0.5f) --islandHeight;
            if (Random.Range(0f, 1f) > 0.9f) ++islandHeight;
            if (Random.Range(0f, 1f) > 0.8f) --islandWidth;
            if (Random.Range(0f, 1f) > 0.9f) ++islandWidth;
            currentPos = currentPos + new Vector3(3f, 0f);
            currentPos.z = 0f;
            currentPos.z = Random.Range(-6f, 6f);
            currentPos.y = 0;
        }
        Debug.Log("All tiles generated");
        // GenerateElevation();
    }

    void GenerateElevation()
    {
        int i = 0;
        Vector3 currentPos = new Vector3(0, 0);

        for (int x = 0; x < islandWidth; ++x)
        {
            for (int y = 0; y < islandHeight; ++y)
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

        //WaterTile wTile = (tiles.Count > 0 && tiles[i - 1] is WaterTile) ? (WaterTile)tiles[i - 1] : null;
        //if (wTile != null && wTile.direction == WaterTile.WaterTileDirection.N)
        //{
        //    createTileOfType(waterTile, currentPos, x, y, i);
        //    return currentPos = currentPos + new Vector3(0f, 0f, 3f);
        //}

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
        obj.season = season;
        obj.transform.position = currentPosition + new Vector3(0f,-50f);
        obj.RandomizeTile();
        LeanTween.move(obj.gameObject, currentPosition, 2f).setEaseOutQuad();
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
            // @Todo: what to do with this
            // tile.SetNeighbor(Tile.TileDirections.W, tiles[i - islandWidth]);
            //  tiles[i - islandWidth].SetNeighbor(Tile.TileDirections.E, tile);
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
        if (tile.x < 1 || tile.y < 1 || tile.x == islandWidth - 1 || tile.y == islandHeight - 1)
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

            if (Mathf.Abs(neighbor.elevation - diff) > 1)
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

   public void SetSeason(int season)
    {
        Debug.Log("Vuodenaika vaihtui" + season);
        this.season = (Tile.Season)season;
    }


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
        if (GUILayout.Button("Generate island"))
        {
            generator.GenerateIsland();
        }
        if (GUILayout.Button("Randomize tiles"))
        {
            generator.EditorGenerate(10, 10);
        }

    }
}
#endif
