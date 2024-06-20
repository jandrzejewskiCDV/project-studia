using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 200f;
    public AudioSource coinPickupSound;
    public AudioSource deathSound;
    public AudioSource backgroundSound;

    private Rigidbody rigidBody;
    private float horizontal;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = -Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        var velocity = Vector3.zero;
        velocity += (transform.right * horizontal); //Strafe
        velocity *= movementSpeed * Time.fixedDeltaTime; //Framerate and speed adjustment
        velocity.y = rigidBody.velocity.y;

        rigidBody.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameState.Instance.die();
            Destroy(gameObject);
            deathSound.Play();
            backgroundSound.Stop();
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            GameState.Instance.addCoin();
            Destroy(other.gameObject);
            coinPickupSound.Play();
        }
    }
}