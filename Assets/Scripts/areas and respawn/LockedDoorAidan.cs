using System;
using AbilitySystem;
using UnityEngine;

namespace areas_and_respawn
{
    public class LockedDoorAidan : Interactable
    {
        private bool _isLocked = true;
        
        [SerializeField] [Tooltip("Invert the state for when block is placed.")]
        private bool inverted;

        [SerializeField] private DoorController[] doors;

        [SerializeField] [Tooltip("Plates that need to be active to unlock.")] 
        private PlateController[] triggers;
        
        private bool _displaying;

        protected override void Initialize()
        {
            foreach (PlateController trigger in triggers)
            {
                trigger.AddDoor(this);
                trigger.SetUp();
            }
        }

        public void TryUnlock()
        {
            bool checkTriggers = true;
            foreach (PlateController trigger in triggers)
            {
                checkTriggers &= trigger.IsTriggered() ^ inverted;
            }
            _isLocked = !checkTriggers;
            gameObject.SetActive(_isLocked);
            foreach (DoorController door in doors)
            {
                door.Lock(_isLocked);
            }
        }

        public override void Interact() {} // Locked doors are not interactable

        public override void Save()
        {
            SavedState = _isLocked;
        }

        public override void Reset()
        {
            _isLocked = SavedState;
            foreach (DoorController door in doors)
            {
                door.Lock(_isLocked);
            }
        }

        public override void InRange()
        {
            if (!_displaying)
            {
                _displaying = true;
                //TODO: send message that door is locked
                print("LOCKED");
                Invoke(nameof(OutOfRange), 2f);
            }
        }

        protected override void OutOfRange()
        {
            _displaying = false;
        }
    }
}
