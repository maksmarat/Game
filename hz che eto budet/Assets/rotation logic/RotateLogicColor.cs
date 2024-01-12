using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLogicColor : MonoBehaviour
{
    public string targetTag = "logicRotate";

    public int connectionNumber = 999;
    private bool disconnection = false;
    public Color targetColor = Color.blue;
    public Color originalColor = Color.white;

    private void Awake()
    {
    }

    private void OnTriggerExit(Collider other)
    {
        // Проверяем, если объект с определенным тегом выходит из триггера
        if (other.CompareTag(targetTag))
        {
            if (other.GetComponent<RotateLogicColor>().connectionNumber < connectionNumber)
            {
                GameObject[] b = GameObject.FindGameObjectsWithTag(targetTag);

                for (int i = 0; i < b.Length; i++)
                {
                    if (b[i].GetComponent<RotateLogicColor>().connectionNumber > connectionNumber)
                    {
                        b[i].GetComponent<RotateLogicColor>().connectionNumber = 999;

                        foreach (Renderer childRenderer in b[i].GetComponentsInChildren<Renderer>())
                        {
                            childRenderer.material.color = originalColor;
                        }
                    }
                }

                connectionNumber = 999;

                foreach (Renderer childRenderer in GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = originalColor;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (other.GetComponent<RotateLogicColor>().connectionNumber < connectionNumber)
            {
                connectionNumber = other.GetComponent<RotateLogicColor>().connectionNumber + 1;

                foreach (Renderer childRenderer in GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = targetColor;
                }
            }
        }
    }
}