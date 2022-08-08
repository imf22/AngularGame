using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace angulargame
{
    public class PlatformController : MonoBehaviour
    {
        public float playerHealth = 4;
        public float walkSpeed = 10f;
        public float jumpForce = 10f;
        public Transform groundCheck;
        public float groundCheckRadius = 0.2f;
        public LayerMask groundMask;
        
    
        float dashTimer;

        private Rigidbody _rigidbody;
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

        private Coroutine _DashingCo;
        private Coroutine _DestroyEnemyCoRu = null;
        private Coroutine _DestroyProjectileCoRu = null;
        private BoxCollider _collider;

        public AudioSource _audioSource;
        public AudioClip DeathKickSFX;
        public AudioClip DashSFX;
        public AudioClip DropSFX;
        public AudioClip JumpSFX;
        public AudioClip HitSFX;
        public AudioClip DeathSFX;
        private bool jumpSFXPlayed = false;


        private Collider[] destroyedEnemies;
        private Collider[] destroyedProjectiles;

        public bool newPlayerGODesign = true;


        // Start is called before the first frame update
        void Start()
        {
            // For Normal movement
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();

            _scaleZ = transform.localScale.z;

            // Destory Projectiles and Enemies
            destroyedEnemies = Physics.OverlapBox(new Vector3(0, -10, 0), Vector3.zero);
            _DestroyProjectileCoRu = StartCoroutine(SequentialDestroy(destroyedEnemies));
            _DestroyEnemyCoRu = StartCoroutine(SequentialDestroy(destroyedEnemies));

            // Connect to Hp UI
            GameObject.Find("GameManager").GetComponent<HealthVisuals>().GetComponent<tempShowPlayerHealth>().setPlayer(this.gameObject);
            GameObject.Find("GameManager").GetComponent<HealthVisuals>().GetComponent<tempShowPlayerHealth>().startHpUI();
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
            if (_DestroyProjectileCoRu != null)
            {
                StopCoroutine(_DestroyProjectileCoRu);
                _DestroyProjectileCoRu = StartCoroutine(SequentialDestroy(destroyedProjectiles));
            }

            if (_DestroyEnemyCoRu != null)
            {
                StopCoroutine(_DestroyEnemyCoRu);
                _DestroyEnemyCoRu = StartCoroutine(SequentialDestroy(destroyedEnemies));
            }

            // TODO : Activate trail effect here
            yield return new WaitForSeconds(dashtime);


            // END DASHING
            // TODO : Diable trail effect here
            _rigidbody.useGravity = true;
            //_isDashing = false;
            //_collider.enabled = true;


            //Dash cooldown
            if (_DestroyEnemyCoRu != null)
            {
                // Give small vertical boost on kill and reset dash
                _rigidbody.velocity = new Vector3(0, 1, 0);
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
                        termsList.Add(setToDestroy[j]);
                        Destroy(setToDestroy[j].transform.parent.gameObject);

                        //TODO: Move enemy destroyed sound handling to enemy or a handler.
                        _audioSource.PlayOneShot(DeathKickSFX);

                        yield return new WaitForSeconds(0.15f);
                    }
                }
            }

            yield return null;
        }

        public void reducePlayerHealth(float dmg)
        {
            if (!_isDashing)
            {

                playerHealth -= dmg;

                if (playerHealth <= 0)
                {
                    if (newPlayerGODesign)
                    {
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        Destroy(this.transform.parent.gameObject);
                    }
                }

            }
        }

        public float getPlayerHealth()
        {
            return this.playerHealth;
        }
    }

}
