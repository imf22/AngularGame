using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public class SpawnRandomUpgrade : MonoBehaviour
    {
        public GameObject[] ItemsList;
        [Range (1,10)] public int ItemSpawnInterval = 3;
        [Range(-10,10)] public float ItemSpawnOffset = 2f;
        private int currentWave = 0;
        private int waveCounter;

        private GameObject findPlayer()
        {
            return GameObject.FindGameObjectWithTag("Player");
        }

        public void SpawnUpgrade()
        {
            // Find Player
            GameObject player = findPlayer();

            // Select Random Item From Array
            int randomIndex = Random.Range(0, ItemsList.Length);

            // Player Position
            Vector3 pos = new Vector3(0, player.transform.position.y + 2, player.transform.position.z);

            // Spawn Item above Player position
            GameObject spawnedItem = Instantiate(ItemsList[randomIndex], pos, Quaternion.identity);
        }

        public void UpdateCurrentWave()
        {
            currentWave += 1;

            //Debug.Log("Current Wave: " + currentWave + "\nWave Interval: " + ItemSpawnInterval + "\nMath: " + (currentWave % ItemSpawnInterval));

            if (currentWave % ItemSpawnInterval == 0)
            {
                SpawnUpgrade();
            }
        }
    }    
}
