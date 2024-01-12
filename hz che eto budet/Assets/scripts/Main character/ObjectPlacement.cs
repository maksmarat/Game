using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] prefabToPlace;
    public GameObject[] prefabToPreview;

    public int prefabNumber = 0;

    [Header("LayerMask")]

    public LayerMask groundLayer;
    private bool isPreviewMode = false;
    private GameObject previewObject;

    [Header("Variables")]

    public float installationAccuracy = 2.5f;
    // Задаем интервал времени для обновления предпросмотра
    public float updateInterval = 1.0f;
    private float timer = 0.0f;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TogglePlacementMode();
        }

        if (isPreviewMode)
        {
            // Обновляем таймер
            timer += Time.deltaTime;

            // Проверяем, прошло ли достаточно времени для обновления предпросмотра
            if (timer >= updateInterval)
            {
                UpdatePreviewPosition();

                // Сбрасываем таймер
                timer = 0.0f;
            }

            // Добавляем проверку для отмены предпросмотра по нажатию Escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelPlacement();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ClearPreview();

                if (prefabNumber < prefabToPlace.Length - 1)
                    prefabNumber++;
                else
                    prefabNumber = 0;
                ShowPreview();

            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                previewObject.transform.Rotate(0, 90, 0);
            }
        }
    }

    void TogglePlacementMode()
    {
        isPreviewMode = !isPreviewMode;

        if (isPreviewMode)
        {
            // Включаем предпросмотр
            ShowPreview();
        }
        else
        {
            // Выключаем предпросмотр
            ClearPreview();
        }
    }

    void ShowPreview()
    {
        if (prefabToPlace != null)
        {
            previewObject = Instantiate(prefabToPreview[prefabNumber]);
            previewObject.name = "PreviewObject";
        }
    }

    void ClearPreview()
    {
        // Удаляем предпросмотр объекта
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    void UpdatePreviewPosition()
    {
        if (prefabToPlace != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                if (previewObject != null)
                {
                    float roundedX = Mathf.Round(hit.point.x / installationAccuracy) * installationAccuracy;
                    float roundedY = hit.point.y + prefabToPlace[prefabNumber].transform.localScale.y / 2;
                    float roundedZ = Mathf.Round(hit.point.z / installationAccuracy) * installationAccuracy;

                    previewObject.transform.position = new Vector3(roundedX, roundedY, roundedZ);
                }
            }
        }
    }

    void PlaceObject()
    {
        if (prefabToPlace != null)
        {
            if (previewObject != null)
            {
                // Устанавливаем объект на позицию предпросмотра
                prefabToPlace[prefabNumber].SetActive(true);
                Instantiate(prefabToPlace[prefabNumber], previewObject.transform.position, previewObject.transform.rotation);
                prefabToPlace[prefabNumber].SetActive(false);

                // Выключаем предпросмотр и чистим его
                isPreviewMode = false;
                ClearPreview();
            }
        }
    }

    void CancelPlacement()
    {
        isPreviewMode = false;
        ClearPreview();
    }
}
