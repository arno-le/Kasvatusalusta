using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ClickAction(Vector3 point);
    public EntityManager entityManager;
    
    // Update is called once per frame
    void Update()
    {
        checkRaycasts();
    }

    void checkRaycasts()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.Log(ray);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.gameObject.GetComponent<GroundTile>().interact() == Tile.TileStates.SHAPED)
                {
                    Debug.Log("Soil was shaped");
                }
            }
        }

    }


   public void advanceDay()
    {
        entityManager.growPlants();
    }



}
