using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{

    // Hazards health
    public int hp;

    // Hazards kill point
    public int point;

    public GameObject[] hazards;
    public Boundary boundary;

    // Number of hazard objects in a wave
    public int hazardsInWave;

    public int currentWave;

    // Time to spawn other hazard
    public int spawnWaitTime;

    public bool isRunning;
    public WaitForSeconds delay = new WaitForSeconds(1f);
    // Use this for initialization
    IEnumerator Start()
    {
        while (true)
        {
            currentWave++;
            hazardsInWave = currentWave * 10;
            hp = currentWave * 2;
            point = currentWave * 5;
            if (spawnWaitTime > 1)
            {
                spawnWaitTime--;
            }

            while (hazardsInWave > 0)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, 11);
                Instantiate(hazards[Random.Range(0, hazards.Length)], spawnPosition, new Quaternion(180, 0, 0, 0));
                hazardsInWave--;
                yield return new WaitForSeconds(spawnWaitTime);
            }
            yield return delay;
        }
    }

    // Create hazards
    IEnumerator Spawn()
    {
        isRunning = true;
        while (hazardsInWave > 0)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, 11);
            Instantiate(hazards[Random.Range(0, hazards.Length)], spawnPosition, new Quaternion(180, 0, 0, 0));
            hazardsInWave--;
            yield return new WaitForSeconds(spawnWaitTime);
        }
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        return;
        if (isRunning == false)
        {
            //isRunning = true;
            hazardsInWave = currentWave * 10;
            hp = currentWave * 2;
            point = currentWave * 5;
            StartCoroutine(Spawn());
            currentWave++;
            //hazardsInWave = currentWave * 10;
            if (spawnWaitTime > 1)
            {
                spawnWaitTime--;
            }
            //StopCoroutine(Spawn());
            //yield return new WaitForSeconds(5);
        }

    }
}
