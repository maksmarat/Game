using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLines : MonoBehaviour
{

    void Start()
    {
        FindObjectOfType<GK.PowerBox>().CalculateBoundingBox();

    }

    private void FixedUpdate()
    {
        GameObject closestObject = FindClosestObjectWithTag("PowerLines");

        if (closestObject != null)
        {
            CreateWire(transform.position, closestObject.transform.position);
        }
    }

    GameObject FindClosestObjectWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestObject = null;
        float closestDistance = 100;

        foreach (GameObject obj in objectsWithTag)
        {
            if (obj != gameObject) // Исключаем текущий объект из рассмотрения
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }
        }

        return closestObject;
    }

    void CreateWire(Vector3 start, Vector3 end)
    {
        GameObject wire = new GameObject("Wire");
        LineRenderer lineRenderer = wire.AddComponent<LineRenderer>();

        // Настройки линии
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Установка точек начала и конца линии
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        Destroy(this);
    }
}
