using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCameraController : MonoBehaviour
{
    public float movementSpeed = 7f;
    public float horizontalSpeed = 5f;
    public float zoomSpeed = 7f;
    public float minZoomDistance = 7f;
    public float maxZoomDistance = 26f;

    private bool isMoving;
    private Vector3 lastMousePosition;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Check if right Tab is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMoving = true;
            lastMousePosition = Input.mousePosition;
        }

        // Check if right Tab is released
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isMoving = false;
        }

        // Handle camera movement while right mouse button is held down
        if (isMoving)
        {
            // Move the camera based on mouse delta movement
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            float mouseDeltaX = mouseDelta.x;
            float mouseDeltaY = mouseDelta.y;

            // Move forward/backward and strafe left/right with WASD keys
            float verticalMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
            float horizontalMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;

            transform.Translate(new Vector3(horizontalMovement, 0f, verticalMovement));

            // Move horizontally with Q/E keys
            float horizontalTranslation = Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;
            transform.Translate(new Vector3(horizontalTranslation, 0f, 0f));

            // Rotate the camera based on mouse delta movement
            transform.RotateAround(transform.position, Vector3.up, mouseDeltaX * horizontalSpeed * Time.deltaTime * 1.25f);
            transform.RotateAround(transform.position, transform.right, -mouseDeltaY * horizontalSpeed * Time.deltaTime * 1.25f);
        }

        // Handle camera zoom with scroll wheel
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scrollWheelInput * zoomSpeed * Time.deltaTime;
        float newZoomDistance = Vector3.Distance(transform.position, Camera.main.transform.position) + zoomAmount;
        newZoomDistance = Mathf.Clamp(newZoomDistance, minZoomDistance, maxZoomDistance);
        Vector3 zoomDirection = transform.forward * zoomAmount;
        transform.position += zoomDirection;

        lastMousePosition = Input.mousePosition;
    }
}
