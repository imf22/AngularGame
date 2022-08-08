//using angulargame;
using System;
using System.Collections;
using UnityEngine;

namespace angulargame
{
    public class SpawnController : MonoBehaviour
    {
        public Transform[] spawners;
        public GameObject[] possibleEnemies;
        public GameObject RoomBounds;


        public int Wave = 0;
        public float difficultyScale = 0;
        private int Score = 0;
        private int numEnemiesPerSpawner = 1;
        private float waveLength;

        public int spawnedSubWaves = 0;
        private float subWaveIntervalsTime = 7.5f;
        private int numSubWaves = 5;
        public int numPossibleEnemies;

        private GameObject player;


         void Start()
        {
            numPossibleEnemies = 0;

            // Game starting conditions
            //waveLength = 5;
            player = GameObject.Find("PlayerCube");

            // Start Game
            StartCoroutine(SpawnWaveEnemies());
        }

        // Update is called once per frame
        void Update()
        {

        }


        private IEnumerator SpawnWaveEnemies()
        {
            while (player != null)
            {
                System.Random r = new System.Random();
                int rInt = r.Next(0, numPossibleEnemies);
                print("rInt: " + rInt);
                GameObject selectedEnemy = possibleEnemies[rInt];
           
                //for (int i = 0; i < numSubWaves; i++)
                //{
                //}
            
                foreach (Transform spawnpoint in spawners)
                {
                    GameObject newEnemy = (GameObject)Instantiate(selectedEnemy, spawnpoint);
                }

                spawnedSubWaves += 1;
                Wave += 1;

                if (spawnedSubWaves == numSubWaves)
                {
                    spawnedSubWaves = 0;
                    numSubWaves += 1;
                    //Wave += 1;
                }
                if (waveLength % 5 == 0)
                {
                    if (numPossibleEnemies <= possibleEnemies.Length - 1)
                    {
                        numPossibleEnemies += 1;
                    }

                }

                yield return new WaitForSeconds(subWaveIntervalsTime);
            }
        }

        public int getCurrentWave()
        {
            return this.Wave;
        }

    }

}
