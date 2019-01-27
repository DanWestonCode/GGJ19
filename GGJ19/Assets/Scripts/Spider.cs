using System.Collections;
using UnityEngine;

public class Spider : MonoBehaviour {

    /// <summary>
    /// Nifty little class to descripe a slung "web" 
    /// </summary>
    private class Web {
        public readonly Vector2 start;
        public readonly Vector2 end;
        public readonly WebPlatform target;


        public Web(Vector2 start, Vector2 end, WebPlatform target) {
            this.start = start;
            this.end = end;

            this.target = target;
        }

        public Vector2 Direction() {
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
    /// How long the spider web is
    /// </summary>
    float slingDist = 25.0f;

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
    /// The current zip routine
    /// </summary>
    public Coroutine currentZip;

    /// <summary>
    /// The Current reel routine
    /// </summary>
    public Coroutine currentReel;

    /// <summary>
    /// The platform which the spider is currently on
    /// </summary>
    private WebPlatform currentPlatform;

    /// <summary>
    /// The current platform orientation
    /// </summary>
    private Vector2 currentOrientation = Vector2.zero;

    /// <summary>
    /// LineRenderer used to Spiders web
    /// </summary>
    public LineRenderer web;

    /// <summary>
    /// The current fly on the spiders back
    /// </summary>
    public Fly victim;

    /// <summary>
    /// Setup in Awake, the layers which the spiders web will consider.
    /// </summary>
    private LayerMask raycastMask;

    /// <summary>
    /// Reference to Animator on spider
    /// </summary>
    public Animator animator;

    /// <summary>
    /// SpriteRenderer for spider
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// Reference transform where 
    /// flies will be carried
    /// </summary>
    public Transform spiderBackpack;

    private enum State {
        Idle, 
        Walking, 
        Shooting, 
        Flying
    };

    private enum RotState {
        normalHorizontal,
        InvertHorizontal,
        normalVertical, 
        invertVertical
    };

    State state;
    RotState rotState;

    private void Awake() {
        Debug.Assert(reticle != null);
        Debug.Assert(web!= null);
        Debug.Assert(animator != null);
        Debug.Assert(spriteRenderer != null);
        
        raycastMask = LayerMask.GetMask("Floor");
        raycastMask |= LayerMask.GetMask("Fly");

        state = State.Idle;
        rotState = RotState.normalHorizontal;
    }
    void Update() {   
        Target();
        Shoot();
        Move();
    }

    private void Move() {
        Vector2 dir = Vector2.zero;

        // stop movement whilst zipping
        if (currentZip == null) {

            if (Input.GetKey(KeyCode.D) && (currentOrientation.x <= -1.0f || currentOrientation.x >= 1.0f)) {
                dir = Vector3.right;

                if (rotState == RotState.normalHorizontal) {
                    // horizontal 
                    spriteRenderer.flipX = true;
                } else {
                    spriteRenderer.flipX = false;
                }

                if ((this.transform.position + (Vector3)dir * speed * Time.deltaTime).x >= (this.currentPlatform.end.position.x) + (this.currentPlatform.end.GetComponent<SpriteRenderer>().bounds.size.x * .5f)) {
                    dir = Vector2.zero;
                }
            }

            if (Input.GetKey(KeyCode.A) && (currentOrientation.x <= -1.0f || currentOrientation.x >= 1.0f)) {
                dir = Vector3.left;

                if (rotState == RotState.normalHorizontal) {
                    // horizontal 
                    spriteRenderer.flipX = false;
                }
                else {
                    spriteRenderer.flipX = true;
                }

                if ((this.transform.position + (Vector3)dir * speed * Time.deltaTime).x <= this.currentPlatform.start.position.x - (this.currentPlatform.end.GetComponent<SpriteRenderer>().bounds.size.x * .5f)) {
                    dir = Vector2.zero;
                }
            }

            if (Input.GetKey(KeyCode.W) && (currentOrientation.y >= 1.0f || currentOrientation.y <= -1.0f)) {
                dir = Vector3.up;

                if (rotState == RotState.normalVertical) {
                    spriteRenderer.flipX = true;
                }
                else {
                    spriteRenderer.flipX = false;
                }

                if ((this.transform.position + (Vector3)dir * speed * Time.deltaTime).y >= this.currentPlatform.end.position.y - (this.currentPlatform.end.GetComponent<SpriteRenderer>().bounds.size.y * .5f)) {
                    dir = Vector2.zero;
                }
            }

            if (Input.GetKey(KeyCode.S) && (currentOrientation.y >= 1.0f || currentOrientation.y <= -1.0f)) {
                dir = Vector3.down;

                if (rotState == RotState.normalVertical) {
                    spriteRenderer.flipX = false;
                }
                else {
                    spriteRenderer.flipX = true;
                }

                if ((this.transform.position + (Vector3)dir * speed * Time.deltaTime).y <= this.currentPlatform.start.position.y + (this.currentPlatform.start.GetComponent<SpriteRenderer>().bounds.size.y * .5f)) {
                    dir = Vector2.zero;
                } 
            }
        }

        if (dir == Vector2.zero && currentZip == null) {
            animator.SetTrigger("Idle");
            state = State.Idle;
        }
        else if (dir != Vector2.zero) {
            animator.SetTrigger("Walk");
            state = State.Walking;
        }

        if (victim != null && currentReel == null) {
            victim.transform.position = spiderBackpack.transform.position;
        }

        this.transform.position += (Vector3)dir * speed * Time.deltaTime;       
    }

    private void Target() {

        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

        Vector2 facingDirection = worldMousePosition - transform.position;
        float aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f) {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        float x = transform.position.x + .75f * Mathf.Cos(aimAngle);
        float y = transform.position.y + .75f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        reticle.transform.position = crossHairPosition;
    }

    private void Shoot() {
        // left click
        if (Input.GetMouseButtonDown(0) && currentZip == null) {
            state = State.Shooting;

            animator.SetTrigger("Fire");

            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, aimDirection, slingDist, raycastMask);

            web.startColor = new Color(241 / 255f, 241 / 255f, 215 / 255f);
            web.endColor = new Color(241 / 255f, 241 / 255f, 215 / 255f);

            web.SetWidth(0.05f, 0.05f);
            web.positionCount = 2;
            web.SetPosition(0, this.transform.position);

            bool hitTrigger = false;
            for (int i = 0, counti = hit.Length; i < counti; i++) {
               if (hit[i].collider != null) {
                    hitTrigger = true;              
                    // Spider has hit platform which is not theirs
                    if (hit[i].transform.GetComponent<WebPlatform>() != null && hit[i].transform.GetComponent<WebPlatform>() != currentPlatform) {
                        // grab the platform we're aiming at
                        WebPlatform nextPlatform = hit[i].transform.gameObject.GetComponent<WebPlatform>();

                        // can't zip to same platform... this causes issues...
                        if (currentWeb == null && nextPlatform != null && nextPlatform != currentPlatform) {
                            currentWeb = new Web(this.transform.position, hit[i].point, hit[i].transform.gameObject.GetComponent<WebPlatform>());
                            Vector2 target = currentWeb.end;
                            currentOrientation = currentWeb.target.Direction();
                            //store the current coroutine so we don't start another until it's up!.. IEnumerator kinda sucks, but game jam!
                            Debug.Log("SETTING CURRENT ZIP");
                            currentZip = StartCoroutine(IZip(target));

                            float dot = Vector2.Dot(hit[i].normal, Vector2.up);

                            Debug.Log(string.Format("Surface Normal: {0}, Spider Up {1}, Dot {2}", hit[i].normal, Vector2.up, dot));

                            // floor
                            if (dot <= -1) {
                                Debug.Log("Horizontal");
                                Quaternion currentRot = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
                                rotState = RotState.InvertHorizontal;

                                this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, 180);                                
                            }

                            // ceiling
                            if (dot >= 1) {
                                Debug.Log("Horizontal");
                                rotState = RotState.normalHorizontal;

                                Quaternion currentRot = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
                                this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, 0);
                            }


                            if ((dot > 0 && dot < 1) || (dot < 0 && dot > -1)) {
                                Debug.Log("Vertical");

                                Quaternion currentRot = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
                                if (aimDirection.x < 0) {
                                    rotState = RotState.invertVertical;
                                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, -90);
                                }else {
                                    rotState = RotState.normalVertical;
                                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, 90);
                                }
                            }
                            break;
                        }
                    }
                    // Fly has caught spider
                    else if (hit[i].transform.GetComponent<Fly>() && hit[i].transform.GetComponent<Fly>() != victim) {
                        Debug.Log("Hit Fly");

                        hitTrigger = true;

                        Fly nextFly = hit[i].transform.GetComponent<Fly>();
                        /// i.e spider is not carrying anyone
                        if (victim == null) {                  

                            victim = nextFly;
                            victim.Caught();

                            currentReel = StartCoroutine(IReelInVictim());
                        } else {
                            nextFly.Kill();
                        }

                        break;
                    }
                }
            }
            
            // no hit
            if (!hitTrigger) {
                Debug.Log("No Hit");
                Debug.DrawRay(transform.position, aimDirection * slingDist, Color.yellow);
                web.SetPosition(1, this.transform.position + (Vector3)(aimDirection * 3));
            }       

        } else {
            if (currentZip == null && currentReel == null) {
                // stop slinging that web        
                web.positionCount = 0;
            }
        }
    }

    
    IEnumerator IZip(Vector2 target) {
        bool loop = true;
        animator.SetTrigger("Fly");

        while (loop) {
            state = State.Flying;

            transform.position = Vector3.MoveTowards(transform.position, target, zipSpeed * Time.deltaTime);

            web.SetPosition(0, this.transform.position);
            web.SetPosition(1, target);

            if ((Vector2.Distance(this.transform.position, target) > 0.25f)) {
                yield return null;
            }
            else {
                transform.position = target;                         

                currentPlatform = currentWeb.target;           

                web.positionCount = 0;

                loop = false;
                currentWeb = null;
                Debug.Log("Current Zip is Null now");
                currentZip = null;

                yield return true;
            }
        }
    }
    
    IEnumerator IReelInVictim () {
        if (victim != null) {

            // TODO: set Fly state?      
            bool loop = true;   

            while (loop) {
                victim.transform.position = Vector3.MoveTowards(victim.transform.position, spiderBackpack.position, zipSpeed * Time.deltaTime);
                web.SetPosition(0, spiderBackpack.position);
                web.SetPosition(1, victim.transform.position);

                if ((Vector2.Distance(spiderBackpack.position, victim.transform.position) > 0.25f)) {
                    yield return null;
                } else {
                    loop = false;
                    currentReel = null;
                }
            }

        } else {
            Debug.LogWarning("IReelInVictim called with now victim to reel!");
            currentReel = null;
        }
    }
}
