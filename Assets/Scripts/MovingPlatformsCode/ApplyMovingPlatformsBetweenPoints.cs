using System.Collections.Generic;
using movement_and_Camera_Scripts;
using UnityEngine;

namespace MovingPlatformsCode
{
    public class ApplyMovingPlatformsBetweenPoints : ApplyEverySecsFunc
    {
        //private Rigidbody myRigidbody;
        //private CharacterController controller;
        [Header("Movement Prep")]
        [SerializeField]
        [Tooltip("Insert the empty gameobject that stores the end position of this moving platform here")]
        private Transform[] goToTransform;
        private int _nextPos;
        private Vector3[] _endPos;
        private Vector3 _startPos;
        private static readonly string[] MovableTags = { "Player", "Respawn"  };

        [Header("Current Movement")]
        [SerializeField]
        private float speed = .5f;
        private Vector3 direction;
        private bool _running;
        private Vector3 _targetLoc;
        private Vector3 _prevTargetLoc;
        private List<PlayerController> players;

        [Header("Frozen")]
        private float prevSpeed;
        private bool isFrozen = false;

        private void Awake()
        {
            players = new List<PlayerController>();
            //controller = GetComponent<CharacterController>();
            //myRigidbody = GetComponent<Rigidbody>();
            _nextPos = 0;
            _endPos = new Vector3[goToTransform.Length];
            for (int i = 0; i < goToTransform.Length; i++)
            {
                _endPos[i] = goToTransform[i].localPosition;
            }
            _startPos = transform.localPosition;
        }

        private void Update()
        {
            if (_running)
            {
                Move();
                CheckEnd();
            }
            /*else
        {
            myRigidbody.velocity = new Vector3(0,0,0);
        }*/
        }

        public override void StartApply()
        {
            _running = true;
            _prevTargetLoc = transform.localPosition;
            if (_nextPos < _endPos.Length)
            {
                _targetLoc = _endPos[_nextPos];
                _nextPos++;
            }
            else
            {
                _targetLoc = _startPos;
                _nextPos = 0;
            }
            //direction = (_targetLoc - transform.position).normalized;
        }

        public override void TurnOff()
        {
            //Do nothing and have the platform stop at the next point
        }

        /*public override void Unapply()
    {
        running = true;
        targetLoc = startPos;
    }*/

        // Moves this platform to the current target pos
        private void Move()
        {
            Vector3 localMovement = (_targetLoc - transform.localPosition).normalized/*direction.normalized*/ * speed * Time.deltaTime;
            transform.Translate(localMovement);
            foreach (PlayerController player in players)
            {
                player.GetComponent<CharacterController>().Move(transform.TransformVector(localMovement).normalized * speed * Time.deltaTime);
            }
            /*if (CollidingWithWall())
        {
            transform.Translate(/*(_targetLoc - transform.position).normalized*-direction.normalized * speed * Time.deltaTime);
            // Undo the translation
        }*/
            //myRigidbody.velocity = direction.normalized * speed;
            //controller.Move(direction.normalized * speed * Time.deltaTime);
        }

        // Ends the current method (whether Apply or UnApply) if the platform has reached its destination
        // This means that the distance between its current location and its target is 0
        private void CheckEnd()
        {
            if (AtTarget())
            {
                _running = false;
                // This means we have reached our target location
                /*if (targetLoc.Equals(endPos))
            {
                Debug.Log("Apply is Done");
                // This means we are running apply
                SetApplyDone();
            }
            else if (targetLoc.Equals(startPos))
            {
                Debug.Log("Unapply is Done");
                // This means we are running unapply
                SetUnapplyDone();
            }*/
                SetApplyDone();
            }
        }

        // Returns true if this platform made it to its target and false otherwise
        private bool AtTarget()
        {
            //return ((targetLoc - transform.position).magnitude < .1f);
            //Vector3 origPos = prevTargetLoc;
            // If this platform is farther from the non-target position than the
            // target location is, then it has passed the target
            return ((transform.localPosition - _prevTargetLoc).magnitude > (_targetLoc - _prevTargetLoc).magnitude);
        }

        // Runs when an object is placed on this
        private void OnTriggerEnter(Collider other)
        {
            // Adds all objects with certain tags on top of this object as a child
            // This way, all objects on top of this one will move with this object
            /*foreach (string tag in MovableTags)
        {
            if (other.CompareTag(tag))
            {
                other.transform.SetParent(transform);
            }
        }*/
            if (other.CompareTag("Player"))
            {
                players.Add(other.GetComponent<PlayerController>());
                Debug.Log("Got Player");
            }
        }

        // Runs when an object is removed from this
        private void OnTriggerExit(Collider other)
        {
            // Removes all objects that were added as children when they leave this platform
            /*foreach (string tag in MovableTags)
        {
            // The object on this one only stops being a child of this object if it is
            // still a child of this object and has not been snatched by another object
            // and had its parent changed.
            if (other.CompareTag(tag) && other.transform.parent.Equals(transform))
            {
                other.transform.SetParent(null);
            }
        }*/
            if (other.CompareTag("Player"))
            {
                PlayerController otherController = other.GetComponent<PlayerController>();
                if (players.Contains(otherController))
                {
                    players.Remove(otherController);
                    Debug.Log("Left Player");
                }
            }
        }

        public override void ToggleFreeze()
        {
            isFrozen = !isFrozen;
            if (isFrozen)
            {
                prevSpeed = speed;
                speed = 0;
            }
            else
            {
                speed = prevSpeed;
            }
        }

        protected override void Initialize()
        {
            // NOTHING
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
