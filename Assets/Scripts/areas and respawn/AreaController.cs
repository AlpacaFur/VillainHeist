using System;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;

namespace areas_and_respawn
{
    public class AreaController : MonoBehaviour
    {
        private BoxCollider _boundary;

        private BoxCollider _cameraBoundary;
        
        private SubAreaController[] _subAreas;

        public CheckPointController spawnPoint;

        private void Start()
        {
            _boundary = GetComponent<BoxCollider>();
            _boundary.isTrigger = true;
            
            _cameraBoundary = gameObject.AddComponent<BoxCollider>();
            _cameraBoundary.center = _boundary.center - new Vector3(0, 0, 4.5f);
            _cameraBoundary.size = _boundary.size;
            _cameraBoundary.isTrigger = true;
            
            _subAreas = GetComponentsInChildren<SubAreaController>();
            
            Save();
        }

        public void Reset()
        {
           foreach (SubAreaController subArea in _subAreas) // call on sub areas
           {
               subArea.Reset();
           }
        }

        public void Save()
        {
            foreach (SubAreaController subArea in _subAreas) // call on sub areas
            {
                subArea.Save();
            }
        }

        public BoxCollider GetCameraBoundary()
        {
            return _cameraBoundary;
        }
    }
}
