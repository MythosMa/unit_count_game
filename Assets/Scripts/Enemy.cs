using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public float moveSpeed;

    private float stopTime = 1;
    private float currentStopTime = 0;
    private GameObject player;
    // Start is called before the first frame update

    private GameController gameController;

    public ParticleSystem deadParticle;
    void Start()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.IsGameOver())
        {
            float speedDown = 1;
            if (currentStopTime > 0)
            {
                currentStopTime -= Time.deltaTime;
                speedDown = 0.5f;
            }
            transform.LookAt(player.transform);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime * speedDown);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            hp--;
            currentStopTime = stopTime;
            Destroy(other.gameObject);

            if (hp <= 0)
            {
                Instantiate(deadParticle, transform.position, deadParticle.transform.rotation);
                gameController.KillEnemy();
                Destroy(gameObject);
            }
        }

    }
}
