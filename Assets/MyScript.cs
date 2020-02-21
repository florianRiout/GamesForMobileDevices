using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScript : MonoBehaviour
{
    GameObject[] cubes;
    // Start is called before the first frame update
    void Start()
    {
        cubes = GameObject.FindGameObjectsWithTag("Cube");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowHide()
    {
        for(int i = 0; i < cubes.Length; i++)
        {
            cubes[i].SetActive(!cubes[i].activeSelf);
        }
    }
}
