using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] List<GameObject> powerUpPrefabs;
    [SerializeField] int maxPowerUpCount = 3;

    private float createPowerUpRate = 1f;
    private float createPowerUpRange = 15f;

    [SerializeField] List<GameObject> enemyPrefabs;
    private float createEnemyRate = 2f;
    private float createEnemyRange = 15f;

    private float validDistance = 4f;

    private bool isGameOver = true;

    private GameObject player;

    [SerializeField] TextMeshProUGUI killCountText;
    private int killCount = 0;

    [SerializeField] TextMeshProUGUI gameTimeText;
    private float gameTime = 90;

    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameInfoScreen;


    // Start is called before the first frame update

    public void StartGame()
    {
        isGameOver = false;
        startScreen.SetActive(false);
        gameInfoScreen.SetActive(true);
        killCountText.SetText("Kill Count: " + killCount);
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().StartGame();
        StartCoroutine(CreatePowerUp());
        StartCoroutine(CreateEnemy());
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            gameTime -= Time.deltaTime;
            gameTimeText.SetText("LastTime:" + Mathf.FloorToInt(gameTime));

            if (gameTime <= 0)
            {
                GameOver();
            }
        }
    }

    IEnumerator CreatePowerUp()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(createPowerUpRate);
            if (!isGameOver && CanCreatePowerUp())
            {
                int index = Random.Range(0, powerUpPrefabs.Count);
                Instantiate(powerUpPrefabs[index], CreatePosition(createPowerUpRange), powerUpPrefabs[index].transform.rotation);
            }
        }
    }

    bool CanCreatePowerUp()
    {
        int count = 0;
        foreach (GameObject powerUp in powerUpPrefabs)
        {
            string tagName = powerUp.tag;
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);
            count += objects.Length;
        }
        return count < maxPowerUpCount;
    }

    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(createEnemyRate);
            if (!isGameOver)
            {
                float randomNum = Random.Range(0f, 1f);
                int enemyIndex = 0;
                if (randomNum < 0.5f)
                {
                    enemyIndex = 0;
                }
                else if (randomNum < 0.8f)
                {
                    enemyIndex = 1;
                }
                else
                {
                    enemyIndex = 2;
                }
                Instantiate(enemyPrefabs[enemyIndex], CreatePosition(createEnemyRange), enemyPrefabs[enemyIndex].transform.rotation);
            }

        }
    }

    Vector3 CreatePosition(float range)
    {
        Vector3 createPosition = new Vector3(Random.Range(-range, range), 2.5f, Random.Range(-range, range));
        return Vector3.Distance(player.transform.position, createPosition) > validDistance ? createPosition : CreatePosition(range);
    }

    public void KillEnemy()
    {
        killCount++;
        killCountText.SetText("Kill Count: " + killCount);
    }

    public void GameOver()
    {
        isGameOver = true;
        StartCoroutine(ShowGameOver());
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(2f);
        gameInfoScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        gameOverScreen.transform.Find("EndTip").GetComponent<TextMeshProUGUI>().SetText("YOUR KILL: " + killCount + " CUBES!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
