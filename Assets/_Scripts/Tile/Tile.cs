using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[SelectionBase]
[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    public Season season = Season.SUMMER;

    public int elevation;
    public int x;
    public int y;

    //Materials
    public Color matColor;
    public List<Material> seasonMaterials = new List<Material>(4);

    public List<GameObject> variations = new List<GameObject>();
    public List<GameObject> SummerFoliage = new List<GameObject>();
    public List<GameObject> FallFoliage = new List<GameObject>();
    public List<GameObject> WinterFoliage = new List<GameObject>();
    public List<GameObject> SpringFoliage = new List<GameObject>();
    public TileStates state = TileStates.DEFAULT;
    public TileTypes type;

    // Graphical representation
    public GameObject tile;

    protected MeshRenderer meshRenderer;

    public enum Season
    {
        SUMMER,
        FALL,
        WINTER,
        SPRING
    }

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

    void Awake()
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


    /// <summary>
    /// Custom override color
    /// </summary>
    /// <param name="color">Color to input</param>
    public void setColor(Color color)
    {
        tile.GetComponent<MeshRenderer>().materials[1].color = color;
    }

    /// <summary>
    /// Randomizes tile properties
    /// </summary>
    public virtual void RandomizeTile()
    {
        tile = Instantiate(variations[Random.Range(0, variations.Count)], transform);
        meshRenderer = tile.GetComponent<MeshRenderer>();
        GameObject feature;
        switch (season)
        {

            case Season.SUMMER:
                feature = Instantiate(SummerFoliage[Random.Range(0, SummerFoliage.Count)], tile.transform);
                SetMaterial(seasonMaterials[(int)Season.SUMMER]);
                SetFeaturePosition(feature);
                break;
            case Season.FALL:
                // @Fixme: fall foliage
                feature = Instantiate(SummerFoliage[Random.Range(0, SummerFoliage.Count)], tile.transform);
              //  feature = Instantiate(FallFoliage[Random.Range(0, FallFoliage.Count)], tile.transform);
                SetMaterial(seasonMaterials[(int)Season.FALL]);
                SetFeaturePosition(feature);
                break;
            case Season.WINTER:
                // @Fixme: winter foliage
                feature = Instantiate(SummerFoliage[Random.Range(0, SummerFoliage.Count)], tile.transform);
               // feature = Instantiate(WinterFoliage[Random.Range(0, WinterFoliage.Count)], tile.transform);
                SetMaterial(seasonMaterials[(int)Season.WINTER]);
                SetFeaturePosition(feature);
                break;
            case Season.SPRING:
                // @Fixme: spring foliage
                feature = Instantiate(SummerFoliage[Random.Range(0, SummerFoliage.Count)], tile.transform);
               // feature = Instantiate(SpringFoliage[Random.Range(0, SpringFoliage.Count)], tile.transform);
                SetMaterial(seasonMaterials[(int)Season.SPRING]);
                SetFeaturePosition(feature);
                break;
        }
        matColor = meshRenderer.materials[1].color;

    }

    public virtual void SetMaterial(Material material)
    {
        if (meshRenderer == null) return;
        Material[] mats = meshRenderer.materials;
        mats[1] = material;
        meshRenderer.materials = mats;
    }

    /// <summary>
    /// Spawn position
    /// @Todo: make sure spawn is actually inside of the square
    /// </summary>
    /// <param name="feature"></param>
    private void SetFeaturePosition(GameObject feature)
    {
        // Make sure it's inside the tile
        feature.transform.position += new Vector3(-3f, 0.2f, -3f) + new Vector3(Random.Range(0.7f, 2.7f), 0f, Random.Range(0.7f, 2.7f));
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
