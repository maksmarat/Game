using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public LayerMask interactableMask;
    UnityEvent onInteract;

    [Header("Audio Settings")]
    public AudioClip takeClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RaycastHit hit;

        // Проверяем, сталкивается ли луч с объектом
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 20, interactableMask))
        {
            GameObject otherGameObject = hit.collider.gameObject;
            onInteract = otherGameObject.GetComponent<InteractableObject>().onInteract;
            // Если объект содержит компонент Interactable, выполняем взаимодействие
            if (Input.GetKeyDown(KeyCode.E) && onInteract != null)
            {
                onInteract.Invoke();
                audioSource.PlayOneShot(takeClip);
            }
        }
    }
}
