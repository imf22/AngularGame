using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace angulargame
{
    public class PlatformController : MonoBehaviour
    {
        public int maxPlayerHealth = 4;
        public int playerHealth = 4;
        public float walkSpeed = 10f;
        public float jumpForce = 10f;
        public Transform groundCheck;
        private Rigidbody _rigidbody;

        public float groundCheckRadius = 0.2f;
        public LayerMask groundMask;
        private FireProjectile ProjectileLauncher;

        private bool _isGrounded;
        private float _scaleZ;
        private float _moveInput;
        private bool _canDrop = false;

        public float DashCooldown = 3f;
        public float dashPower = 15f;
        private float dashtime = 0.2f;
        public float dashDivsor;
        public bool debugDash = false;
        public LayerMask enemyMask;
        public LayerMask projectileMask;
        private bool _canDash = true;
        private bool _isDashing = false;
        private Vector3 hitbox;
        float dashTimer;
        private float _MINIMUM_DASH_COOLDOWN = 0.6f;

        private Coroutine _DashingCo;
        private Coroutine _DestroyEnemyCoRu = null;
        private Coroutine _DestroyProjectileCoRu = null;

        private BoxCollider _collider;
        //private MeshCollider _collider;

        private AudioSource GlobalAudio;
        public AudioSource _audioSource;
        public AudioClip DeathKickSFX;
        public AudioClip DashSFX;
        public AudioClip DropSFX;
        public AudioClip JumpSFX;
        public AudioClip HitSFX;
        public AudioClip DeathSFX;
        private bool jumpSFXPlayed = false;

        public GameObject DeathVFX;


        private Collider[] destroyedEnemies;
        private Collider[] destroyedProjectiles;

        public bool newPlayerGODesign = true;

        public static event Action OnPlayerDamaged;
        public static event Action OnPlayerDeath;

        //private GameObject ScoringSystem;
        private ScoreScript ScoringSystem;


        // Start is called before the first frame update
        void Start()
        {
            // Set player object name
            this.gameObject.name = "PlayerCube";

            // Get Projectile Component
            ProjectileLauncher = this.GetComponentInChildren<FireProjectile>();

            // Get Global audio source
            GlobalAudio = GameObject.Find("GlobalAudioSource0").GetComponent<AudioSource>();

            // For Normal movement
            _rigidbody = GetComponent<Rigidbody>();

            _collider = GetComponent<BoxCollider>();
            //_collider = GetComponent<MeshCollider>();

            _scaleZ = transform.localScale.z;

            // Destory Projectiles and Enemies
            destroyedEnemies = Physics.OverlapBox(new Vector3(0, -10, 0), Vector3.zero);
            _DestroyProjectileCoRu = StartCoroutine(SequentialDestroy(destroyedEnemies));
            _DestroyEnemyCoRu = StartCoroutine(SequentialDestroy(destroyedEnemies));

            // Connect to Hp UI
            GameObject temp = GameObject.Find("hpDisplay");
            temp.GetComponent<tempShowPlayerHealth>().setPlayer(this.gameObject);
            temp.GetComponent<tempShowPlayerHealth>().startHpUI();

            ScoringSystem = GameObject.Find("Score").GetComponent<ScoreScript>();

        }

        // Update is called once per frame
        void Update()
        {
            // Do nothing if already dashing
            if (_isDashing)
            {
                return;
            }

            //Movment Direction
            _moveInput = Input.GetAxis("Horizontal");

            // Determine if character is on ground for jump
            if(Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundMask).Length > 0)
            {
                _isGrounded = true;
                ScoringSystem.resetCombo();

            }
            else
            {
                _isGrounded = false;
            }

            // Dash timer countdown
            dashTimer -= Time.deltaTime;


            // Actions
            Jump();

            Drop();

            reset();

            if (VirtualInputManager.Instance.Dash && _canDash)
            {
                _DashingCo = StartCoroutine(Dash());
            }

            if (VirtualInputManager.Instance.plus)
            {
                print("WAVE INCREASE");
                GameObject.Find("SpawnerController").GetComponent<SpawnController>().waveUpKey();
            }

            if (VirtualInputManager.Instance.hardMode)
            {
                print("HARD MODE");
                GameObject.Find("SpawnerController").GetComponent<SpawnController>().enableHardMode();
            }

            if (VirtualInputManager.Instance.pause)
            { 
                GameObject.Find("GameManager").GetComponent<pauseMenu>().Pause();
            }


        }


        private void FixedUpdate()
        {
            Flip();
            Move();
        }

        private void reset()
        {
            if (Input.GetKey(KeyCode.R))
            {
                this.gameObject.transform.position = Vector3.up;
            }
        }
        
        private void Move()
        {
            //_rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, _scaleZ * walkSpeed * _moveInput);

            if (!_isDashing)
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, _scaleZ * walkSpeed * _moveInput);
            }
        }

        private void Jump()
        {

            if (VirtualInputManager.Instance.Jump && _isGrounded)
            {
                _isGrounded = false;
                _canDrop = true;
                _rigidbody.velocity = new Vector3(0, jumpForce, _rigidbody.velocity.z);
                _audioSource.PlayOneShot(JumpSFX);
                jumpSFXPlayed = true;
            }

        }

        private void Drop()
        {
            if (VirtualInputManager.Instance.Drop && !_isGrounded && _canDrop)
            {
                if (Math.Abs(_rigidbody.velocity.y) < 1)
                {
                    _rigidbody.velocity = new Vector3(0, dashPower/2 * -1, _rigidbody.velocity.z);
                    _canDrop = false;
                }
                else
                {
                    _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y * -1, _rigidbody.velocity.z);
                    _canDrop = false;
                }
                //Drop SFX
                _audioSource.PlayOneShot(DropSFX);
            }
        }

        private void Flip()
        {
            // Face Player in Correct direction
            if(_moveInput > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, _scaleZ);
                //this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if(_moveInput < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, _scaleZ * -1);
                //this.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

        }


        private IEnumerator Dash()
        {
            _canDash = false;
            _isDashing = true;
            _rigidbody.useGravity = false;

            // Determine dash direction
            int dir;
            if (VirtualInputManager.Instance.MoveLeft)
            {
                dir = -1;
            }
            else if (VirtualInputManager.Instance.MoveRight)
            {
                dir = 1;
            }
            else { dir = 0; }

            // BEGIN DASHING: Apply force
            _rigidbody.velocity = new Vector3(0, 0, dir * dashPower);
            //_collider.enabled = false;

            //Dash SFX
            _audioSource.PlayOneShot(DashSFX);

            // Dash hitbox
            hitbox = new Vector3(0, _rigidbody.transform.position.y, _rigidbody.transform.position.z + ((dashPower / dashDivsor)/2) * dir);

            destroyedEnemies = Physics.OverlapBox(hitbox, new Vector3(1, 1, (dashPower / dashDivsor)), Quaternion.Euler(0,0,0), enemyMask);
            
            destroyedProjectiles = Physics.OverlapBox(hitbox, new Vector3(1, 1, (dashPower / dashDivsor)), Quaternion.Euler(0, 0, 0), projectileMask);

            //Debugging: Show Dash Hit Boxes
            if (debugDash) CreateDashBox();

            // Set collided projectiles and enemies to be destroyed
            // NOTE TO SELF: These coroutines will always trigger making this check unecessary 
            if (destroyedProjectiles.Length >= 1)
            //if (_DestroyProjectileCoRu != null)
            {
                StopCoroutine(_DestroyProjectileCoRu);
                _DestroyProjectileCoRu = StartCoroutine(SequentialDestroy(destroyedProjectiles));
            }

            //if (_DestroyEnemyCoRu != null)
            if (destroyedEnemies.Length >= 1)
            {
                ScoringSystem.addCombo();
                StopCoroutine(_DestroyEnemyCoRu);
                _DestroyEnemyCoRu = StartCoroutine(SequentialDestroy(destroyedEnemies));
            }

            // TODO : Activate trail effect here
            yield return new WaitForSeconds(dashtime);


            // END DASHING
            // TODO : Diable trail effect here
            _rigidbody.useGravity = true;


            //Dash cooldown
            //Debug.Log("Active: " + _DestroyEnemyCoRu);
            if (destroyedEnemies.Length >= 1)
            {
                // Give small vertical boost on kill and reset dash
                _rigidbody.velocity = new Vector3(0, 5, 0);
                yield return new WaitForSeconds(0.1f);
                _isDashing = false;
                //_collider.enabled = true;
                _canDash = true;
            }
            else
            {
                // Standard end dash
                _isDashing = false;
                _collider.enabled = true;

                yield return new WaitForSeconds(DashCooldown);
                _canDash = true;
            }

    }

        private void CreateDashBox()
        {
            //DEBUG where is dash hitbox
            GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
            c.transform.position = hitbox;
            c.transform.localScale = new Vector3(1, 1, (dashPower / dashDivsor));
            Collider cC = c.GetComponent<Collider>();
            cC.enabled = false;
            Destroy(c, 1.0f);
        }

        private IEnumerator SequentialDestroy(Collider[] setToDestroy)
        {
            List<Collider> termsList = new List<Collider>();

            if (setToDestroy.Length > 0)
            {
                for (int j = 0; j < setToDestroy.Length; j++)
                {
                    if (termsList.Contains(setToDestroy[j]))
                    {
                        continue;
                    }
                    else 
                    {
                        if(setToDestroy[j] != null)
                        {
                            termsList.Add(setToDestroy[j]);
                            if(setToDestroy[j].tag == "Projectile")
                            {
                                Destroy(setToDestroy[j].gameObject);
                            }
                            else
                            {
                                setToDestroy[j].transform.parent.gameObject.GetComponent<BasicEnemyAI>().destorySelfScoring(10);
                                //Destroy(setToDestroy[j].transform.parent.gameObject);
                            }
                        }
                        
                        //TODO: Move enemy destroyed sound handling to enemy or a handler.
                        _audioSource.PlayOneShot(DeathKickSFX);

                        yield return new WaitForSeconds(0.15f);
                    }
                }
            }

            yield return null;
        }

        public void reducePlayerHealth(int dmg)
        {
            if (!_isDashing)
            {

                playerHealth -= dmg;
                OnPlayerDamaged?.Invoke();

                GlobalAudio.PlayOneShot(HitSFX);

                if (playerHealth <= 0)
                {
                    // Play Death Sound and SFS
                    GlobalAudio.PlayOneShot(DeathSFX);
                    GameObject Explosion = (GameObject)Instantiate(DeathVFX, this.transform.position, Quaternion.identity);
                    if (newPlayerGODesign)
                    {
                        OnPlayerDeath?.Invoke();
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        OnPlayerDeath?.Invoke();
                        Destroy(this.transform.parent.gameObject);
                    }

                }

            }
        }

        public void healAmount(int hpRecovered)
        {
            playerHealth += hpRecovered;

            if (playerHealth > maxPlayerHealth) playerHealth = maxPlayerHealth;
        }

        public int getPlayerHealth()
        {
            return this.playerHealth;
        }
        
        public int getMaxPlayerHealth()
        {
            return this.maxPlayerHealth;
        }

        // UPGRADE METHODS

        // Swap Character
        internal void swapCharacter(string character)
        {
            GameController GC = GameObject.Find("GameManager").GetComponent<GameController>();

            GC.swapCharacter(character);
            
        }

        // Player Movement Upgrades
        internal void upgradeMovementSpeed()
        {
            walkSpeed += 2.0f;
        }


        internal void upgradeJumpPower()
        {
            jumpForce += 2.0f;
        }

        internal void upgradeDashPower()
        {
            dashPower += 2.0f;
        }


        internal void upgradeDashCooldown()
        {
            if (DashCooldown > _MINIMUM_DASH_COOLDOWN)
            {
                DashCooldown -= 0.2f;
            }
        }

        // Projectile Launcher Upgrades
        public void upgradeFireRate()
        {
            ProjectileLauncher.upgradeFireRate();
        }

        public void upgradeProjSize()
        {
            ProjectileLauncher.upgradeProjSize();
        }
        public void upgradeProjSpeedUp()
        {
            ProjectileLauncher.upgradeProjSpeedUp();
        }
        public void upgradeProjSpeedDown()
        {
            ProjectileLauncher.upgradeProjSpeedDown();
        }
        public void upgradeWideShot()
        {
            ProjectileLauncher.upgradeWideShot();
        }

        // Get Stats For new player
        public Dictionary<string, float> getPlayerStats()
        {
            Dictionary<string, float> stats = new Dictionary<string, float>();

            

            return stats;
        }

        
    }
}
