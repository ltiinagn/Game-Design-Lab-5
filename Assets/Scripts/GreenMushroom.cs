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

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Player")){
			// update UI
			CentralManager.centralManagerInstance.addPowerup(t, gameConstants.powerupGreenSlot, this);
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
			GetComponent<Collider2D>().enabled = false;
			GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}