using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RedMushroom : MonoBehaviour, ConsumableInterface
{
	public Texture t;
	public GameConstants gameConstants;
	
	public void consumedBy(GameObject player){
		// give player jump boost
		player.GetComponent<PlayerController>().upSpeed += gameConstants.powerUpRedIncrease;
		StartCoroutine(removeEffect(player));
	}

	IEnumerator removeEffect(GameObject player){
		GameObject powerupSlot1 = GameObject.Find("UI/PowerupSlot1");
		for (int i = 0; i < gameConstants.powerUpRedTimeSteps; i++) {
			powerupSlot1.SetActive(!powerupSlot1.activeSelf);
			yield return new WaitForSeconds(gameConstants.powerUpBlinkInterval);
		}
		powerupSlot1.SetActive(false);
		player.GetComponent<PlayerController>().upSpeed -= gameConstants.powerUpRedIncrease;
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Player")){
			// update UI
			CentralManager.centralManagerInstance.addPowerup(t, gameConstants.powerupRedSlot, this);
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
			GetComponent<Collider2D>().enabled = false;
			GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}