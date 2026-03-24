using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Reference to the player object.
    private Vector3 offset; // Distance between the camera and the player.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - player.transform.position; // Calculate the initial offset.
    }

    // Update is called once per frame
    void LateUpdate() // Every Frame after all Update functions have been called
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset; // Update the camera's position to follow the player.
        }
    }
}
