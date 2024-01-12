using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float maxUpRotation = 150f;
    public float maxDownRotation = -60f;
    public float inversion = 1;
    void Start()
    {
        Camera.main.orthographicSize = Screen.height / 2f;

    }

    public void FixedUpdate()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (PlayerPrefs.HasKey("mouseY_rotationSpeed")) rotationSpeed = PlayerPrefs.GetFloat("mouseY_rotationSpeed", 0f);
        if (PlayerPrefs.HasKey("mouse_inversion")) inversion = PlayerPrefs.GetInt("mouse_inversion", 0);
    }

    void Update()
    {
        float mouseY = inversion * Input.GetAxis("Mouse Y");
        RotateCharacter(mouseY);
    }

    void RotateCharacter (float mouseY)
    {
        // Поворот по оси X
        float currentXRotation = transform.rotation.eulerAngles.x;

        // Преобразуем угол в диапазон -180 до 180 градусов
        currentXRotation = (currentXRotation > 180) ? currentXRotation - 360 : currentXRotation;

        float newRotationX = currentXRotation - mouseY * rotationSpeed;

        // Ограничиваем угол поворота по оси X
        newRotationX = Mathf.Clamp(newRotationX, -maxUpRotation, -maxDownRotation);

        // Применяем новый поворот
        transform.rotation = Quaternion.Euler(newRotationX, transform.rotation.eulerAngles.y, 0f);
    }
}