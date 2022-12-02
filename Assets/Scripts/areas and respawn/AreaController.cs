using System;
using System.ComponentModel;
using JetBrains.Annotations;
using movement_and_Camera_Scripts;
using UnityEngine;
using UnityEngine.UIElements;

namespace areas_and_respawn
{
    public class AreaController : MonoBehaviour
    {
        private RoomController[] _subRooms;

        public CheckPointController spawnPoint;

        private void Start()
        {
            _subRooms = GetComponentsInChildren<RoomController>();
            foreach (RoomController subRoom in _subRooms) // call on sub areas
            {
                subRoom.SetUp();
                subRoom.Save();
                if (subRoom.CompareTag($"Start Room"))
                {
                    PlayerController playerController = FindObjectOfType<PlayerController>();
                    playerController.SetRoom(subRoom);
                    playerController.checkpoint = spawnPoint;
                    FindObjectOfType<PlayerController>().transform.position = spawnPoint.transform.position;
                }
            }
        }

        public void Reset()
        {
           foreach (RoomController subArea in _subRooms) // call on sub areas
           {
               subArea.Reset();
           }
        }
    }
}
