using System.Collections;
using UnityEngine;

public class GreenMushroom : MonoBehaviour, ConsumableInterface
{
	public Texture t;
	public GameConstants gameConstants;
	
	public void consumedBy(GameObject player){
		// give player speed boost
		player.GetComponent<PlayerController>().maxSpeed *= gameConstants.powerUpGreenMultiplier;
		StartCoroutine(removeEffect(player));
	}

	IEnumerator removeEffect(GameObject player){
		GameObject powerupSlot2 = GameObject.Find("UI/PowerupSlot2");
		for (int i = 0; i < gameConstants.powerUpGreenTimeSteps; i++) {
			powerupSlot2.SetActive(!powerupSlot2.activeSelf);
			yield return new WaitForSeconds(gameConstants.powerUpBlinkInterval);
		}
		powerupSlot2.SetActive(false);
		player.GetComponent<PlayerController>().maxSpeed /= gameConstants.powerUpGreenMultiplier;
		Destroy(gameObject);
	}

	IEnumerator minimize() {
		int steps = 5;
		float stepper = 1.0f/(float) steps;

		for (int i = 0; i < steps; i ++){
			gameObject.transform.parent.transform.localScale = new Vector3(gameObject.transform.parent.transform.localScale.x + stepper, gameObject.transform.parent.transform.localScale.y + stepper, gameObject.transform.parent.transform.localScale.z);
			yield return null;
		}

		for (int i = 0; i < steps*2; i ++){
			gameObject.transform.parent.transform.localScale = new Vector3(gameObject.transform.parent.transform.localScale.x - stepper, gameObject.transform.parent.transform.localScale.y - stepper, gameObject.transform.parent.transform.localScale.z);
			yield return null;
		}
		gameObject.transform.parent.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		gameObject.transform.parent.GetComponent<Collider2D>().enabled = false;
		gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Player")){
			// update UI
			CentralManager.centralManagerInstance.addPowerup(t, gameConstants.powerupGreenSlot, this);
			BoxCollider2D parentColl = gameObject.transform.parent.GetComponent<BoxCollider2D>();
            parentColl.sharedMaterial.friction = 1;
            parentColl.enabled = false;
            parentColl.enabled = true;
			StartCoroutine(minimize());
		}
	}
}