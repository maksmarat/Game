using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMove : MonoBehaviour
{
    public float scrollSpeed = 0.5f;

    public float speed = 10f;
    private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = renderer.material.GetTextureOffset("_MainTex");

        if (offset.y < -1) offset.y++;
        offset.y -= scrollSpeed * Time.deltaTime;

        // Применяем новое смещение
        renderer.material.SetTextureOffset("_MainTex", offset);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.gameObject.GetComponent<CharacterController>();
            controller.Move(transform.forward * -speed * Time.deltaTime);
        }
        else if (other.CompareTag("mobile"))
        {
            other.transform.position += transform.forward * -speed * Time.deltaTime;
        }
    }
}
