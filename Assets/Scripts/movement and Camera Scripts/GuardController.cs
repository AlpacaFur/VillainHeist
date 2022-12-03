using System.Collections;
using System.Linq;
using AbilitySystem;
using areas_and_respawn;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Transform = UnityEngine.Transform;

namespace movement_and_Camera_Scripts
{
    public class GuardController : Interactable
    {
        public Transform[] points;
        private Vector3[] _vertices;
        private Vector3 _next;
        private Vector3 _prev;
        private int _index;
        private bool _forwards = true;
        private bool _moving;
        private GameObject player;
        private PlayerController playerController;
        
        [SerializeField]
        private Animator _animator;
        
        [SerializeField] private float speed;
        [SerializeField] private float viewAngle;
        [SerializeField] private float range;
        [SerializeField] private float pauseTime;
        [SerializeField] [Tooltip("If true guard will freeze for pauseTime Seconds" +
                                  "at each point in points[], if False, guard will pause only at the ends")] 
        private bool pauseOnAll;
        [Header("Freezing")]
        private bool isFrozen = false;

        // Guard can taze player if true. Set to false after tazing player, then true after a cooldown.
        private bool canTaze;
        
        // Start is called before the first frame update
        void Start()
        {
            _vertices = new [] { transform.position };
            foreach (Transform t in points)
            {
                _vertices = _vertices.Concat(new [] {t.position}).ToArray();
            }
            if (_vertices.Length > 1)
            {
                _index = 1;
                _next = _vertices[_index];
                _prev = _vertices[0];
                _moving = true;
                Rotate();
            }
            
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_moving)
            {
                Move();
            }
            AttackPlayer();
        }
        
        private void AttackPlayer()
        {
            Vector3 toTarget = player.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, toTarget) <= viewAngle)
            {
                // Guard behavior when player is in top down
                if (playerController.isFirstPov == false)
                {
                    topDownAttack(toTarget);
                }

                // Guard behavior when player is in first person
                else
                {
                    firstPersonAttack(toTarget);
                }
            }
        }

        private void topDownAttack(Vector3 toTarget)
        {
            Debug.DrawRay(transform.position + Vector3.up, toTarget.normalized * range, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up, toTarget, out RaycastHit hit, range))
            {
                if (hit.transform == player.transform)
                {
                    _animator.SetTrigger("Alert");
                    Debug.Log("HIT");
                    playerController.Respawn();
                }
            }
        }
        
        private void firstPersonAttack(Vector3 toTarget)
        {
            Debug.DrawRay(transform.position + Vector3.up, toTarget.normalized * range, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up, toTarget, out RaycastHit hit, range))
            {
                _animator.SetTrigger("Alert");
                if (hit.transform == player.transform)
                {
                    Debug.Log("HIT");
                    playerController.Tased();
                    canTaze = false;
                    Invoke("EnableTaze", 5f);
                }
            }
        }

        private void EnableTaze()
        {
            canTaze = true;
        }

        private void Move()
        {
            if (transform.position != _next)
            {
                transform.position = Vector3.MoveTowards(transform.position, _next,
                    speed * Time.deltaTime);
            }
            else
            {
                if (_forwards)
                {
                    if (_index < points.Length)
                    {
                        _index++;
                        _prev = _next;
                        _next = _vertices[_index];
                        if (pauseOnAll)
                        {
                            _moving = false;
                            Invoke(nameof(StartMoving), pauseTime);
                        }
                    }
                    else
                    {
                        _forwards = false;
                        _moving = false;
                        _prev = _next;
                        _index--;
                        _next = _vertices[_index];
                        Invoke(nameof(StartMoving), pauseTime);
                    }
                }
                else
                {
                    if (_index > 0)
                    {
                        _index--;
                        _prev = _next;
                        _next = _vertices[_index];
                        if (pauseOnAll)
                        {
                            _moving = false;
                            Invoke(nameof(StartMoving), pauseTime);
                        }
                    }
                    else
                    {
                        _forwards = true;
                        _moving = false;
                        _prev = _next;
                        _index++;
                        _next = _vertices[_index];
                        Invoke(nameof(StartMoving), pauseTime);
                    }
                }
                Rotate();
            }
        }

        private void Rotate()
        {
            transform.rotation = Quaternion.LookRotation(_next - _prev, Vector3.up);
        }
        
        private void StartMoving()
        {
            _moving = true;
        }

        /**
         * Toggles if this guard is frozen. If they are, they stop moving and no longer are waiting to move.
         * If they are not, they start moving as normal.
         */
        private void ToggleFreeze()
        {
            isFrozen = !isFrozen;
            if (isFrozen)
            {
                _animator.SetBool("Sleeping", true);
                CancelInvoke(nameof(StartMoving));
                _moving = false;
            }
            else
            {
                _animator.SetBool("Sleeping", false);
                _moving = true;
            }
        }

        protected override void Initialize()
        {
            // NOTHING
            print("INITIALIZING ROBOT");
        }

        public override void Interact()
        {
            ToggleFreeze();
        }

        public override void Save()
        {
            Transform t = transform;
            SavedPosition = t.position;
            SavedState = isFrozen;
        }

        public override void Reset()
        {
            if (SavedState)
            {
                Transform t = transform;
                t.position = SavedPosition;
                isFrozen = false;
                ToggleFreeze();
            }
            else
            {
                Initialize();
            }
        }
    }
}
