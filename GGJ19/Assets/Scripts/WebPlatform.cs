using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebPlatform : MonoBehaviour {

    public Transform a, b;

    public Vector3 Direction () {
        return (a.position - b.position).normalized;
    }
}