using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms; //index 0 --> LR, index 1 --> LRB, index 2 --> LRT, index 3 --> LRTB
    public GameObject[] endRooms;     // Assign in Inspector — end rooms

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

        // Інстанціюй стартову кімнату й збережи посилання на неї
        GameObject startRoom = Instantiate(rooms[2], spawnPos, Quaternion.identity);

        GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        if (vcam != null && player != null)
        {
            vcam.Follow = player.transform;
        }


        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if (timeBtwRoom <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (direction == 1 || direction == 2) //Move RIGHT
        {
            if (transform.position.x < maxX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                Collider2D roomCheck = Physics2D.OverlapCircle(transform.position, 1f, room);
                if (roomCheck == null)
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
        else if (direction == 3 || direction == 4) //Move LEFT
        {
            if (transform.position.x > minX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                Collider2D roomCheck = Physics2D.OverlapCircle(transform.position, 1f, room);
                if (roomCheck == null)
                {
                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);
                }

                direction = Random.Range(3, 6);
            }
            else direction = 5;
        }
        else if (direction == 5) //Move DOWN
        {
            downCounter++;

            if (transform.position.y > minY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1f, room);

                // === Перевірка, чи поточна кімната має верхній прохід ===
                if (roomDetection != null)
                {
                    RoomType currentRoom = roomDetection.GetComponent<RoomType>();
                    if (currentRoom.type != 1 && currentRoom.type != 3)
                    {
                        // Якщо вже не має верхнього отвору — заміни на LRTB
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(rooms[3], transform.position, Quaternion.identity); // LRTB
                    }
                }

                // === Перехід вниз ===
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                Collider2D roomBelow = Physics2D.OverlapCircle(transform.position, 1f, room);
                if (roomBelow == null)
                {
                    int rand = Random.Range(2, 4); // LRT або LRTB
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);
                }

                direction = Random.Range(1, 6);
            }
            else
            {
                stopGeneration = true;

                if (coinSpawner != null)
                {
                    coinSpawner.SpawnCoinsAcrossMap(); // Назви метод як зручно
                }

                int randEnd = Random.Range(0, endRooms.Length);
                Instantiate(endRooms[randEnd], transform.position, Quaternion.identity);

                leftGate.OpenAndClose();
                rightGate.OpenAndClose();
            }
        }
    }

}
