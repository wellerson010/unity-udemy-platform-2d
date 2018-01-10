using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour {

    private Player PlayerScript;

    public AudioClip FXCoin;

	// Use this for initialization
	void Start () {
        PlayerScript = GameObject.Find("Player").GetComponent<Player>();
	}

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")){
            PlayerScript.DamagePlayer();
        }

        if (collision.CompareTag("Coin"))
        {
            SoundManager.instance.PlaySound(FXCoin);
            Destroy(collision.gameObject);
        }
    }
}
