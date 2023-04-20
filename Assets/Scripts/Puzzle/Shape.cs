using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public float rotationSpeed = 2f; // 200f / 3
    public float flipSpeed = 200f;
    public float moveSpeed = 5f;

    void Start()
    {
        Time.timeScale = 1f;
        Debug.Log("Timescale is" + Time.timeScale);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            RotateLeft();
        }
        if (Input.GetKey(KeyCode.G))
        {
            RotateRight();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flip();
        }
        Move();
    }

    public void RotateLeft()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void RotateRight()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    public void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
    }

    public void Move()
{
    float horizontal = -Input.GetAxis("Horizontal");
    float vertical = -Input.GetAxis("Vertical");

    Vector3 movement = new Vector3(horizontal, vertical, 0);
    transform.position += movement * moveSpeed * Time.deltaTime;
}
}
