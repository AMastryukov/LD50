using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : Interactable
{
    bool opened = false;

    float max = 20f;
    float min = 10f;

    private void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
    }
    public override void Interact()
    {
        if (!opened)
        {
            GetComponent<Rigidbody>().useGravity = true;
            float x = UnityEngine.Random.Range(min, max);
            float y = UnityEngine.Random.Range(min, max);
            float z = UnityEngine.Random.Range(min, max);
            GetComponent<Rigidbody>().AddForce(new Vector3(x, y, z));
            opened = true;
        }

    }
}
