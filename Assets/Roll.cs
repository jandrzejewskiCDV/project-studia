using UnityEngine;

public class Roll : MonoBehaviour
{
    public Vector3 vec3;

    private GameObject player;
    private FloorTrigger floorTrigger;
    private Vector3 lastPosition;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        floorTrigger = player.GetComponent<FloorTrigger>();
        lastPosition = Vector3.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameState.Instance.isRunning())
        {
            return;
        }

        if (floorTrigger.IsPrimaryFloor(gameObject))
        {
            var lastPosition = this.lastPosition; //get the old position
            this.lastPosition = transform.position; //update the last position with new position

            if (lastPosition != Vector3.zero)
            {
                var distance = Vector3.Distance(lastPosition, this.lastPosition); //calculate difference between old position and new position
                GameState.Instance.addDistance(distance);
            }
        }

        transform.position += vec3 * (GameState.Instance.getAdditionalSpeed() * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("FloorRemover")) return;
        floorTrigger.OnFloorRemove(gameObject);
        Destroy(gameObject);
    }
}
