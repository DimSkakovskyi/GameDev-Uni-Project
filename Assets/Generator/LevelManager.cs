using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject levelGeneratorPrefab;

    public void GenerateNextLevel()
    {
        // Destroy all current rooms and player
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
            Destroy(obj);
        Destroy(GameObject.FindGameObjectWithTag("Player"));

        // Spawn new LevelGeneration
        Instantiate(levelGeneratorPrefab, Vector3.zero, Quaternion.identity);
    }
}

