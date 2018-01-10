using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Sprite[] Sprites;
    public Image LifeBar;

    public static HUD instance = null;

	// Use this for initialization
	void Awake () {
		if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
	}
	
	public void RefreshLife(int playerHealth)
    {
        LifeBar.sprite = Sprites[playerHealth];
    }
}
