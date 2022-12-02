using System.Collections.Generic;
using System.Linq;
using movement_and_Camera_Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace areas_and_respawn
{
    public class RoomController : MonoBehaviour
    {
        private RoomController[] _subAreas;

        private readonly List<Interactable> _intractables = new();
        
        private BoxCollider _boundary;

        private BoxCollider _cameraBoundary;

        public void SetUp()
        {
            _boundary = GetComponent<BoxCollider>();
            _boundary.isTrigger = true;
            
            _cameraBoundary = gameObject.AddComponent<BoxCollider>();
            _cameraBoundary.center = _boundary.center - new Vector3(0, 0, 4.67f);
            _cameraBoundary.size = _boundary.size;
            _cameraBoundary.isTrigger = true;
            
            _subAreas = GetComponentsInChildren<RoomController>();
            foreach (Transform t in transform)
            {
                if (t.CompareTag($"Interactable"))
                {
                    Interactable interactable = t.gameObject.GetComponent<Interactable>();
                    interactable.SetUp();
                    _intractables.Add(interactable);
                }
            }
        }

        public void Save()
        {
            foreach (Interactable i in _intractables) i.Save();
        }

        public void Reset()
        {
            foreach (Interactable i in _intractables) i.Reset();
            for (int i = 1; i < _subAreas.Length; i++) // call on sub areas, exclude own area
            {
                _subAreas[i].Reset();
            }
        }

        public BoxCollider GetCameraBoundary()
        {
            if (_boundary is not null) return _cameraBoundary;
            return _cameraBoundary;
        }
    }
}
