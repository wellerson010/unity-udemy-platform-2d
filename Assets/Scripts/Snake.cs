using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {
    public int Health;
    public float Speed;
    public Transform WallCheck;

    private bool TouchedWall = false;
    private bool FacingRight = true;
    private Rigidbody2D RGBD2D;
    private SpriteRenderer Sprite;
	// Use this for initialization
	void Start () {
        RGBD2D = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        TouchedWall = Physics2D.Linecast(transform.position, WallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (TouchedWall)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        RGBD2D.velocity = new Vector2(Speed, RGBD2D.velocity.y);
    }

    void Flip()
    {
        FacingRight = !FacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        Speed *= -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            DamageEnemy();
        }
    }

    IEnumerator DamageEffect()
    {
        float actualSpeed = Speed;
        Speed = Speed * -1;
        Sprite.color = Color.red;
        RGBD2D.AddForce(new Vector2(0, 200));
        yield return new WaitForSeconds(0.1f);
        Sprite.color = Color.white;
        Speed = actualSpeed;
        
    }

    void DamageEnemy()
    {
        Health--;
        StartCoroutine(DamageEffect());

        if (Health < 1)
        {
            Destroy(gameObject);
        }
    }
}
