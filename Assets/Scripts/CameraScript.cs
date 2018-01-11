using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float SmoothTimeX;
    public float SmoothTimeY;

    private Vector2 Velocity;
    private Transform Player;
    private float ShakeTime;
    private float ShakeAmount;

    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (Player != null)
        {
            float posX = Mathf.SmoothDamp(transform.position.x, Player.position.x, ref Velocity.x, SmoothTimeX);
            float posY = Mathf.SmoothDamp(transform.position.y, Player.position.y, ref Velocity.y, SmoothTimeY);

            transform.position = new Vector3(posX, posY, transform.position.z);
        }
    }

    void Update()
    {
        if (ShakeTime >= 0)
        {
            Vector2 shakePosition = Random.insideUnitCircle * ShakeAmount;
            transform.position = new Vector3(transform.position.x + shakePosition.x, transform.position.y + shakePosition.y, transform.position.z);
            ShakeTime -= Time.deltaTime;
        }
    }

    public void ShakeCamera(float timer, float amount)
    {
        ShakeTime = timer;
        ShakeAmount = amount;
    }
}
