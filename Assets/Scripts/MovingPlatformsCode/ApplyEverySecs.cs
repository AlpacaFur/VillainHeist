using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using UnityEngine;

namespace MovingPlatformsCode
{
    // This will apply a given ApplyUnapplyEverySecsFunc or ApplyEverySecsFunc
// every given number of seconds.
    public class ApplyEverySecs : MonoBehaviour
    {
        [SerializeField]
        private float seconds = 0;
        private ApplyEverySecsFunc _func;
        [SerializeField]
        private bool startOn = true;
        private bool _isOn;
        private bool _isOff;
        // Start is called before the first frame update
        void Start()
        {
            SetActive(startOn);
        }

        // Applies and unapplies a method every given seconds sec
        private IEnumerator ApplyEverySeconds()
        {
            while (_isOn)
            {
                _func.Apply();
                yield return new WaitUntil(() => _func.FinishedApplying());
                yield return new WaitForSeconds(seconds);
            }
            if (!_isOff)
            {
                TurnOff();
            }
        }

        // Returns the state before running apply or the state after running unapply
        public void TurnOff()
        {
            _isOn = false;
            _isOff = true;
            _func.TurnOff();
        }

        // Restarts this script
        public void TurnOn()
        {
            _isOff = false;
            _isOn = true;
            _func = GetComponent<ApplyEverySecsFunc>();
            StartCoroutine(ApplyEverySeconds());
        }

        // Sets if this script is running or not
        // If this script is no longer running, this will return to the unapply state or the state before apply
        public void SetActive(bool active)
        {
            if (active && !_isOn)
            {
                TurnOn();
            }
            else if (!_isOff)
            {
                TurnOff();
            }
        }
    }

// A class that will apply and unapply itself every couple of seconds
    public abstract class ApplyUnapplyEverySecsFunc : ApplyEverySecsFunc
    {
        private float _seconds;
        //private bool applyIsDone = false;
        //private bool unapplyIsDone = false;
        private bool _runApply;
        // If true, run Apply
        // If false, run Unapply

        private void Start()
        {
            _runApply = true;
        }

        public void SetSeconds(float seconds)
        {
            this._seconds = seconds;
        }

        public float GetSeconds()
        {
            return _seconds;
        }

        sealed public override void Apply()
        {
            if (_runApply)
            {
                StartApply();
            }
            else
            {
                StartUnapply();
            }
        }

        public abstract void StartUnapply();
    }

// A class that will apply itself every couple of seconds with a ApplyEverySecs
    public abstract class ApplyEverySecsFunc : FreezableObject
    {
        private float _seconds;
        private bool _applyIsDone = false;
        public void SetSeconds(float seconds)
        {
            this._seconds = seconds;
        }

        public float GetSeconds()
        {
            return _seconds;
        }

        public virtual void Apply()
        {
            StartApply();
        }

        // This is here to make the ApplyEverySecsFunc and ApplyUnapplyEverySecsFunc
        // use the same apply method name.
        public abstract void StartApply();

        public abstract void TurnOff();

        // Returns true when apply is finished and resets apply
        public bool FinishedApplying()
        {
            if (_applyIsDone)
            {
                _applyIsDone = false;
                return true;
            }
            return false;
        }

        // Sets apply to be done for this frame
        protected void SetApplyDone()
        {
            _applyIsDone = true;
        }
    }
}