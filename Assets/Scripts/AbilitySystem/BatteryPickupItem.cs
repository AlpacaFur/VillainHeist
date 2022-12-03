using UnityEngine;

namespace AbilitySystem
{
    public class BatteryPickupItem : MonoBehaviour
    {

    
        [Tooltip("Insert Battery Storage Object")]
        public Battery bat;
    
        /*
    [SerializeField]
    [Tooltip("Add AbilityInputSystem")]
    public AbilityInputs abilityInputSystem;
*/
    

        [Tooltip("The Player")]
        public GameObject killGuy;

        [SerializeField]
        [Tooltip("Distance Until the item is picked up")]
        public float killDistance;




        // Start is called before the first frame update
        void Start()
        {
            /*
        bat = abilityInputSystem.bat;
        */
        }

        private void OnDestroy()
        {
           this.bat.addToCurrent(2);
        }

        // Update is called once per frame
        void Update()
        {
            if ( Vector3.Distance(killGuy.transform.position, this.transform.position) < this.killDistance)
                {
                    Destroy(this.gameObject);
                }
        }
    }
}
