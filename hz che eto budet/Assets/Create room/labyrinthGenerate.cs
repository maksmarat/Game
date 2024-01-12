using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class labyrinthGenerate : MonoBehaviour
{
    [Header("Maze Settings")]
    public int mazeSize = 5;          // Размер лабиринта
    public float roomSize = 5;          // Размер комнаты
    public int passageSize = 2;       // Размер прохода

    [Header("Objects")]
    public Transform player;
    public GameObject roomPrefab;
    public GameObject[] wall;
    public GameObject[] floor;

    private int[,] maze;

    void Start()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.transform.position = new Vector3(mazeSize * roomSize / 2, 3.04f, mazeSize * roomSize / 2);
        player.GetComponent<CharacterController>().enabled = true;

        GenerateMaze();
    }

    void GenerateMaze()
    {
        maze = new int[mazeSize, mazeSize]; // Инициализация массива
        int mazeSizex = mazeSize / 2;
        int mazeSizez = mazeSize / 2;
        maze[mazeSizex, mazeSizez] = 1;

        for (int i = 0; i < mazeSize; i++) 
        { 
            int c = Random.Range(0, 4);
            if (mazeSizex < 1 || mazeSizex + 2 > mazeSize) break;
            if (mazeSizez < 1 || mazeSizez + 2 > mazeSize) break;
            if (c == 0)
            {
                mazeSizex++;
            }
            if (c == 1)
            {
                mazeSizex--;
            }
            if (c == 2)
            {
                mazeSizez--;
            }
            if (c == 3)
            {
                mazeSizez++;
            }
            Debug.Log(mazeSizex);
            Debug.Log(mazeSizez);
            maze[mazeSizex, mazeSizez] = 1;
        }

        for (int x = 0; x < mazeSize; x++)
        {
            for (int z = 0; z < mazeSize; z++)
            {
                if (maze[x, z] == 1)
                {
                    Vector3 position = new Vector3(x * roomSize, 0, z * roomSize);
                    
                    if (x > 0 && maze[x - 1, z] == 1)
                    {
                        wall[0].SetActive(false);
                        floor[0].SetActive(true);
                    }

                    if (x < mazeSize - 1 && maze[x + 1, z] == 1)
                    {
                        wall[1].SetActive(false);
                        floor[1].SetActive(true);
                    }

                    if (z < mazeSize - 1 && maze[x, z + 1] == 1)
                    {
                        wall[2].SetActive(false);
                        floor[2].SetActive(true);
                    }

                    if (z > 0 && maze[x, z - 1] == 1)
                    {
                        wall[3].SetActive(false);
                        floor[3].SetActive(true);
                    }

                    Instantiate(roomPrefab, position, Quaternion.identity);


                    for (int i = 0; i < 4;i++)
                    {
                        wall[i].SetActive(true);
                        floor[i].SetActive(false);
                    }
                }
            }
        }
    }
}
