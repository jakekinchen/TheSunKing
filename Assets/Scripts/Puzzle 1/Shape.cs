using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public float flipSpeed = 200f;

    public void Rotate()
    {
         transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    public void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
    }
}