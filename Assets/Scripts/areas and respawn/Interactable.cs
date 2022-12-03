using System;
using movement_and_Camera_Scripts;
using UnityEngine;

namespace areas_and_respawn
{
    public abstract class Interactable : MonoBehaviour
    {
        public Vector3 SavedPosition;
        public Quaternion SavedRotation;
        public bool SavedState;  // for toggleable object like lights, gates, or moving platforms

        protected Renderer Renderer;
        // [SerializeField] [Tooltip("Default/Starting Material")]
        protected Material RegularMaterial;
        [SerializeField] [Tooltip("Interactivity Material / Can be picked up material")]
        protected Material selectedMaterial;

        private PlayerController _player;
        
        public void SetUp()
        {
            Renderer = GetComponentInChildren<Renderer>();
            Renderer.enabled = true;
            RegularMaterial = Renderer.material;
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            Initialize();
        }

        protected abstract void Initialize();

        public abstract void Interact();

        public abstract void Save();

        public abstract void Reset();

        public virtual void InRange()
        {
            Renderer.material = selectedMaterial;
            Invoke(nameof(OutOfRange), 1f);
        }

        protected virtual void OutOfRange()
        {
            Transform playerTransform = _player.transform;
            Physics.Raycast(playerTransform.position + Vector3.up / 2, playerTransform.forward,
                out RaycastHit hit, _player.interactDistance);
            if (hit.transform != transform)
            {
                Renderer.material = RegularMaterial;
            }
        }
    }
}
