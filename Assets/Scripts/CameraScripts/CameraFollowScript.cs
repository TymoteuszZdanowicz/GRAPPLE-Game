using UnityEngine;

/// <summary>
/// Makes the camera follow the player with a fixed X and Z, and keeps the initial Y offset.
/// </summary>
public class CameraFollowScript : MonoBehaviour
{
    public Transform player;         // Reference to the player transform
    private Vector3 initialOffset;   // Initial offset between camera and player

    // Store the initial offset at the start
    void Start()
    {
        if (player != null)
        {
            initialOffset = transform.position - player.position;
        }
    }

    // Update camera position after all movement is done
    void LateUpdate()
    {
        if (player != null)
        {
            // Follow player on Y axis, keep fixed X and Z
            transform.position = new Vector3(-200, player.position.y + initialOffset.y, -10);
            transform.rotation = Quaternion.identity;
        }
    }
}