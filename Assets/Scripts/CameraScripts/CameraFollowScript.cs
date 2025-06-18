using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform player;
    private Vector3 initialOffset;

    void Start()
    {
        if (player != null)
        {
            initialOffset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(-200, player.position.y + initialOffset.y, -10);
            transform.rotation = Quaternion.identity;
        }
    }
}
