using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : Tile
{
    public WaterTileDirection direction;
    public enum WaterTileDirection
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

    void Awake()
    {
    }

    public override void RandomizeTile()
    {
        if (neighbors[2] is WaterTile)
        {
            tile = Instantiate(variations[(int)WaterTileDirection.S], transform);
            meshRenderer = tile.GetComponentInChildren<MeshRenderer>();
            direction = WaterTileDirection.S;
            Debug.Log("Water tile direction set to" + direction);
            return;
        }
        else
        {
            tile = Instantiate(variations[(int)WaterTileDirection.N], transform);
            direction = WaterTileDirection.N;
        }
        switch (season)
        {

            case Season.SUMMER:
                SetMaterial(seasonMaterials[(int)Season.SUMMER]);
                break;
            case Season.FALL:
                SetMaterial(seasonMaterials[(int)Season.FALL]);
                break;
            case Season.WINTER:
                SetMaterial(seasonMaterials[(int)Season.WINTER]);
                break;
            case Season.SPRING:
                SetMaterial(seasonMaterials[(int)Season.SPRING]);
                break;
        }
    }

    public override void SetMaterial(Material material)
    {
        if (meshRenderer == null) return;
        Material[] mats = meshRenderer.materials;
        mats[2] = material;
        meshRenderer.materials = mats;
    }

}
