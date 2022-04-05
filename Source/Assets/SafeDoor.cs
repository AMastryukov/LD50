using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeDoor : Interactable
{
    bool opened = false;
    public override void Interact()
    {
        if (!opened)
        {
            transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -120, 0), 1);
            opened = true;
        }
        
    }

}
