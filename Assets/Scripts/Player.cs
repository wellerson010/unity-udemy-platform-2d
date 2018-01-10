using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float Speed;
    public int JumpForce;
    public int Health;
    public Transform GroundCheck;
    public float AttackRate;
    public Transform SpawnAttack;
    public GameObject AttackPrefab;
    public GameObject Crown;
    public AudioClip FXHurt;
    public AudioClip FXJump;
    public AudioClip FXAttack;

    private float NextAttack = 0;
    private bool Invunerable = false;
    private bool Grounded = false;
    private bool Jumping = false;
    private bool FacingRight = true;
    private SpriteRenderer Sprite;
    private Rigidbody2D RB2D;
    private Animator Animator;
    private CameraScript CameraScript;

    // Use this for initialization
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        RB2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        CameraScript = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Grounded && Input.GetButtonDown("Jump"))
        {
            Jumping = true;
            SoundManager.instance.PlaySound(FXJump);
        }

        Animations();

        if (Input.GetButton("Fire1") && Grounded && Time.time > NextAttack)
        {
            SoundManager.instance.PlaySound(FXAttack);
            Attack();
        }
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        RB2D.velocity = new Vector2(move * Speed, RB2D.velocity.y);

        if ((move < 0 && FacingRight) || move > 0 && !FacingRight)
        {
            Flip();
        }

        if (Jumping)
        {
            RB2D.AddForce(new Vector2(0, JumpForce));
            Jumping = false;
        }
    }

    void Flip()
    {
        FacingRight = !FacingRight;

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
    }

    void Animations()
    {
        Animator.SetFloat("VelY", RB2D.velocity.y);
        Animator.SetBool("JumpFall", !Grounded);
        Animator.SetBool("Walk", Mathf.Abs(RB2D.velocity.x) != 0 && Grounded);
    }

    void Attack()
    {
        Animator.SetTrigger("Punch");

        NextAttack = Time.time + AttackRate;

        GameObject cloneAttack = Instantiate(AttackPrefab, SpawnAttack.position, SpawnAttack.rotation);

        if (!FacingRight)
        {
            cloneAttack.transform.eulerAngles = new Vector3(180, 0, 180);
        }
    }

    IEnumerator DamageEffect()
    {
        for (float i = 0; i < 1f; i += 0.1f)
        {
            Sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            Sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        Invunerable = false;
    }

    public void DamagePlayer()
    {
        if (!Invunerable)
        {
            Invunerable = true;
            Health--;

            SoundManager.instance.PlaySound(FXHurt);
            HUD.instance.RefreshLife(Health);
            CameraScript.ShakeCamera(0.5f, 0.1f);
            
            StartCoroutine(DamageEffect());

            if (Health < 1)
            {
                KingDeath();
                Invoke("ReloadLevel", 3f);
                gameObject.SetActive(false);
            }
        }
    }

    void KingDeath()
    {
        GameObject crown = Instantiate(Crown, transform.position, Quaternion.identity);
        Rigidbody2D rigidBodyCrown = crown.GetComponent<Rigidbody2D>();
        rigidBodyCrown.AddForce(Vector3.up * 500);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
