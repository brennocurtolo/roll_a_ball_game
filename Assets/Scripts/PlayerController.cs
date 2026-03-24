using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    //Texts
    public TextMeshProUGUI GameEndTextObject; // Reference to the UI text element that displays the lose message.
    
    //Rigidbody
    private Rigidbody rb; // Rigidbody of the player.

    //Score
    public TextMeshProUGUI scoreText; // Reference to the UI text element that displays the score.
    private int score = 0;
    private int totalPickUps = 0;

    //Movement
    public float speed = 0;
    private float movementX;
    private float movementY;

    //Jump
    public float jumpForce = 7f; // Force applied to the player when jumping.

    //Ground
    public LayerMask groundLayer; // Define which layers are considered ground
    private bool isGrounded = true; // Variable to check if the player is on the ground, allowing jumping.
    private float groundCheckDistance = 1.0f; // Distance for ground detection

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        totalPickUps = GameObject.FindGameObjectsWithTag("PickUp").Length; // Conta todos os pickups na cena
        SetScoreText();
        GameEndTextObject.gameObject.SetActive(false); // Hide the lose text at the start of the game.
    }

    void OnMove(InputValue movementValue)
    {
        // UPDATE-type function — intention (input / logic) 
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue jumpValue)
    {
        if (jumpValue.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply an upward force to the player for jumping.
            isGrounded = false; // Set isGrounded to false when the player jumps.
        }
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
        if(score >= totalPickUps)
        {
            GameEndTextObject.gameObject.SetActive(true); // Show the win text when all pickups are collected.
            GameEndTextObject.text = "YOU WIN!"; // Set the win message text

            // Destroy all enemy game objects when the player wins.
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    void FixedUpdate()
    {
        // FIXEDUPDATE-type function — physics execution

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Move the player based on input
        rb.AddForce(movement * speed);
    }

    void Update()
    {
        CheckGround(); // Check if player is grounded every frame
    }

    void CheckGround()
    {
        // Small offset upward to avoid detecting itself
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        // Raycast down to detect ground
        isGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GameEndTextObject.gameObject.SetActive(true); // Show the lose text when colliding with an enemy.
            GameEndTextObject.text = "YOU LOSE!"; // Set the lose message text

            Destroy(gameObject); // Destroy the player game object when colliding with an enemy.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false); // Disable the game object that the player collides with
            score++; // Increment the player's score
            SetScoreText(); // Update the score text on the UI
        }
    }
}