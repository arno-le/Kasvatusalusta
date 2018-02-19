using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[SelectionBase]
public class Tile : MonoBehaviour
{
    public int elevation;
    public int x;
    public int y;
    public Color matColor;
    public List<GameObject> variations = new List<GameObject>();
    public List<GameObject> foliage = new List<GameObject>();
    public TileStates state = TileStates.DEFAULT;
    public TileTypes type;

    // Graphical representation
    public GameObject tile;


    public enum TileStates
    {
        DEFAULT,
        SHAPED,
        PLANTED
    }

    public enum TileTypes
    {
        GROUND,
        TREE,
        ROCK
    }

    public enum TileDirections
    {
        N,
        E,
        S,
        W
    }

    public List<Tile> neighbors = new List<Tile>(3);

    void Start()
    {
    }

    void Update()
    {

    }

    public void setCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void setElevation(float elevation)
    {
        transform.position = transform.position + new Vector3(0, elevation, 0);
        this.elevation = (int)(elevation / TileConstants.heightStep);
    }

   
    public void setColor()
    {
        Gradient g = new Gradient();
        GradientColorKey[] gck = new GradientColorKey[2];
        GradientAlphaKey[] gak = new GradientAlphaKey[2];
        gck[0].color = Color.red;
        gck[0].time = 1.0F;
        gck[1].color = Color.blue;
        gck[1].time = -1.0F;
        gak[0].alpha = 0.0F;
        gak[0].time = 1.0F;
        gak[1].alpha = 0.0F;
        gak[1].time = -1.0F;
        g.SetKeys(gck, gak);
        tile.GetComponent<MeshRenderer>().materials[1].color = g.Evaluate(1);
    }

    public virtual void RandomizeTile()
    {
        tile = Instantiate(variations[Random.Range(0, variations.Count)], transform);
        matColor = tile.GetComponent<MeshRenderer>().materials[1].color;
        GameObject feature = Instantiate(foliage[Random.Range(0, foliage.Count)], tile.transform);

        // Make sure it's inside the tile
         feature.transform.position += new Vector3(-3f, 0.2f, -3f) + new Vector3(Random.Range(0.5f, 2.5f), 0f, Random.Range(0.5f, 2.5f));
         feature.transform.Rotate(new Vector3(0f, 1f), Random.Range(0f, 359f));

    }

    public virtual TileStates interact()
    {
        return state;
    }

    public void SetNeighbor(TileDirections direction, Tile tile)
    {
        neighbors[(int)direction] = tile;
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Tile tile = (Tile)target;
        if (GUILayout.Button("Set color"))
        {
            Gradient g = new Gradient();
            GradientColorKey[] gck = new GradientColorKey[2];
            GradientAlphaKey[] gak = new GradientAlphaKey[2];
            gck[0].color = Color.red;
            gck[0].time = 1.0F;
            gck[1].color = Color.blue;
            gck[1].time = -1.0F;
            gak[0].alpha = 0.0F;
            gak[0].time = 1.0F;
            gak[1].alpha = 0.0F;
            gak[1].time = -1.0F;
            g.SetKeys(gck, gak);
            tile.GetComponent<MeshRenderer>().materials[1].color = g.Evaluate(1);
        }

    }
}
#endif
