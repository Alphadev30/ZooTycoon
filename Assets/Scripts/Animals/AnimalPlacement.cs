using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalPlacement : MonoBehaviour
{
    public GameObject tigerPrefab;
    public GameObject penguinPrefab;
    public GameObject wolfPrefab;
    public GameObject bearPrefab;

    public bool canPlace;

    //public FenceBuilder fenceBuilder; // Reference to the PathBuilder script

    private GameObject selectedAnimalPrefab;

    private void Start()
    {
        canPlace = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            PlaceAnimal();
        }
    }

    private void PlaceAnimal()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Check if the raycast hit a valid placement area or terrain
            if (hit.collider.CompareTag("Terrain") && !IsPointerOverUIElement())
            {
                // Check if the placement area is within the fenced boundaries
                if (IsInsideFence(hit.point))
                {
                    // Instantiate the selected animal prefab at the hit point
                    Instantiate(selectedAnimalPrefab, hit.point, Quaternion.identity);
                }
            }
        }
    }

    /* Check if the mouse pointer is over any UI element*/
    private bool IsPointerOverUIElement()
    {
        // Check if the mouse pointer is over any UI element
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    private bool IsInsideFence(Vector3 point)
    {
        // Consider placement valid by default
        return true;
    }

    public void SelectTiger()
    {
        selectedAnimalPrefab = tigerPrefab;
    }

    public void SelectPenguin()
    {
        selectedAnimalPrefab = penguinPrefab;
    }

    public void SelectWolf()
    {
        selectedAnimalPrefab = wolfPrefab;
    }

    public void SelectBear()
    {
        selectedAnimalPrefab = bearPrefab;
    }

    public void changeCanPlay()
    {
        canPlace = !canPlace;
    }
}
