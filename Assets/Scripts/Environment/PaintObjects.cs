using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintObjects : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject rocksPrefab;
    public GameObject shrubsPrefab;
    public GameObject treePrefab;

    private string[] objectTags = { "Grass", "Rocks", "Shrubs", "Tree" };
    private int[] objectCounts = { 4, 1, 2, 1 };
    private float placementRadius = 1.0f; // Adjust this value as per your requirement

    public bool canPaint = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canPaint)
        {
            PlaceObjects();
        }
    }

    private void PlaceObjects()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 placementPoint = hit.point;

            for (int i = 0; i < objectTags.Length; i++)
            {
                string selectedTag = objectTags[i];
                GameObject selectedPrefab = GetPrefabByTag(selectedTag);

                if (selectedPrefab != null && !IsOverlapping(placementPoint))
                {
                    Instantiate(selectedPrefab, placementPoint, Quaternion.identity);
                }

                // Update the placement point for the next object
                placementPoint += Vector3.right * placementRadius;
            }
        }
    }

    private GameObject GetPrefabByTag(string tag)
    {
        switch (tag)
        {
            case "Grass":
                return grassPrefab;
            case "Rocks":
                return rocksPrefab;
            case "Shrubs":
                return shrubsPrefab;
            case "Tree":
                return treePrefab;
            default:
                return null;
        }
    }

    private bool IsOverlapping(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point, placementRadius);

        // Check if any colliders with the specified tags are overlapping
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Grass") || collider.CompareTag("Rocks") || collider.CompareTag("Shrubs") || collider.CompareTag("Tree") || collider.CompareTag("Fence") || collider.CompareTag("Path"))
            {
                return true; // Overlapping object found
            }
        }

        return false; // No overlapping objects found
    }
}
