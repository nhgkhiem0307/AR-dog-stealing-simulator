using UnityEngine;

public class DogSpawner : MonoBehaviour
{
    public GameObject dogPrefab; // Kéo Prefab con chó vào đây
    public float spawnRadius = 8f; // Chó sẽ xuất hiện trong phạm vi tầm 8 mét quanh người chơi
    public float minSpawnRadius = 4f; // Không spawn quá gần mặt người chơi

    private bool isDogAlive = false;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = Camera.main.transform;
        SpawnNewDog();
    }

    void Update()
    {
        // Nếu vì lý do gì đó chó chưa được tạo và không có con nào sống, tạo con mới
        if (!isDogAlive)
        {
            SpawnNewDog();
        }
    }

    public void SpawnNewDog()
    {
        if (dogPrefab == null || playerTransform == null) return;

        // Tính toán góc ngẫu nhiên 360 độ xung quanh người chơi
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        float randomDistance = Random.Range(minSpawnRadius, spawnRadius);

        // Tính tọa độ X và Z dựa trên góc vòng tròn
        float spawnX = playerTransform.position.x + Mathf.Cos(randomAngle) * randomDistance;
        float spawnZ = playerTransform.position.z + Mathf.Sin(randomAngle) * randomDistance;
        
        // Cho chó nằm ở mặt phẳng ngang với người chơi (hoặc tùy chỉnh Y theo mặt sàn AR nếu cần)
        Vector3 spawnPosition = new Vector3(spawnX, playerTransform.position.y - 1f, spawnZ); 

        // Tiến hành tạo chó
        Instantiate(dogPrefab, spawnPosition, Quaternion.identity);
        isDogAlive = true;
    }

    public void OnDogKilled()
    {
        isDogAlive = false;
        // Bạn có thể delay vài giây trước khi spawn con tiếp theo nếu muốn ở đây
    }
}