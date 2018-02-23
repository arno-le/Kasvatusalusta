using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class GroundTile : Tile
{

    public GameObject shapedTile;
    public GameObject plant; // Hardcoded
    public GameObject wateringEffect;
    public float speed = 2.0f;


    void Awake()
    {
    }

    public override TileStates interact()
    {
        switch (state)
        {
            case TileStates.DEFAULT:
                StartCoroutine("shapeCoroutine");
                return state;
            case TileStates.SHAPED:
                plantSeeds();
                return state;
            case TileStates.PLANTED:
                return state;
            default:
                return base.interact();
        }
    }

    private IEnumerator shapeCoroutine()
    {
        state = TileStates.SHAPED;
        Transform particleTransform = new GameObject().transform;
        particleTransform.position = transform.position + new Vector3(0f, 2f);
        GameObject obj = GameObject.Instantiate(wateringEffect, particleTransform);
        yield return StartCoroutine(waitUntilWatered(obj.GetComponent<ParticleSystem>()));
        Destroy(obj);
    }


    private IEnumerator waitUntilWatered(ParticleSystem water)
    {
        float progress = 0f;
        while (water.isPlaying)
        {
            progress = progress + Time.deltaTime * speed;
            //  rend.material.Lerp(defaultMaterial, shapedMaterial, progress);
            yield return new WaitForEndOfFrame();
        }

    }


    private void plantSeeds()
    {
        state = TileStates.PLANTED;
        GameObject planted = Instantiate(plant, transform);
        GameObject.FindObjectOfType<EntityManager>().addPlant(planted);
    }




}
