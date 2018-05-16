using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Types of hazards in a wave
    public GameObject[] hazards;

    public GameObject boss;

    public List<GameObject> hazardsAlive;

    public Boundary boundary;

    // Number of hazard objects in a wave
    public int hazardsInWave;

    // Hazrad alive
    //public int hazardsAlive;

    public int currentWave;

    // Time to spawn other hazard in a wave
    public float spawnWaitTime = 3f;

    // Check status of Coroutine
    public bool isRunning;

    // Delay time for waves
    public WaitForSeconds delay = new WaitForSeconds(1f);

    // Use this for initialization
    IEnumerator Start()
    {
        while (true)
        {
            currentWave++;

            // Each 5 waves, a boss wave appear
            if (currentWave % 5 == 0)
            {
                Hazards h = boss.transform.GetComponent<Hazards>();
                h.hp = currentWave * 5;
                h.point = currentWave * 10;
                Instantiate(boss, new Vector3(0, 0, 11), new Quaternion(180,0,0,0));
            }
            else
            {
                hazardsInWave = currentWave * 5;

                //hazardsAlive = hazardsInWave;

                // Set health and point for hazards
                foreach (var gameObject in hazards)
                {
                    Hazards h = gameObject.transform.GetComponent<Hazards>();
                    h.hp = currentWave * 2;
                    h.point = currentWave * 5;
                }

                while (hazardsInWave > 0)
                {
                    // Limit spawn position of hazards in boundary
                    Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, 11);

                    GameObject go = Instantiate(hazards[Random.Range(0, hazards.Length)],
                        spawnPosition, new Quaternion(180, 0, 0, 0));

                    //hazardsAlive.Add(go);

                    //if(hazardsAlive[0].gameObject == null)
                    //{
                    //    print("Object destroyed: " + go.ToString());
                    //}

                    hazardsInWave--;
                    yield return new WaitForSeconds(spawnWaitTime);
                }
            }
            
            // Reduce hazards spawn delay each wave
            if (spawnWaitTime > 0.5)
            {
                spawnWaitTime -= 0.5f;
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
        //if (isRunning == false)
        //{
        //    //isRunning = true;
        //    hazardsInWave = currentWave * 10;
        //    hp = currentWave * 2;
        //    point = currentWave * 5;
        //    StartCoroutine(Spawn());
        //    currentWave++;
        //    //hazardsInWave = currentWave * 10;
        //    if (spawnWaitTime > 1)
        //    {
        //        spawnWaitTime--;
        //    }
        //    //StopCoroutine(Spawn());
        //    //yield return new WaitForSeconds(5);
        //}

    }
}
