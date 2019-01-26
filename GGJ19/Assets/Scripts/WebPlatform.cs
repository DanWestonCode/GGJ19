using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebPlatform : MonoBehaviour {

    public Transform start, end;

    public Vector3 Direction () {
        return (start.position - end.position).normalized;
    }
}