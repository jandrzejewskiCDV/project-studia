using System;
using UnityEngine;

public class Roll : MonoBehaviour
{
    public Vector3 vec3;

    private GameObject player;
    private FloorTrigger floorTrigger;
    private Vector3 lastPosition;
    private bool storedPosition = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        floorTrigger = player.GetComponent<FloorTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.Instance.isRunning())
        {
            return;
        }

        if (floorTrigger.IsPrimaryFloor(this.gameObject))
        {
            if (!storedPosition)
            {
                this.lastPosition = transform.position;
                storedPosition = true;
            }
            else
            {
                Vector3 lastPosition = this.lastPosition; //get the old position
                this.lastPosition = transform.position; //update the last position with new position

                float distance = Vector3.Distance(lastPosition, this.lastPosition); //calculate difference between old position and new position
                GameState.Instance.addDistance(distance);
            }
        }

        transform.position += (vec3 * GameState.Instance.getAdditionalSpeed() * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FloorRemover"))
        {
            floorTrigger.OnFloorRemove(gameObject);
            Destroy(gameObject);
        }
    }
}
