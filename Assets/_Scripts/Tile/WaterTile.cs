using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : Tile
{

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

    // Use this for initialization
    void Start()
    {
        RandomizeTile();
    }


    void onEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void RandomizeTile()
    {
        if (neighbors[2] is WaterTile)
        {
            tile = Instantiate(variations[(int)WaterTileDirection.S], transform);
            return;
        }
        tile = Instantiate(variations[(int)WaterTileDirection.N], transform);

    }
}
