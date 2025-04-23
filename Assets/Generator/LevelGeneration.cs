using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms; //index 0 --> LR, index 1 --> LRB, index 2 --> LRT, index 3 --> LRTB
    public GameObject[] firstRooms;   // Assign in Inspector Ч start rooms
    public GameObject[] endRooms;     // Assign in Inspector Ч end rooms

    private int direction;
    public float moveAmount;

    [SerializeField] private CinemachineVirtualCamera vcam;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    public float minX;
    public float maxX;
    public float minY;
    public bool stopGeneration;

    public GameObject endRoomPrefab; // assign in Inspector

    public LayerMask room;

    private int downCounter;
    public GameObject playerPrefab; // assign in Inspector

    void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        Vector2 spawnPos = startingPositions[randStartingPos].position;
        transform.position = spawnPos;

        // ≤нстанц≥юй стартову к≥мнату й збережи посиланн€ на нењ
        int randFirst = Random.Range(0, firstRooms.Length);
        GameObject startRoom = Instantiate(firstRooms[randFirst], spawnPos, Quaternion.identity);
        //if (randFirst == 2 ) { randFirst++;};
        //startRoom = Instantiate(rooms[randFirst], spawnPos, Quaternion.identity);

        // —пробуй знайти об'Їкт ≥з тегом "PlayerSpawn" всередин≥ стартовоњ к≥мнати
        Transform spawnPoint = startRoom.transform.Find("Square");

        // јЅќ (альтернатива через тег, €кщо вищий вар≥ант не працюЇ)
        if (spawnPoint == null)
        {
            GameObject tagged = GameObject.FindGameObjectWithTag("PlayerSpawn");
            if (tagged != null)
                spawnPoint = tagged.transform;
        }

        // якщо знайдено точку спавну Ч спавнимо гравц€ там
        if (spawnPoint != null)
        {
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

            // ѕризначаЇмо камер≥ гравц€ €к об'Їкт дл€ Follow
            if (vcam != null)
            {
                vcam.Follow = player.transform;
            }
        }
        else
        {
            Debug.LogError("Spawn point not found! Make sure 'Square' exists in start room.");
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

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 2;
                }
                else if ( direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
            
        } else if (direction == 3 || direction == 4) //Move LEFT
        {
            if (transform.position.x > minX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
            
        } else if (direction == 5) //Move DOWN
        {
            downCounter++;

            if(transform.position.y > minY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                if(roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {
                    if(downCounter >= 2)
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();

                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                    }
                    
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
            } 
            else
            {
                // STOP GENERATION //
                stopGeneration = true;

                // Instantiate exit room at current position
                int randEnd = Random.Range(0, endRooms.Length);
                Instantiate(endRooms[randEnd], transform.position, Quaternion.identity);
            }

        }
    }
}
