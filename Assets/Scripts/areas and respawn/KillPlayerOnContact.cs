using System;
using movement_and_Camera_Scripts;
using UnityEngine;

namespace areas_and_respawn
{
    public class KillPlayerOnContact : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag($"Player"))
            {
                print("gottem");
                other.GetComponent<PlayerController>().Respawn();
            }
        }
    }
}
