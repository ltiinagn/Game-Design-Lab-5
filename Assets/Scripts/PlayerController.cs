using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed, upSpeed;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public float maxSpeed;
    private bool onGroundState = true;
    private bool jumpState = false;

    private Animator marioAnimator;
    private AudioSource marioAudio;

    public ParticleSystem sparkle;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
	    Application.targetFrameRate = 30;
	    marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        
        marioAnimator = GetComponent<Animator>();
        marioAnimator.SetBool("onGround", onGroundState);

        marioAudio = GetComponent<AudioSource>();

        // subscribe to player event
        GameManager.OnPlayerDeath += PlayerDiesSequence;
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (Input.GetKeyDown("space") && onGroundState) {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            marioAnimator.SetBool("onGround", onGroundState);
            jumpState = true;
        }
        
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0) {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        if (onGroundState) {
            if (marioBody.velocity.y != 0) {
                onGroundState = false;
                marioAnimator.SetBool("onGround", onGroundState);
            }
            if ((!Input.GetKey("a") && !Input.GetKey("d"))) {
                // stop
                marioBody.velocity = Vector2.zero;
            }
        }
        else if (!onGroundState) {
            if (marioBody.velocity.y == 0) {
                if (jumpState) {
                    jumpState = false;
                }
                onGroundState = true;
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        // toggle state
        if (Input.GetKeyDown("a") && faceRightState) {
            if (onGroundState && Mathf.Abs(marioBody.velocity.x) > 1.0) {
	            marioAnimator.SetTrigger("onSkid");
            }
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState) {
            if (onGroundState && Mathf.Abs(marioBody.velocity.x) > 1.0) {
	            marioAnimator.SetTrigger("onSkid");
            }
            faceRightState = true;
            marioSprite.flipX = false;
        }

        if (Input.GetKeyDown("z")) {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
        }

        if (Input.GetKeyDown("x")) {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
        }
    }

    void PlayJumpSound() {
        if (jumpState) {
            marioAudio.PlayOneShot(marioAudio.clip);
        }
    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && !onGroundState) {
            onGroundState = true; // back on ground
            marioAnimator.SetBool("onGround", onGroundState);
            if (jumpState) {
                jumpState = false;
            }
            sparkle.Play();
        }
        // else if (col.gameObject.CompareTag("Obstacles") && Mathf.Abs(marioBody.velocity.y) < 0.01f) {
        //     Debug.Log("Collided");
        //     sparkle.Play();
        //     onGroundState = true; // back on ground
        //     marioAnimator.SetBool("onGround", onGroundState);
        //     if (jumpState) {
        //         jumpState = false;
        //     }
        // }
    }

    // void OnTriggerEnter2D(Collider2D other) {
    //     if (other.gameObject.CompareTag("Goomba")) {
    //         Debug.Log("Collided with Goomba!");
    //         Camera camera = Camera.main;
    //         camera.backgroundColor = Color.black;
    //         camera.clearFlags = CameraClearFlags.SolidColor;
    //         GameObject cameraObject = GameObject.Find("Main Camera");
    //         cameraObject.GetComponent<CameraController>().enabled = false;
    //         GameObject mainGameObject = GameObject.Find("UI").GetComponent<MenuController>().mainGameObject;
    //         Destroy(mainGameObject);
    //         Transform transformUI = GameObject.Find("UI").transform;
    //         foreach (Transform eachChild in transformUI) {
    //             if (eachChild.name != "Score") {
    //                 Debug.Log("Child found. Name: " + eachChild.name);
    //                 // enable them
    //                 eachChild.gameObject.SetActive(true);
    //             }
    //         }
    //     }
    // }

    void  PlayerDiesSequence(){
        // Mario dies
        Debug.Log("Mario dies");
        // TODO do whatever you want here, animate etc
        // ...
    }
}
