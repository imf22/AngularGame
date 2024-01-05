using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace angulargame
{

    public class GameController : MonoBehaviour
    {
        public Canvas ResetMenu;
        public Transform PlayerSpawnPoint;
        public GameObject PlayerPrefab;
        private static GameObject Player;
        private AudioSource GlobalAudio0;
        private Camera MainCamera;
        private HPVisuals HpBarVisual;
        private GameObject SpawnController;
        private GameObject ScoreController;

        // Start is called before the first frame update
        void Start()
        {
            //print("This scene has been loaded");
            // Hide Reset Menu
            ResetMenu.enabled = false;

            // Create Player
            if (Player == null)
            {
                Player = (GameObject) Instantiate(PlayerPrefab, PlayerSpawnPoint);
            
            }

            // Set Global Audio Component
            GlobalAudio0 = GameObject.Find("GlobalAudio").GetComponent<AudioSource>() ;
            
            // Find Main Camera
            MainCamera = Camera.main;
            MainCamera.GetComponent<FollowCamera>().setTarget(Player.transform);

            // Start Spawning
            SpawnController = GameObject.Find("SpawnerController");
            SpawnController.GetComponent<SpawnController>().setPlayer(Player);
            //GameObject.Find("SpawnerController").GetComponent<SpawnController>().startSpawner();

            ScoreController = GameObject.Find("Score");


        }

        public GameObject getPlayer()
        {
            return Player;
        }

        // Update is called once per frame
        void Update()
        {
            StartCoroutine(DeterminePlayerState());
        }

        private IEnumerator DeterminePlayerState()
        {
            yield return new WaitForSeconds(1);
            if (Player == null)
            {
                yield return new WaitForSeconds(3);

                // Get Final Score
                GameObject.Find("FinalScoreText").GetComponent<TMPro.TextMeshProUGUI>().text = ScoreController.GetComponent<ScoreScript>().getFinalScore();
                GameObject.Find("FinalWaveText").GetComponent<TMPro.TextMeshProUGUI>().text = SpawnController.GetComponent<SpawnController>().getFinalWave().ToString();

                ResetMenu.enabled = true;
            }
        }

        public void resetScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void exitGame()
        {
            Application.Quit();
        }

        public void playGlobalSound(AudioClip sound)
        {
            GlobalAudio0.PlayOneShot(sound);
        }

        public void swapCharacter(string character)
        {
            GameObject newplayer = null;

            switch (character)
            {
                case "Triangle":
                    newplayer = (GameObject)LoadPrefabFromFile("PlayerTriangle");
                    break;
                case "Hex":
                    newplayer = (GameObject)LoadPrefabFromFile("PlayerHex");
                    break;
                case "Oct":
                    newplayer = (GameObject)LoadPrefabFromFile("PlayerOct");
                    break;

            }

            GameObject temp = Player;
            Vector3 pos = new Vector3(0, Player.transform.position.y, Player.transform.position.z);
            Player = (GameObject) Instantiate(newplayer, pos, Quaternion.identity);
            Destroy(temp);

            // Reattach Main Camera
            MainCamera.GetComponent<FollowCamera>().setTarget(Player.transform);

            // Reattach Spawner
            SpawnController.GetComponent<SpawnController>().resetPlayer(Player);

            // Update HP
            GameObject.Find("HpBarVisual").GetComponent<HPVisuals>().setPlayer(Player);
            GameObject.Find("HpBarVisual").GetComponent<HPVisuals>().DrawHpBars();




        }

        // Load Resource
        // https://answers.unity.com/questions/313398/is-it-possible-to-get-a-prefab-object-from-its-ass.html
        private UnityEngine.Object LoadPrefabFromFile(string filename)
        {
            Debug.Log("Trying to load LevelPrefab from file (" + filename + ")...");
            var loadedObject = Resources.Load("Player/" + filename);
            if (loadedObject == null)
            {
                throw new System.IO.FileNotFoundException("...no file found - please check the configuration");
            }
            return loadedObject;
        }
    }
}
