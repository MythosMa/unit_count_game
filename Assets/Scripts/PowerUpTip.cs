using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTip : MonoBehaviour
{
    public GameObject bulletPowerUp;
    public GameObject speedPowerUp;

    private float rotateSpeed = 200f;
    private GameObject player;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        transform.position = player.transform.position;

        checkBulletPowerUpActive();
        checkSpeedPowerUpActive();
    }

    void checkBulletPowerUpActive()
    {
        if (playerController.checkIsBulletPowerUp() && !bulletPowerUp.activeSelf)
        {
            bulletPowerUp.SetActive(true);
        }
        else if (!playerController.checkIsBulletPowerUp() && bulletPowerUp.activeSelf)
        {
            bulletPowerUp.SetActive(false);
        }
    }

    void checkSpeedPowerUpActive()
    {
        if (playerController.checkIsSpeedPowerUp() && !speedPowerUp.activeSelf)
        {
            speedPowerUp.SetActive(true);
        }
        else if (!playerController.checkIsSpeedPowerUp() && speedPowerUp.activeSelf)
        {
            speedPowerUp.SetActive(false);
        }
    }
}
