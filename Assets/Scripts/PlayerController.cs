using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int groundRange = 24;

    private Rigidbody playerRb;
    private float horizontalInput;
    private float verticalInput;
    private bool turnLeftInput;
    private bool turnRightInput;

    [SerializeField] float speed = 1000f;
    [SerializeField] float speedPowerUp = 1000f;
    [SerializeField] float speedPowerUpTime = 5f;
    [SerializeField] bool isSpeedPowerUp = false;

    [SerializeField] float rotateSpeed = 60f;
    [SerializeField] float rotateSpeedPowerUp = 60f;

    public GameObject bulletPrefab;
    private float bulletOffset = 1.0f;


    [SerializeField] float shotCD = 0.1f;
    [SerializeField] float bulletPowerUpTime = 5f;
    [SerializeField] bool isBulletPowerUp = false;

    private GameController gameController;

    private bool isDead = false;
    // Start is called before the first frame update

    public ParticleSystem deadParticle;

    public void StartGame()
    {
        playerRb = GetComponent<Rigidbody>();
        StartCoroutine(Shot());
    }
    void Start()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.IsGameOver())
        {
            ForceMove();
            Rotation();
        }
    }

    void ForceMove()
    {
        //horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //playerRb.AddRelativeForce(Vector3.right * Time.deltaTime * speed * horizontalInput);
        playerRb.AddRelativeForce(Vector3.forward * Time.deltaTime * (speed + (isSpeedPowerUp ? speedPowerUp : 0)) * verticalInput);


        Vector3 position = transform.position;
        Vector3 velocity = playerRb.velocity;


        position.x = ClampPosition(position.x, -groundRange, groundRange, ref velocity.x);
        position.y = ClampPosition(position.y, -groundRange, groundRange, ref velocity.y);
        position.z = ClampPosition(position.z, -groundRange, groundRange, ref velocity.z);

        transform.position = position;
        playerRb.velocity = velocity;
    }

    float ClampPosition(float value, float min, float max, ref float velocity)
    {
        if (value < min)
        {
            value = min;
            velocity = 0;
        }
        else if (value > max)
        {
            value = max;
            velocity = 0;
        }
        return value;
    }

    void Rotation()
    {
        turnLeftInput = Input.GetKey(KeyCode.LeftArrow);
        turnRightInput = Input.GetKey(KeyCode.RightArrow);

        if (turnLeftInput && !turnRightInput)
        {
            transform.Rotate(Vector3.up * -(rotateSpeed + (isSpeedPowerUp ? rotateSpeedPowerUp : 0)) * Time.deltaTime);
        }
        else if (!turnLeftInput && turnRightInput)
        {
            transform.Rotate(Vector3.up * (rotateSpeed + (isSpeedPowerUp ? rotateSpeedPowerUp : 0)) * Time.deltaTime);
        }
    }

    IEnumerator Shot()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(shotCD);
            Vector3 playerPosition = transform.position;
            Vector3 playerForward = transform.forward;
            Vector3 spawnPosition = playerPosition + playerForward * bulletOffset;
            if (!isBulletPowerUp)
            {
                Instantiate(bulletPrefab, spawnPosition, transform.rotation * bulletPrefab.transform.rotation);
            }
            else
            {
                Vector3 spawnPosition_1 = spawnPosition - transform.right * bulletOffset * 0.5f;
                Instantiate(bulletPrefab, spawnPosition_1, transform.rotation * bulletPrefab.transform.rotation);

                Vector3 spawnPosition_2 = spawnPosition + transform.right * bulletOffset * 0.5f;
                Instantiate(bulletPrefab, spawnPosition_2, transform.rotation * bulletPrefab.transform.rotation);
            }

        }

    }

    public void EatPowerUp(string powerUpTag)
    {
        if (powerUpTag == "BulletPowerUp")
        {
            bulletPowerUpTime = 5;
            if (!isBulletPowerUp)
            {
                isBulletPowerUp = true;
                StartCoroutine(BulletPowerUpIE());
            }

        }
        if (powerUpTag == "SpeedPowerUp")
        {
            speedPowerUpTime = 5;
            if (!isSpeedPowerUp)
            {
                isSpeedPowerUp = true;
                speedPowerUpTime = 5;
                StartCoroutine(SpeedPowerUpIE());
            }

        }
    }

    IEnumerator BulletPowerUpIE()
    {
        while (isBulletPowerUp)
        {
            yield return new WaitForSeconds(1);
            bulletPowerUpTime--;
            if (bulletPowerUpTime <= 0)
            {
                isBulletPowerUp = false;
            }
        }
    }

    IEnumerator SpeedPowerUpIE()
    {
        while (isSpeedPowerUp)
        {
            yield return new WaitForSeconds(1);
            speedPowerUpTime--;
            if (speedPowerUpTime <= 0)
            {
                isSpeedPowerUp = false;
            }
        }
    }

    public bool checkIsBulletPowerUp()
    {
        return isBulletPowerUp;
    }

    public bool checkIsSpeedPowerUp()
    {
        return isSpeedPowerUp;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Instantiate(deadParticle, transform.position, deadParticle.transform.rotation);
            isDead = true;
            isBulletPowerUp = false;
            isSpeedPowerUp = false;
            gameController.GameOver();
            Destroy(gameObject);
        }
    }
}
