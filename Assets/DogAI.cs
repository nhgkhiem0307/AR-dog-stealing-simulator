using UnityEngine;
using UnityEngine.UI;

public class DogAI : MonoBehaviour
{
    public int maxHealth = 60;
    private int currentHealth;

    [Header("Movement Settings")]
    public float forwardSpeed = 2f;      // Tốc độ tiến về phía người chơi
    public float zigzagSpeed = 5f;       // Tốc độ lượn qua lượn lại
    public float zigzagWidth = 1.5f;     // Độ rộng của đường ziczac

    [Header("Attack Settings")]
    public int attackDamage = 15;
    public float attackRange = 1.5f;     // Khoảng cách đủ gần để cắn người chơi
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    public Slider hpBar; // Thanh máu World Space treo trên đầu con chó

    private Transform playerTransform;
    private float randomOffset;

    void Start()
    {
        currentHealth = maxHealth;
        // Người chơi chính là Camera trong AR
        playerTransform = Camera.main.transform;
        // Tạo chút ngẫu nhiên để các con chó không lượn ziczac giống hệt nhau
        randomOffset = Random.Range(0f, 100f); 
    }

    void Update()
    {
        if (playerTransform == null) return;

        MoveAndZigzag();
        CheckAttack();
    }

    void MoveAndZigzag()
    {
        // Hướng thẳng tới người chơi (bỏ qua trục Y để chó không bay lên trời)
        Vector3 targetPos = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        Vector3 directionToPlayer = (targetPos - transform.position).normalized;

        // Tính toán hướng vuông góc để làm đường đi ziczac
        Vector3 sidewaysDirection = Vector3.Cross(directionToPlayer, Vector3.up).normalized;

        // Công thức Ziczac mượt mà dùng Sin
        float zigzagOffset = Mathf.Sin(Time.time * zigzagSpeed + randomOffset) * zigzagWidth;

        // Di chuyển tổng hợp: Tiến lên + Lướt ngang
        Vector3 movement = directionToPlayer * forwardSpeed * Time.deltaTime + sidewaysDirection * zigzagOffset * Time.deltaTime;
        
        transform.position += movement;

        // Luôn xoay mặt nhìn về phía người chơi
        transform.LookAt(targetPos);
    }

    void CheckAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            // Gây sát thương cho người chơi
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
                Debug.Log("Chó cắn! Gâu gâu!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (hpBar != null) hpBar.value = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Thông báo cho Spawner biết con chó này đã oẹo để spawn con khác
        FindObjectOfType<DogSpawner>().OnDogKilled();
        Destroy(gameObject);
    }
}