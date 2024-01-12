using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectsGenerate : MonoBehaviour
{
    public Transform player;
    public float activationDistance = 100f;

    [Header("Decoration")]
    public GameObject[] decorationPrefab;
    public GameObject[] logicPrefab;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= activationDistance)
        {
            GenerateObjects();
        }
    }

    void GenerateObjects()
    {
        Vector3 position = transform.position;
        for (int i = 0; i < decorationPrefab.Length; i++)
        {
            for (int c = 0; c < Random.Range(0, 5); c++)
            {
                Vector3 decorationPosition = new Vector3(Random.Range(-50f, 50f), 1 + decorationPrefab[i].transform.localScale.y / 2, Random.Range(-50f, 50f));
                Instantiate(decorationPrefab[i], position + decorationPosition, Quaternion.identity);
            }

        }

        Vector3 logicPrefabPosition = new Vector3(Random.Range(-35f, 35f), 1 + logicPrefab[0].transform.localScale.y / 2, Random.Range(-35f, 35f));
        int f = Random.Range(1, 4) * 2 + 1;
        for (int i = 0; i < f; i++)
        {
            for (int c = 0; c < f; c++)
            {
                Instantiate(logicPrefab[Random.Range(2, logicPrefab.Length)], position + logicPrefabPosition + new Vector3(i * 4, 0, c * 4), Quaternion.identity);
            }
        }
        // Генерация входа
        GameObject b = Instantiate(logicPrefab[0], position + logicPrefabPosition + new Vector3((f - 1) * 2 + Mathf.Pow(-1, Random.Range(1, 3)) * (f + 1) * 2, 0, Random.Range(0, f) * 4), Quaternion.identity);
        //поворот входа по y
        b.transform.LookAt(position + logicPrefabPosition);
        float currentRotation = b.transform.eulerAngles.y;
        float roundedRotation = Mathf.Round(currentRotation / 180.0f) * 180.0f;
        b.transform.eulerAngles = new Vector3(b.transform.eulerAngles.x, 270 + roundedRotation, b.transform.eulerAngles.z);
        // Генерация выхода
        b = Instantiate(logicPrefab[1], position + logicPrefabPosition + new Vector3(Random.Range(0, f) * 4, 0, (f - 1) * 2 + Mathf.Pow(-1, Random.Range(1, 3)) * (f + 1) * 2), Quaternion.identity);
        //поворот выхода по y
        b.transform.LookAt(position + logicPrefabPosition);
        currentRotation = b.transform.eulerAngles.y;
        roundedRotation = Mathf.Round(currentRotation / 180.0f) * 180.0f;
        b.transform.eulerAngles = new Vector3(b.transform.eulerAngles.x, 180 + roundedRotation, b.transform.eulerAngles.z);
        Destroy(this);
    }
}
