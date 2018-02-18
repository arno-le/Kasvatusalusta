using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class TreeTile : Tile {

	// Use this for initialization
	void Start () {
       if (state == TileStates.DEFAULT) RandomizeTile();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
