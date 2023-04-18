using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 1;
    public GameObject[] levels;

    [SerializeField] public Transform levelParent;
    public GameObject currentLevelInstance;

    

    
    public void Start()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int level)
    {
        if (level > levels.Length)
        {
            Debug.Log("You have completed all levels!");
            return;
        }

        // Disable the previous level instance if it exists
        if (currentLevelInstance != null)
        {
            currentLevelInstance.SetActive(false);
        }

        // Instantiate the new level prefab and set it as the active instance
        GameObject levelPrefab = Instantiate(levels[level - 1], levelParent);
        levelPrefab.SetActive(true);
        currentLevelInstance = levelPrefab;
    }

    public void LevelComplete()
    {
        // Increment level and load the next level
        currentLevel++;
        LoadLevel(currentLevel);
    }
}
