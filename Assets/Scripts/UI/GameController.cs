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
        private GameObject Player;
        private AudioSource GlobalAudio0;
        private Camera MainCamera;

        // Start is called before the first frame update
        void Start()
        {
            //print("This scene has been loaded");
            // Hide Reset Menu
            ResetMenu.enabled = false;

            // Create Player
            if (Player != null)
            {
                print("halp");
            }
                Player = (GameObject) Instantiate(PlayerPrefab, PlayerSpawnPoint);

            GlobalAudio0 = GameObject.Find("GlobalAudio").GetComponent<AudioSource>() ;
            MainCamera = Camera.main;

            // Find player
            //Player = GameObject.Find("Player");
            //hand = GameObject.Find("Hand");
        }

        public GameObject getPlayer()
        {
            return this.Player;
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
    }
}
