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
        public readonly WebPlatform target;


        public Web (Vector2 start, Vector2 end, WebPlatform target) {
            this.start = start;
            this.end = end;

            this.target = target;
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
    /// Will be set to true when landed during zip
    /// </summary>
    private bool landed = false;

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

    /// <summary>
    /// Coroutine for current line renderer sling
    /// </summary>
    public Coroutine currentSling;

    private WebPlatform currentPlatform;

    /// <summary>
    /// The current platform orientation
    /// </summary>
    private Vector2 currentOrientation = Vector2.zero;

    public LineRenderer web;

    private void Awake() {
        Debug.Assert(reticle);
        Debug.Assert(web);
    }

    void Update () {
        Target();
        Shoot();
        Move();
	}

    private void Move() {
        Vector2 dir = Vector2.zero;

        // Only reconsider a zip when we have a web, the key is pressed and there is no current zipping!
    
        if (Input.GetKey(KeyCode.D) && (currentOrientation.x <= -1.0f || currentOrientation.x >= 1.0f)) {
            dir = Vector3.right;

            if ((this.transform.position + (Vector3)dir * speed * Time.deltaTime).x >= (this.currentPlatform.end.position.x) + (this.currentPlatform.end.GetComponent<SpriteRenderer>().bounds.size.x*.5f)) {
                dir = Vector2.zero;
            }
        }

        if (Input.GetKey(KeyCode.A) && (currentOrientation.x <= -1.0f || currentOrientation.x >= 1.0f)) { 
            dir = Vector3.left;

            if ((this.transform.position + (Vector3)dir * speed * Time.deltaTime).x <= this.currentPlatform.start.position.x - (this.currentPlatform.end.GetComponent<SpriteRenderer>().bounds.size.x * .5f)) {
                dir = Vector2.zero;
            }
        }
                    
        if (Input.GetKey(KeyCode.W) && (currentOrientation.y >= 1.0f || currentOrientation.y <= -1.0f)) {
            dir = Vector3.up;

            if ((this.transform.position + (Vector3)dir * speed * Time.deltaTime).y >= this.currentPlatform.end.position.y - (this.currentPlatform.end.GetComponent<SpriteRenderer>().bounds.size.y * .5f)) {
                dir = Vector2.zero;
            }
         }

        if (Input.GetKey(KeyCode.S) && (currentOrientation.y >= 1.0f || currentOrientation.y <= -1.0f)) {
            dir = Vector3.down;

            if ((this.transform.position + (Vector3)dir * speed * Time.deltaTime).y <= this.currentPlatform.start.position.y + (this.currentPlatform.start.GetComponent<SpriteRenderer>().bounds.size.y * .5f)) {
                dir = Vector2.zero;
            }
        }

        this.transform.position += (Vector3)dir * speed * Time.deltaTime;
    }

    IEnumerator IZip(Vector2 target) {
        bool loop = true;

        while (loop) {
            transform.position = Vector3.MoveTowards(transform.position, target, zipSpeed * Time.deltaTime);

            if ((Vector2.Distance(this.transform.position, target) > 0.25f)) {
                yield return null;
            } else {
                Debug.Log("Setting vars to false");

                currentPlatform = currentWeb.target;             

                loop = false;
                currentWeb = null;
                currentZip = null;

                yield return true;
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
        float x = transform.position.x + .5f * Mathf.Cos(aimAngle);
        float y = transform.position.y + .5f * Mathf.Sin(aimAngle);
        
        var crossHairPosition = new Vector3(x, y, 0);
        reticle.transform.position = crossHairPosition; 
    }

    private void Shoot() {
        // left click
        if (Input.GetMouseButtonDown(0)) {

            // TODO: Check for flies/enemies    
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, 3, LayerMask.GetMask("Floor"));

            web.startColor = Color.red;
            web.startColor = Color.red;

            web.SetWidth(0.1f, 0.1f);
            web.positionCount = 2;
            web.SetPosition(0, this.transform.position);

            if (hit.collider) {
                Debug.Log(string.Format("Hit {0}", hit.transform.gameObject.name));
                Debug.DrawRay(transform.position, aimDirection * hit.distance, Color.yellow);

                // draw sling to platform
                web.SetPosition(1, hit.point);

                // grab the platform we're aiming at
                WebPlatform nextPlatform = hit.transform.gameObject.GetComponent<WebPlatform>();

                // can't zip to same platform... this causes issues...
                if (currentWeb == null && nextPlatform != null && nextPlatform != currentPlatform) {

                    currentWeb = new Web(this.transform.position, hit.point, hit.transform.gameObject.GetComponent<WebPlatform>());

                    Vector2 target = currentWeb.end;
                    currentOrientation = currentWeb.target.Direction();
                    //store the current coroutine so we don't start another until it's up!.. IEnumerator kinda sucks, but game jam!
                    currentZip = StartCoroutine(IZip(target));
                }
            }
            else {
                Debug.Log("No Hit");
                Debug.DrawRay(transform.position, aimDirection * 3, Color.yellow);
                web.SetPosition(1, this.transform.position + (Vector3)(aimDirection * 3));
            }

        } else {
            // stop slinging that web        
            web.positionCount = 0;
        }
    }
   
}
