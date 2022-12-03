using System;
using System.Collections.Generic;
using System.Linq;
using areas_and_respawn;
using Unity.VisualScripting;
using UnityEngine;

namespace AbilitySystem
{
    public class PlateController : Interactable
    {
        [SerializeField] [Tooltip("Block to trigger event when placed.")]
        private GrabbableItem key;
        
        private readonly List<LockedDoorAidan> _locks = new();

        [SerializeField] [Tooltip("Invert the state for when block is placed.")]
        private bool inverted;

        private bool _triggered;

        protected override void Initialize()
        {
            _triggered = inverted;
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.transform.TryGetComponent(out GrabbableItem grabbable))
            {
                if (grabbable == key)
                {
                    _triggered = true ^ inverted;
                    SetSelectedMaterials();
                }
            }
        }
        
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Interactable"))
            {
                _triggered = false ^ inverted;
                SetRegularMaterials();
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (LockedDoorAidan lockedDoor in _locks)
            {
                lockedDoor.TryUnlock();
            }
        }

        public void AddDoor(LockedDoorAidan lockedDoor)
        {
            _locks.Add(lockedDoor);
        }

        public bool IsTriggered()
        {
            return _triggered;
        }

        public override void Interact() {} // No interaction

        public override void Save()
        {
            SavedState = _triggered;
        }

        public override void Reset()
        {
            _triggered = SavedState;
        }
    }
}
