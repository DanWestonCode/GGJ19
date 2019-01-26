using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {

    /// <summary>
    /// Nifty little class to descripe a slung "web" 
    /// </summary>
    private class Web {
        public readonly Vector2 start;
        public readonly Vector2 end;

        public Web (Vector2 start, Vector2 end) {
            this.start = start;
            this.end = end;
        }

        public Vector2 Direction () {
            return (end - start).normalized;
        }
    }

    /// <summary>
    /// Basic movement speed
    /// </summary>
    public float speed = 15.0f;

    /// <summary>
    /// Zipline speed
    /// </summary>
    public float zipSpeed = 15.0f;

    /// <summary>
    /// Current direction of the reticle
    /// </summary>
    private Vector2 aimDirection;

    /// <summary>
    /// The current web in progress 
    /// </summary>
    private Web currentWeb = null;

    /// <summary>
    /// Reticle game object reference
    /// </summary>
    public GameObject reticle;

    /// <summary>
    /// 
    /// </summary>
    public Coroutine currentZip;

    private void Awake() {
        Debug.Assert(reticle);
    }

    void Update () {
        Target();
        Shoot();
        Move();
	}

    private void Move() {
        Vector2 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.D)) {
            dir = Vector3.right;
        }

        if (Input.GetKey(KeyCode.A)) {
            dir = Vector3.left;
        }
        
        // Only reconsider a zip when we have a web, the key is pressed and there is no current zipping!
        if (Input.GetKeyDown(KeyCode.W) && currentWeb != null && currentZip == null) {
            Vector2 target = currentWeb.end;
            currentWeb = null;

            //store the current coroutine so we don't start another until it's up!.. IEnumerator kinda sucks, but game jam!
            currentZip = StartCoroutine(IZip(target));
        }

        this.transform.position += (Vector3)dir * speed * Time.deltaTime;
    }

    IEnumerator IZip(Vector2 target) {
        bool loop = true;
        while (loop) {
            transform.position = Vector3.MoveTowards(transform.position, target, zipSpeed * Time.deltaTime);

            if ((Vector2.Distance(this.transform.position, target) > 0.25f)) {
                yield return null;
            }
            else {
                loop = false;
                currentZip = null;
            }
        }
    }

    private void Target () {

        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

        Vector2 facingDirection = worldMousePosition - transform.position;
        float aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f) {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        float x = transform.position.x + 1f * Mathf.Cos(aimAngle);
        float y = transform.position.y + 1f * Mathf.Sin(aimAngle);
        
        var crossHairPosition = new Vector3(x, y, 0);
        reticle.transform.position = crossHairPosition;
    }

    private void Shoot() {
        if (Input.GetMouseButton(0)) {

            // TODO: Check for flies/enemies
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, Mathf.Infinity, LayerMask.GetMask("Floor"));           

            if (hit.collider) {
                Debug.Log(string.Format("Hit {0}", hit.transform.gameObject.name));
                Debug.DrawRay(transform.position, aimDirection * hit.distance, Color.yellow);

                currentWeb = new Web(this.transform.position, hit.point);
            }
            else {
                Debug.DrawRay(transform.position, aimDirection * 10, Color.yellow);
                //currentWeb = null;
            }

        }
    }

  
}
