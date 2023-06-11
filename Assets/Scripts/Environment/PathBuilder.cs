using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* 
- The script is responsible for placing footpaths or fences based on the value of the isFootPath variable.
- It allows the player to place objects on the terrain by clicking on it. The placement is constrained by the terrain layer.
- The script supports deletion of path nodes by clicking on them while the canDelete flag is true.
- It includes the ability to modify the position of existing path nodes by clicking and dragging them while the canModify flag is true.
- The path visualization is created using small cube or cylinder objects placed along the path nodes at a specified spacing.
*/

public class PathBuilder : MonoBehaviour
{
    public bool isFootPath; // Tells whether to lay footpath or fence
    public GameObject[] pathNodePrefab; // Prefab for the path node GameObject
    public LayerMask terrainLayerMask; // Layer mask to detect the terrain
    public GameObject[] pathVisualPrefab; // Prefab for the path visual GameObject
    public float spacing = 1.3f; // Spacing between the small cube or cylinder GameObjects

    private List<GameObject> pathNodes = new List<GameObject>(); // Stores references to the instantiated path node GameObjects
    private GameObject pathVisual; // Reference to the path visual GameObject

    int holder = 0; // Index to decide to lay footpath or fence, here 0 = footpath, 1 = fence;

    public bool canPlace; // player will be able to place prefabs only when canPlace is true
    public bool canDelete; // player will be able to delete groundobj only when canDelete is true
    public bool canModify; // player will be able to modify groundobj only when canModify is true

    private GameObject selectedPathNode; // for modification of path

    private void Start()
    {
        canPlace = false;
        canDelete = false;
        canModify = false;
        holder = isFootPath ? 0 : 1;
        spacing = isFootPath ? 1f : 1.3f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartNewFencePlacement();
        }

        if (Input.GetMouseButtonDown(1))
        {
            canPlace = false;
            canDelete = false;
            canModify = false;
        }

        deleteNodeVisual();
        placeNodes();
    }

    /* Checks for mouse input to delete a path or fence node from the scene.
       It raycasts from the mouse position and disables the hit object if it has the "groundobj" tag and canDelete is true. */
    public void deleteNodeVisual()
    {
        // Disable the path/fence from the scene
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Check if the raycast hit a path node
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.tag == "groundobj" && canDelete)
                {
                    // Remove the hit path node from the list and disable it
                    pathNodes.Remove(hitObject);
                    hitObject.SetActive(false);
                }
            }
        }
    }

    /* Handles the placement and modification of path or fence nodes. 
       Checks for mouse input and raycasts from the mouse position. 
       If canModify is true, it selects the clicked node for modification. 
       If canPlace is true, it adds a new path or fence node at the clicked position. 
       If a node is selected, it moves the node to the mouse position.*/
    public void placeNodes()
    {
        if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Check if the raycast hit a path node
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.tag == "groundobj" && canModify)
                {
                    // Set the selected path node
                    selectedPathNode = hitObject;
                }
                else if (canPlace)
                {
                    // Add a new path node at the clicked position
                    Vector3 clickPosition = hit.point;
                    AddPathNode(clickPosition);
                }
            }
        }

        // Move the selected path node
        if (selectedPathNode != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Update the position of the selected path node
                selectedPathNode.transform.position = hit.point;
                UpdatePathVisual();
            }

            // Release the selected path node when the mouse button is released
            if (Input.GetMouseButtonUp(0))
            {
                selectedPathNode = null;
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

    /* Adds a new path or fence node at the specified position. 
       It raycasts from the position to find the terrain surface and uses the hit point as the new position. 
       It instantiates a new path or fence node GameObject, adds it to the pathNodes list, and calls UpdatePathVisual() */
    private void AddPathNode(Vector3 position)
    {
        // Perform a raycast to find the terrain surface at the given position
        if (Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out RaycastHit hit, Mathf.Infinity, terrainLayerMask))
        {
            position = hit.point; // Use the hit point as the new position on the terrain
        }

        GameObject pathNode = Instantiate(pathVisualPrefab[holder], position, Quaternion.identity);
        pathNodes.Add(pathNode);

        UpdatePathVisual();
    }

    /* Updates the visual representation of the path or fence. 
       It destroys the existing path visual if it exists, instantiates a new path visual GameObject,
       and positions the small cube or cylinder GameObjects along the path between the nodes. */
    private void UpdatePathVisual()
    {
        // Destroy the existing path visual if it exists
        if (pathVisual != null)
        {
            Destroy(pathVisual);
        }

        // Create a new path visual GameObject
        pathVisual = Instantiate(pathVisualPrefab[holder]);

        // Add the path nodes as child objects to the path visual and position them along the path
        for (int i = 0; i < pathNodes.Count - 1; i++)
        {
            GameObject currentPathNode = pathNodes[i];
            GameObject nextPathNode = pathNodes[i + 1];

            // Calculate the direction between the current and next path nodes
            Vector3 direction = (nextPathNode.transform.position - currentPathNode.transform.position).normalized;

            // Calculate the distance between the current and next path nodes
            float distance = Vector3.Distance(currentPathNode.transform.position, nextPathNode.transform.position);

            // Calculate the number of small cube or cylinder GameObjects to place along the path
            int numObjects = Mathf.CeilToInt(distance / spacing);

            // Place the small cube or cylinder GameObjects along the path between the nodes
            for (int j = 0; j < numObjects; j++)
            {
                // Generates a position that is a fraction of the distance between the two path nodes
                float t = (j + 1) / (float)numObjects;
                Vector3 position = Vector3.Lerp(currentPathNode.transform.position, nextPathNode.transform.position, t);

                // Instantiate the fence GameObject
                GameObject visualObject = Instantiate(pathNodePrefab[holder], position, Quaternion.identity);

                // Set the rotation of the fence GameObject based on the direction between the nodes
                visualObject.transform.LookAt(nextPathNode.transform);
                visualObject.transform.Rotate(Vector3.up, 90f);

                visualObject.transform.SetParent(pathVisual.transform);
            }
        }
    }

    /* Clears the pathNodes list and creates a new path visual GameObject. Called when starting a new fence placement. */
    private void StartNewFencePlacement()
    {
        // Clear the path nodes list
        pathNodes.Clear();

        // Create a new path visual GameObject
        pathVisual = Instantiate(pathVisualPrefab[holder]);
    }

    // Function to change the value of isFootPath, which is accessed by UI Button
    public void ChangeToFootPath()
    {
        isFootPath = true;
        holder = isFootPath ? 0 : 1;
        spacing = isFootPath ? 1f : 1.3f;
        canPlace = true;
        StartNewFencePlacement();
    }

    // Function to change the value of isFootPath, which is accessed by UI Button
    public void ChangeToFence()
    {
        isFootPath = false;
        holder = isFootPath ? 0 : 1;
        spacing = isFootPath ? 1f : 1.3f;
        canPlace = true;
        StartNewFencePlacement();
    }

    public void changeCanDelete()
    {
        canDelete = !canDelete;
        canPlace = false;
        canModify = false;
    }

    public void changeCanModify()
    {
        canModify = !canModify;
        canDelete = false;
        canPlace = false;
        StartNewFencePlacement();
    }
}
