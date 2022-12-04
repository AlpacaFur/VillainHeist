using System;
using movement_and_Camera_Scripts;
using UnityEngine;

namespace areas_and_respawn
{
    public class CheckPointController : MonoBehaviour
    {
        private Transform _spawnPoint;
        
        private RoomController _room;

        private AreaController _area;

        private bool _isSpawn;

        private AudioSource checkPointAS;

        // Start is called before the first frame update
        void Start()
        {
            _spawnPoint = transform;
            _room = GetComponentInParent<RoomController>();
            _area = GetComponentInParent<AreaController>();
            _rend = GetComponentInChildren<Renderer>();
            _isSpawn = _room is null;
            if (_isSpawn) _room = GameObject.FindWithTag("Start Room").GetComponent<RoomController>();
            checkPointAS = GetComponent<AudioSource>();
        }

        public void Respawn(PlayerController player)
        {
            player.transform.position = _spawnPoint.position;
            player.SetRoom(_room);
            if (_isSpawn)
            {
                _area.Reset();
            }
            else
            {
                _room.Reset();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                if (player.checkpoint != this && !player.isFirstPov)
                {
                    checkPointAS.Play();
                    player.SetCheckpoint(this);
                    _area.Save();
                    
                }
            }
        }

        public void DeselectCheckpoint()
        {
            _rend.materials[2] = openCheckpoint;
        }
    }
}
