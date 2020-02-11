using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllable : MonoBehaviour
{
    Renderer my_renderer;
    // Start is called before the first frame update
    void Start()
    {
        my_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void Select()
    {
        my_renderer.material.color = Color.red;
    }

    internal void Unselect()
    {
        my_renderer.material.color = Color.white;
    }

    internal void MoveTo(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, position.z);
    }

    internal void ScaleUp(float scale)
    {
        transform.localScale += new Vector3(1, 1, 1) * scale;
    }

    internal void ScaleDown(float scale)
    {
        if ((transform.lossyScale - new Vector3(1, 1, 1) * scale).x > 0)
            transform.localScale -= new Vector3(1, 1, 1) * scale;
    }
}
