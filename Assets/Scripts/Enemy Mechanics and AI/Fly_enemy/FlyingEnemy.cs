using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFlyingEnemy : MonoBehaviour
{
    public float speed = 2f; // speed of our enemy
    public float noiseScale = 0.5f; 
    public Vector2 moveRange = new Vector2(5f, 3f); // size area

    private Vector3 origin; //center point area
    private float seedX, seedY; // the lenght and widht of area movement

    private void Start()
    {
        origin = transform.position; // start position - center area
        seedX = Random.Range(0f, 100f);
        seedY = Random.Range(0f, 100f);
    }

    /// <summary>
    /// function update, who calculates new position from axis X, and axis Y
    /// it calculates this new coordinat for special function "Perlin Noise"
    /// this function is available in the library "Mathf"
    /// and using the clamp function we change the coordinates to the new ones
    /// </summary>
    private void Update()
    {
        // Get a chaotic direction from the Perlin noise
        float x = (Mathf.PerlinNoise(seedX, Time.time * noiseScale) - 0.5f) * 2;
        float y = (Mathf.PerlinNoise(seedY, Time.time * noiseScale) - 0.5f) * 2;

        Vector3 direction = new Vector3(x, y, 0).normalized;
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        //Restrict movement within the area
        newPosition.x = Mathf.Clamp(newPosition.x, origin.x - moveRange.x, origin.x + moveRange.x);
        newPosition.y = Mathf.Clamp(newPosition.y, origin.y - moveRange.y, origin.y + moveRange.y);

        transform.position = newPosition;
    }

    //  Visualization of the movement area in the Unity
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        //set center area (if enemy have start position)
        Vector3 center = Application.isPlaying ? origin : transform.position;

        Gizmos.DrawWireCube(center, new Vector3(moveRange.x * 2, moveRange.y * 2, 0));
    }
}




