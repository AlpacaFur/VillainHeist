using System;
using movement_and_Camera_Scripts;
using UnityEngine;

namespace areas_and_respawn
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] [Tooltip("Other Side Of the Door")]
        private DoorController otherDoor;

        private Transform _enterPosition;

        private RoomController _room;

        private PlayerController _player;

        private BoxCollider _collider;

        private bool _isLocked;

        private void Awake()
        {
            _enterPosition = GetComponentsInChildren<Transform>()[1];
            _room = GetComponentInParent<RoomController>();
            _player = FindObjectOfType<PlayerController>();
            _collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                otherDoor.Teleport();
            }
        }

        private void Teleport()
        {
            if (_player.isFirstPov) return; // Turn off teleporting for 1st person
            _player.transform.position = _enterPosition.position;
            _player.SetRoom(_room);
        }

        private void Update()
        {
            _collider.isTrigger = _player.pickedUpItem is null;
        }

        public void Lock(bool locked)
        {
            _collider.isTrigger = !locked;
        }
    }
}
