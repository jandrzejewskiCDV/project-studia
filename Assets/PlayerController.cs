using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 200f;

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
        Vector3 velocity = Vector3.zero;
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
        }else if (other.gameObject.CompareTag("Coin"))
        {
            GameState.Instance.addCoin();
            Destroy(other.gameObject);
            Debug.Log(GameState.Instance.getCoins());
        }
    }
}
