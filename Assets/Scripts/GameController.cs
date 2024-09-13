using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] List<GameObject> powerUpPrefabs;
    [SerializeField] int maxPowerUpCount = 3;

    private float createPowerUpRate = 1f;
    private float createPowerUpRange = 15f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreatePowerUp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CreatePowerUp()
    {
        while (true) {
            yield return new WaitForSeconds(createPowerUpRate);
            if (canCreatePowerUp())
            {
                int index = Random.Range(0, powerUpPrefabs.Count);
                Instantiate(powerUpPrefabs[index], createPosition(createPowerUpRange), powerUpPrefabs[index].transform.rotation);
            }
        }    
    }

    bool canCreatePowerUp()
    {
        int count = 0;
        foreach(GameObject powerUp in powerUpPrefabs)
        {
            string tagName = powerUp.tag;
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);
            count += objects.Length;
        }
        Debug.Log(Random.Range(0, 6) + " | " + count);
        return count < maxPowerUpCount;
    }

    Vector3 createPosition(float range)
    {
        return new Vector3(Random.Range(-range, range), 2.5f, Random.Range(-range, range));
    }
}
