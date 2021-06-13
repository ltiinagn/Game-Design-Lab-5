using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableMushroom : MonoBehaviour
{
    public Vector2 velocityBefore;
    public Rigidbody2D rigidBody;
    public bool stop;
    public BoxCollider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        // rigidBody.AddForce(Vector2.up * 8, ForceMode2D.Impulse);
        coll = GetComponent<BoxCollider2D>();
        coll.sharedMaterial.friction = 0;
        coll.enabled = false;
        coll.enabled = true;
        stop = false;

        int randomInt = Random.Range(0, 2);
        if (randomInt == 0) {
            rigidBody.AddForce(Vector2.left * 3, ForceMode2D.Impulse);
        }
        else {
            rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
        }
    }
    
    void FixedUpdate() {
        velocityBefore = rigidBody.velocity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!stop) {
            Debug.Log(col.gameObject.tag);
            if (col.gameObject.CompareTag("Pipe")) {
                rigidBody.velocity = new Vector2(velocityBefore.x * -1, velocityBefore.y);
            }
            else if (col.gameObject.CompareTag("Player")) {
                stop = true;
                coll.sharedMaterial.friction = 1;
                coll.enabled = false;
                coll.enabled = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Goomba")) {
            Destroy(gameObject);
        }    
    }

    void OnBecameInvisible(){
        Destroy(gameObject);
    }
}
