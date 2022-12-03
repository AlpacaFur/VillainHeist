using areas_and_respawn;
using UnityEngine;

namespace Lighting_Scripts
{
    public class OvalLightController : Interactable
    {
        // [SerializeField] private bool selected;
        // [SerializeField] private Material highlightMaterial;
        // [SerializeField] private Material defaultMaterial;
        private Light _lightComponent;
        // private MeshRenderer _materialComponent;

        private bool _lightOn;
        
        protected override void Initialize()
        {
            _lightComponent = GetComponent<Light>();
            // _materialComponent = GetComponent<MeshRenderer>();
            
            // selected = false;
            _lightOn = false;
            ToggleLight();
        }

        public override void Interact()
        {
            ToggleLight();
        }

        public override void Save()
        {
            SavedState = _lightOn;
        }

        public override void Reset()
        {
            _lightOn = !SavedState;
            ToggleLight();
        }

        // Update is called once per frame
        // void Update()
        // {
        //     if (selected)
        //     {
        //         _materialComponent.material = highlightMaterial;
        //         /*if (Input.GetKeyUp("e"))
        //     {
        //         ToggleLight();
        //     }*/
        //     }
        //     else
        //     {
        //         _materialComponent.material = defaultMaterial;
        //     }
        // }

        // Toggle the associated LightSource
        public void ToggleLight()
        {
            _lightOn = !_lightOn;
            _lightComponent.enabled = _lightOn;
        }

        // void deselect()
        // {
        //     selected = false;
        // }
        //
        // void select()
        // {
        //     selected = true;
        // }
    }
}
