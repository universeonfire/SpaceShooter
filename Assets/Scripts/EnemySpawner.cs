﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    private int startingWave = 0;
    private int enemyNumber = 0;
    private bool looping = true;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());

        } while (looping);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator SpawnAllWaves()
    {
        for(int i = startingWave; i < waveConfigs.Count; i++)
        {
            var currentWave = waveConfigs[i];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }


    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig )
    {
        for(int i= 0; i< waveConfig.GetNumberOfEnemies(); i++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWaypoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWayConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
        
    }
}
