using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBuilder : MonoBehaviour
{
    public GameObject fencePrefab;
    public LayerMask fenceLayerMask;

    private List<Vector3> fencePoints = new List<Vector3>();

    private bool isDrawingFence = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawingFence();
        }
        else if (Input.GetMouseButton(0))
        {
            ContinueDrawingFence();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            FinishDrawingFence();
        }
    }

    private void StartDrawingFence()
    {
        isDrawingFence = true;
        fencePoints.Clear();
    }

    public bool IsInsideFencedArea(Vector3 point)
    {
        return true;
    }

    private void ContinueDrawingFence()
    {
        if (!isDrawingFence)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, fenceLayerMask))
        {
            Vector3 fencePoint = hit.point;
            fencePoint.y = 0f;
            fencePoints.Add(fencePoint);
        }
    }

    private void FinishDrawingFence()
    {
        if (!isDrawingFence)
            return;

        isDrawingFence = false;

        // Check if the fence is a valid enclosure
        if (IsFenceValidEnclosure())
        {
            // Build the fence and create the enclosure
            BuildFence();
            CreateEnclosure();
        }
        else
        {
            // Invalid enclosure, clear the fence points
            fencePoints.Clear();
        }
    }

    private bool IsFenceValidEnclosure()
    {
        // Check if the fence forms a closed shape
        if (fencePoints.Count < 3)
            return false;

        // Check if the first and last fence points are close enough to consider it closed
        float distanceThreshold = 1f;
        if (Vector3.Distance(fencePoints[0], fencePoints[fencePoints.Count - 1]) > distanceThreshold)
            return false;

        // Additional checks for a valid enclosure can be performed here if needed

        return true;
    }

    private void BuildFence()
    {
        // Instantiate the fence GameObject and set its positions based on the fence points
        for (int i = 0; i < fencePoints.Count - 1; i++)
        {
            Vector3 startPoint = fencePoints[i];
            Vector3 endPoint = fencePoints[i + 1];

            Vector3 fenceDirection = endPoint - startPoint;
            float fenceLength = fenceDirection.magnitude;
            Quaternion fenceRotation = Quaternion.LookRotation(fenceDirection);

            Instantiate(fencePrefab, startPoint, fenceRotation)
                .transform.localScale = new Vector3(fenceLength, 1f, 1f);
        }
    }

    private void CreateEnclosure()
    {
        // Create the enclosure logic here based on the fenced area
        // This could involve defining a collider or updating a data structure representing the enclosure
        // You can implement the logic specific to your game's requirements
        Debug.Log("Enclosure created!");
    }
}
