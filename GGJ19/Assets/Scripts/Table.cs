using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour {
    private void OnTriggerEnter2D (Collider2D col) {

        if (col.GetComponent<Spider>()) {
            Spider spider = col.GetComponent<Spider>();

            if (spider.victim != null) {
                spider.victim.AtTable();
                spider.victim.transform.position = this.transform.position;
                spider.victim = null;
                FMODUnity.RuntimeManager.PlayOneShot("event:/Flappy Fly/Fly_Struggle");

            }
        }

    }

}
