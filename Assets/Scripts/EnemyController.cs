using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public GameConstants gameConstants;
	private int moveRight;
	private float originalX;
	private Vector2 velocity;
	private Rigidbody2D enemyBody;
	
	void Start()
	{
		enemyBody = GetComponent<Rigidbody2D>();
		
		// get the starting position
		originalX = transform.position.x;
	
		// randomise initial direction
		moveRight = Random.Range(0, 2) == 0 ? -1 : 1;
		
		// compute initial velocity
		ComputeVelocity();

        // subscribe to player event
        GameManager.OnPlayerDeath += EnemyRejoice;
	}
	
	void ComputeVelocity()
	{
	    velocity = new Vector2((moveRight) * gameConstants.maxOffset / gameConstants.enemyPatroltime, 0);
	}
  
	void MoveEnemy()
	{
		enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
	}

	void Update()
	{
		if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset)
		{   
            // move goomba
			MoveEnemy();
		}
		else
		{
			// change direction
			moveRight *= -1;
			ComputeVelocity();
			MoveEnemy();
		}
	}

    void OnCollisionEnter2D(Collision2D other) {
		// check if it collides with Mario
		if (other.gameObject.tag == "Player"){
			// check if collides on top
			float yoffset = (other.transform.position.y - this.transform.position.y);
			if (yoffset > 0.75f) {
				Rigidbody2D marioBody = GameObject.Find("UI").GetComponent<MenuController>().mainGameObject.transform.Find("Mario").GetComponent<Rigidbody2D>();
				marioBody.velocity = new Vector2(marioBody.velocity.x, 0.0f);
				if (gameObject.tag == "Goomba") {
					marioBody.AddForce(Vector2.up * gameConstants.bounce, ForceMode2D.Impulse);
				}
				else if (gameObject.tag == "Koopa") {
					marioBody.AddForce(Vector2.up * gameConstants.bounce * 1.5f, ForceMode2D.Impulse);
				}
				KillSelf();
			}
			else {
				// hurt player
		        CentralManager.centralManagerInstance.damagePlayer();
			}
		}
	}

    void KillSelf() {
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}

    IEnumerator flatten() {
		Debug.Log("Flatten starts");
		int steps = 5;
		float stepper = 1.0f/(float) steps;

		for (int i = 0; i < steps; i ++){
			this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position = new Vector3(this.transform.position.x, gameConstants.groundSurface + GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield return null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
		// enemy dies
		if (gameObject.tag == "Goomba") {
			CentralManager.centralManagerInstance.increaseScoreGoomba();
		}
		else if (gameObject.tag == "Koopa") {
			CentralManager.centralManagerInstance.increaseScoreKoopa();
		}
		yield break;
	}

    // animation when player is dead
    void EnemyRejoice(){
        Debug.Log("Enemy killed Mario");
        // TODO do whatever you want here, animate etc
        // ...
    }
}