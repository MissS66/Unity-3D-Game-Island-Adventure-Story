using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;

    public TMP_Text countText;
    public TMP_Text resultText;
    public GameObject gameOverPanel;

    public int totalPickups = 10;

    private int count = 0;
    private bool isGameOver = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        UpdateCountText();

        if (resultText != null)
            resultText.text = "";

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isGameOver) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0.0f, moveZ);
        rb.AddForce(movement * speed);
    }

    private void Update()
    {
        if (!isGameOver && transform.position.y < -3f)
        {
            FailGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            UpdateCountText();

            if (count >= totalPickups)
            {
                WinGame();
            }
        }
    }

    void UpdateCountText()
    {
        if (countText != null)
            countText.text = "Count: " + count;
    }

    void WinGame()
    {
        isGameOver = true;
        if (resultText != null)
            resultText.text = "WIN!";
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    void FailGame()
    {
        isGameOver = true;
        if (resultText != null)
            resultText.text = "LOSE!!!";
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}