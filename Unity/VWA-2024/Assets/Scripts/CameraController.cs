using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // camera distance
    public float tileSize = 1f;   // size of each tile
    public int tilesInView = 10;  // number of tiles visible in the camera's view
    private Camera mainCamera;


    // camera movement
    public Transform player;
    public float followSpeed = 5f;
    public float panSpeed = 1f;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;
    private Vector3 cameraOffset;
    private bool isPanning = false;

    void Start()
    {
        mainCamera = Camera.main;
        UpdateCamera();
        cameraOffset = transform.position - player.position;
    }
    /*
    void Update()
    {
        UpdateCamera();

        transform.position = Vector3.Lerp(transform.position, player.position + cameraOffset, followSpeed * Time.deltaTime);

        if (!isPanning && (player.position.x < minX || player.position.x > maxX || player.position.y < minY || player.position.y > maxY))
        {
            isPanning = true;
            StartCoroutine(PanCamera());
        }
    }
    */

    void Update()
    {
        UpdateCamera();

        if (!isPanning)
        {
            // Check if the player is outside the boundaries
            if (player.position.x < minX || player.position.x > maxX || player.position.y < minY || player.position.y > maxY)
            {
                isPanning = true;
                StartCoroutine(PanCamera());
            }
            else
            {
                // Lerp the camera to follow the player
                transform.position = Vector3.Lerp(transform.position, player.position + cameraOffset, followSpeed * Time.deltaTime);
            }
        }
    }


    void UpdateCamera()
    {
        float orthoSize = ((float)tilesInView / 2f) * tileSize;  // calculate the size of the orthographic camera based on the number of tiles visible
        mainCamera.orthographicSize = orthoSize;  // set the camera's orthographic size to match the calculated size
    }


    /*
    IEnumerator PanCamera()
    {
        float startTime = Time.time;
        Vector3 startOffset = cameraOffset;
        Vector3 targetOffset = new Vector3(Mathf.Clamp(player.position.x, minX, maxX), Mathf.Clamp(player.position.y, minY, maxY), cameraOffset.z) - player.position;

        while (Time.time < startTime + panSpeed && isPanning)
        {
            cameraOffset = Vector3.Lerp(startOffset, targetOffset, (Time.time - startTime) / panSpeed);
            yield return null;
        }

        isPanning = false;
    }
    */

    IEnumerator PanCamera()
    {
        float startTime = Time.time;
        Vector3 startOffset = cameraOffset;
        Vector3 targetOffset = new Vector3(Mathf.Clamp(player.position.x, minX, maxX), Mathf.Clamp(player.position.y, minY, maxY), cameraOffset.z) - player.position;

        while (Time.time < startTime + panSpeed)
        {
            cameraOffset = Vector3.Lerp(startOffset, targetOffset, (Time.time - startTime) / panSpeed);
            yield return null;
        }

        // Ensure the camera is exactly at the target position
        cameraOffset = targetOffset;

        // Reset the flag after panning is complete
        isPanning = false;
    }
}

