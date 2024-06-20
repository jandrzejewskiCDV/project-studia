using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void Start()
    {
        transform.position = player.position + offset;
    }
}
