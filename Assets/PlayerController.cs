using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    
    public int damage = 20;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    public Slider playerHPBar; // Kéo Slider UI máu của người chơi vào đây
    private Camera arCamera;

    void Start()
    {
        currentHealth = maxHealth;
        arCamera = Camera.main;
        UpdateUI();
    }

    void Update()
    {
        // Kiểm tra nếu người chơi chạm màn hình để bắn
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
        }
        
        // Test trên Máy tính bằng phím Space hoặc Click chuột trái
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Tạo một tia Ray bắn thẳng từ tâm giữa màn hình (0.5, 0.5)
        Ray ray = arCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Nếu bắn trúng Chó
            if (hit.collider.CompareTag("Dog"))
            {
                DogAI dog = hit.collider.GetComponent<DogAI>();
                if (dog != null)
                {
                    dog.TakeDamage(damage);
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateUI();
        if (currentHealth <= 0)
        {
            Debug.Log("Bạn đã bị chó cắn cạn máu! Game Over.");
            // Xử lý hiệu ứng Thua cuộc ở đây
        }
    }

    void UpdateUI()
    {
        if (playerHPBar != null)
        {
            playerHPBar.value = (float)currentHealth / maxHealth;
        }
    }
}