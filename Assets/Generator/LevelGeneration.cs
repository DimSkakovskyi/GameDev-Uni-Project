using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms; //index 0 --> LR, index 1 --> LRB, index 2 --> LRT, index 3 --> LRTB
    public GameObject[] endRooms;     // Assign in Inspector � end rooms

    public bool regenStar;
    public SmartCoinSpawner coinSpawner;

    private int direction;
    public float moveAmount;


    public BorderGate leftGate;
    public BorderGate rightGate;

    [SerializeField] private CinemachineVirtualCamera vcam;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    public float minX;
    public float maxX;
    public float minY;
    public bool stopGeneration;

    public Transform playerSpawnPoint;

    public LayerMask room;

    private int downCounter;
    public GameObject playerPrefab; // assign in Inspector

    void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        Vector2 spawnPos = startingPositions[randStartingPos].position;
        transform.position = spawnPos;

        GameObject startRoom = Instantiate(rooms[3], spawnPos, Quaternion.identity);

        GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        if (vcam != null && player != null)
        {
            vcam.Follow = player.transform;
        }

        direction = Random.Range(1, 6);
    }

    private void Update()
    { if (regenStar == false)
        {
            if (timeBtwRoom <= 0 && !stopGeneration)
            {
                Move();
                timeBtwRoom = startTimeBtwRoom;
            }
            else
            {
                timeBtwRoom -= Time.deltaTime;
            }
        }
    }

    private void Move()
    {
        if (direction == 1 || direction == 2) // Move RIGHT
        {
            if (transform.position.x < maxX)
            {
                downCounter = 0;
                transform.position += Vector3.right * moveAmount;

                if (!Physics2D.OverlapCircle(transform.position, 1f, room))
                {
                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);
                }

                direction = Random.Range(1, 6);
                if (direction == 3) direction = 2;
                else if (direction == 4) direction = 5;
            }
            else direction = 5;
        }
        else if (direction == 3 || direction == 4) // Move LEFT
        {
            if (transform.position.x > minX)
            {
                downCounter = 0;
                transform.position += Vector3.left * moveAmount;

                if (!Physics2D.OverlapCircle(transform.position, 1f, room))
                {
                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);
                }

                direction = Random.Range(3, 6);
            }
            else direction = 5;
        }
        else if (direction == 5) // Move DOWN
        {
            downCounter++;

            if (transform.position.y > minY)
            {
                Vector2 previousPos = transform.position; // ← зберігаємо поточну позицію перед рухом

                // Знаходимо кімнату на цій попередній позиції
                Collider2D previousRoom = Physics2D.OverlapCircle(previousPos, 1.5f, room);
                RoomType roomType = previousRoom?.GetComponentInParent<RoomType>();

                if (roomType == null)
                {
                    Debug.LogWarning("❗ No RoomType found at: " + previousPos);
                }
                else if (roomType.type != 4 && roomType.type != 2)
                {
                    if (downCounter >= 2)
                    {
                        roomType.RoomDestruction();
                        Instantiate(rooms[4], previousPos, Quaternion.identity); // LRTB
                    }
                    else
                    {
                        roomType.RoomDestruction();
                        int randRoomDownOpening = Random.Range(2, 5); // LRT or LRTB
                        if (randRoomDownOpening == 3)
                            randRoomDownOpening = 2;

                        Instantiate(rooms[randRoomDownOpening], previousPos, Quaternion.identity);
                    }
                }

                // Тільки тепер зміщуємо позицію вниз
                transform.position += Vector3.down * moveAmount;

                // І спавнимо кімнату, в яку рухаємось (з TOP входом)
                int randRoom = Random.Range(3, 5); // LRT or LRTB
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
            }
            else
            {
                stopGeneration = true;

                if (coinSpawner != null)
                {
                    coinSpawner.SpawnCoinsAcrossMap();
                }

                int randEnd = Random.Range(0, endRooms.Length);
                Instantiate(endRooms[randEnd], transform.position, Quaternion.identity);

                leftGate.OpenAndClose();
                rightGate.OpenAndClose();
            }
        }

    }

    public void RegenerateLevel()
    {
        regenStar = true;
        
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
                Destroy(gameObject);
        }

        // Очистити ворогів
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        // Знайти і перемістити гравця
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerSpawnPoint.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // зупинити інерцію
        }
        else
        {
            Debug.LogWarning("Player not found when regenerating level!");
        }

        // Генерація початкової кімнати
        stopGeneration = false;
        transform.position = startingPositions[Random.Range(0, startingPositions.Length)].position;
        direction = Random.Range(1, 6);

        downCounter = 0;
        timeBtwRoom = 0;

        GameObject startRoom = Instantiate(rooms[3], transform.position, Quaternion.identity);

        regenStar = false;
    }


}

