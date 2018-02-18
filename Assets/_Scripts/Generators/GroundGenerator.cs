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

    void Start()
    {
        RandomizeMap();
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
        }
        Debug.Log(i);
    }

    Vector3 GenerateTile(Vector3 currentPos, int x, int y, int i)
    {
        // TODO: instantiate tile types based on neighbors
        // For now, only the last generated applies
        float chances = Random.Range(0f, 1f);
        Tile obj;
        //  Debug.Log(chances);
        // TODO: clear this
        if (chances < forestProbability)
        {
            obj = Instantiate(groundTile, transform);
            obj.transform.position = currentPos;
            entityManager.addTile(obj, x, y, i);
        }
        else if ( forestProbability < chances && chances < waterProbability)
        {
            obj = Instantiate(treeTile, transform);
            obj.transform.position = currentPos;
            entityManager.addTile(obj, x, y, i);
        } else if (chances > waterProbability)
        {
            obj = Instantiate(waterTile, transform);
            obj.transform.position = currentPos;
            entityManager.addTile(obj, x, y, i);
        }

        return currentPos = currentPos + new Vector3(0f, 0f, 3f);
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
