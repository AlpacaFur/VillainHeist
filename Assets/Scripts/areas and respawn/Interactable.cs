using System;
using System.Collections.Generic;
using System.Linq;
using movement_and_Camera_Scripts;
using UnityEngine;

namespace areas_and_respawn
{
    public abstract class Interactable : MonoBehaviour
    {
        public Vector3 SavedPosition;
        public Quaternion SavedRotation;
        public bool SavedState;  // for toggleable object like lights, gates, or moving platforms

        protected List<Renderer> Renderers = new();
        // [SerializeField] [Tooltip("Default/Starting Material")]
        protected List<Material[]> RegularMaterials = new();
        [SerializeField] [Tooltip("Interactivity Material / Can be picked up material")]
        protected Material selectedMaterial;

        private PlayerController _player;
        
        public void SetUp()
        {
            Renderers = GetComponentsInChildren<Renderer>().ToList();
            foreach (Renderer rend in Renderers)
            {
                rend.enabled = true;
                RegularMaterials.Add(rend.materials);
            }
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            Initialize();
        }

        protected abstract void Initialize();

        public abstract void Interact();

        public abstract void Save();

        public abstract void Reset();

        public virtual void InRange()
        {
            SetSelectedMaterials();
            Invoke(nameof(OutOfRange), 1f);
        }

        protected virtual void OutOfRange()
        {
            Transform playerTransform = _player.transform;
            Physics.Raycast(playerTransform.position + Vector3.up / 2, playerTransform.forward,
                out RaycastHit hit, _player.interactDistance);
            if (hit.transform != transform)
            {
                SetRegularMaterials();
            }
        }

        protected void SetRegularMaterials()
        {
            foreach (Renderer rend in Renderers)
            {
                int index = Renderers.IndexOf(rend);
                rend.materials = RegularMaterials[index];
            }
        }

        protected void SetSelectedMaterials()
        {
            foreach (Renderer rend in Renderers)
            {
                int matLen = rend.materials.Length;
                Material[] mats = new Material[matLen];
                Array.Fill(mats, selectedMaterial);
                rend.materials = mats;
            }
        }

        protected void SetRenderers(bool isEnabled)
        {
            foreach (Renderer rend in Renderers)
            {
                rend.enabled = isEnabled;
            }
        }
    }
}
