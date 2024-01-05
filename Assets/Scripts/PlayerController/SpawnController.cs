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


        public int Wave = 1;
        public float difficultyScale = 0;

        public int spawnedSubWaves = 0;
        private float subWaveIntervalsTime = 7.5f;
        private int numSubWaves = 1;
        public int numPossibleEnemies;

        //private int _waveSet = 5;

        private float _minimumSubWaveInterval = 4f;
        private float _waveReductionRate = 0.5f;
        private bool _addedEnemyThisWaveSet = false;

        // New Wave variables
        [Range(1, 10)]  public int NewEnemyInterval = 5;
        private float _MINIMUM_WAVE_INTERVAL = 1.0f;
        private float enemySpawnInterval = 7.4f;

        // Debug Output
        public bool WaveStateLog = false;
        public bool SpawnedEnemies = false;

        // Upgrade Spawner
        private SpawnRandomUpgrade UpgradeSpawner;



        private GameObject player;

        void Start()
        {
            // Game starting conditions
            numPossibleEnemies = 0;

            // Get Upgrade Spawner
            UpgradeSpawner = GameObject.Find("UpgradeSpawner").GetComponent<SpawnRandomUpgrade>();

            //// Start Game
            StartCoroutine(SpawnEnemiesContoller());
        }

        private IEnumerator SpawnEnemiesContoller()
        {
            // Brief Starting Grace Period
            yield return new WaitForSeconds(3);

            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];

            int nunberEnemiesPerWave = 10 + (5 * Wave);
            int spawnedEnemies = 0;

            while (player != null)
            {
                System.Random r = new System.Random();

                // Move to player position
                this.transform.position = player.transform.position;

                // Spawn Enemy
                for(int i = 0; i < 4; i++)
                {
                    // Select random enemy to spawn
                    int rInt = r.Next(0, numPossibleEnemies);
                    GameObject selectedEnemy = possibleEnemies[rInt];

                    // Select and apply Random rotation releative to player to enemy spawner
                    rInt = r.Next(0, 180);
                    this.transform.rotation = Quaternion.Euler(new Vector3(rInt, 0, 0));

                    // Spawn point
                    Vector3 pos = new Vector3(0, spawners[0].transform.position.y, spawners[0].transform.position.z);
                    GameObject newEnemy = (GameObject)Instantiate(selectedEnemy, pos, Quaternion.identity);

                    //increament
                    spawnedEnemies += 1;
                }

                if (spawnedEnemies >= nunberEnemiesPerWave)
                {
                    // Increment Wave 
                    Wave += 1;
                    // Reset spawned enemies counter
                    spawnedEnemies = 0;
                    // Update next enemy wave size
                    nunberEnemiesPerWave = 15 + (5 * Wave);

                    // Reduce Spawn Interval
                    if (enemySpawnInterval > _MINIMUM_WAVE_INTERVAL)
                    {
                        enemySpawnInterval -= 0.2f;
                    }


                    // Increase number of possible enemies
                    // Should use possibleEnemies.Length but temporaily hardcoded
                    if ((Wave % NewEnemyInterval) == 0 && numPossibleEnemies < 3)
                    {
                        //Debug.Log("New Enemy Added");
                        numPossibleEnemies += 1;
                    }

                    // Update Wave in UpgradeSpawner: Will Spawn an Upgrade once defined number of waves is reached
                    UpgradeSpawner.UpdateCurrentWave();

                    // Debug Option: Debug Log
                    if (WaveStateLog)
                    {
                        Debug.Log("Wave: " + Wave + "\nWave Size: " + nunberEnemiesPerWave + "\nPossible Enemies" + numPossibleEnemies + "\nWave Interval Time: " + subWaveIntervalsTime);
                    }
                    
                }

                // Debug Option: Spawn Enemies in Current Wave
                if (SpawnedEnemies) Debug.Log("Spawned Enemies" + spawnedEnemies);

                // Spawn Interval
                yield return new WaitForSeconds(enemySpawnInterval);
            }
        }


        public int getCurrentWave()
        {
            return this.Wave;
        }

        public void setPlayer(GameObject nPlayer)
        {
            player = nPlayer;
        }

        public void resetPlayer(GameObject nPlayer)
        {
            player = nPlayer;
            StopCoroutine(SpawnEnemiesContoller());
            StartCoroutine(SpawnEnemiesContoller());

        }


        private void waveUp()
        {
            // Reset subwave counter, increase number of subwaves, increment current wave, add new enemy to 5wave set check
            spawnedSubWaves = 0;
            numSubWaves += 1;
            Wave += 1;
            _addedEnemyThisWaveSet = false;
            print("WAVE: " + Wave + "\nSubwaves: " + numSubWaves + "\nNumPossibleEnemies: " + numPossibleEnemies);

            // Increase wave spawning speed
            if (subWaveIntervalsTime > _minimumSubWaveInterval) subWaveIntervalsTime -= _waveReductionRate;
        }

        private void waveup5()
        {
            if (numPossibleEnemies <= possibleEnemies.Length - 1 && !_addedEnemyThisWaveSet)
            {
                // Increase kinds of enemies that can be encountered
                numPossibleEnemies += 1;
                _addedEnemyThisWaveSet = true;
                print("Incresed Number of Possible Enemies" + "\nNumPossibleEnemies: " + numPossibleEnemies);
                UpgradeSpawner.UpdateCurrentWave();
            }
        }

        public void waveUpKey()
        {
            if (VirtualInputManager.Instance.plus)
            {
                print("WAVE INCREASE");
                waveUp();

                if (Wave % 5 == 0 && Wave != 0)
                {
                    waveup5();
                }

                UpgradeSpawner.UpdateCurrentWave();
            }
        }

        internal void enableHardMode()
        {
            subWaveIntervalsTime = 4.5f;
        }
        public int getFinalWave()
        {
            return Wave;
        }
    }
}
