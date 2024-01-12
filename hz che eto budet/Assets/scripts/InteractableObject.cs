using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public UnityEvent onInteract;

    [Header("Door")]
    public GameObject doorleft;
    public GameObject doorright;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator Door_Open()
    {
        for (int i = 0; i < 900; i++)
        {
            doorleft.transform.Rotate(-0.1f, 0, 0);
            doorright.transform.Rotate(0.1f, 0, 0);
            yield return new WaitForSeconds(0.003f);
        }
    }

    public void Interactable_Destroy()
    {
        Destroy(gameObject);
    }

    public void Interactable_Door_Open()
    {
        Destroy(gameObject.GetComponent<BoxCollider>());
        StartCoroutine(Door_Open());
    }

    public void Interactable_Rotate_y(float rotate)
    {
        transform.Rotate(0, rotate, 0);
    }

}
