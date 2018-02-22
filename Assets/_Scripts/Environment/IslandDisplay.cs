using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IslandDisplay : MonoBehaviour {

    public Renderer textureRend;
    public MeshFilter meshFilter;
    public MeshRenderer meshRend;

    public void DrawTexture(Texture2D texture)
    {
        textureRend.sharedMaterial.mainTexture = texture;
        textureRend.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRend.sharedMaterial.mainTexture = texture;
    }
}
