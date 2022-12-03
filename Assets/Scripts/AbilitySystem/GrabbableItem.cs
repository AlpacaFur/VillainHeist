using areas_and_respawn;
using movement_and_Camera_Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace AbilitySystem
{
    public class GrabbableItem : Interactable
    {
        private PlayerController _playerController;

        private float originalY;

        // [SerializeField]
        // [Tooltip("Normal Material")]
        // private Material regular;

        private bool _isPickedUp;

        private Rigidbody _rigidbody;
        
        // public float playerYRot;
        
        // [SerializeField] private Renderer rend;
        
        protected override void Initialize()
        {
            // rend = GetComponent<MeshRenderer>();
            // regular = Renderer.material;
            // rend.enabled = true;
            _isPickedUp = false;
            this.originalY = this.transform.position.y;
            _playerController = FindObjectOfType<PlayerController>();
            _rigidbody = GetComponent<Rigidbody>();
            
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionX |
                                     RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotation;
        }

        void PickUp()
        {
            this.transform.position = this._playerController.transform.position + new Vector3(0.0f, 20.0f, 0.0f);
            SetRenderers(false);
            this._isPickedUp = true;
            _rigidbody.isKinematic = true;
        }

        void PutDown()
        {
            float pickUpDistance = _playerController.interactDistance;
            Transform playerTransform = this._playerController.transform;
            SetRenderers(true);
            Vector3 position = playerTransform.position;
            this.transform.position = position + playerTransform.forward * pickUpDistance +
                                      new Vector3(0, -position.y + originalY + 0.5f, 0);
            this._isPickedUp = false;
            _rigidbody.isKinematic = false;
        }

        public override void Interact()
        {
            if (_isPickedUp)
            {
                if (_playerController.pickedUpItem == this)
                {
                    _playerController.pickedUpItem = null;
                    PutDown();
                }
            }
            else
            {
                if (_playerController.pickedUpItem is null)
                {
                    _playerController.pickedUpItem = this;
                    PickUp();
                }
            }
        }

        public override void Save()
        {
            Transform t = transform;
            SavedPosition = t.position;
            SavedRotation = t.rotation;
            SavedState = _isPickedUp;
        }

        public override void Reset()
        {
            _isPickedUp = SavedState;
            if (_isPickedUp)
            {
                _playerController.pickedUpItem = this;
                PickUp();
            }
            else
            {
                PutDown();
                Transform t = transform;
                t.position = SavedPosition;
                t.rotation = SavedRotation;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
