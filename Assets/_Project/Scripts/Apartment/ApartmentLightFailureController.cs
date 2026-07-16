using UnityEngine;

namespace Okolitsa.Apartment
{
    public class ApartmentLightFailureController : MonoBehaviour
    {
        [Header("Apartment Lights")]
        [SerializeField] private Light[] apartmentLights;

        [Header("Debug")]
        [SerializeField] private KeyCode testToggleKey = KeyCode.L;
        [SerializeField] private bool startWithPowerOn = true;

        private bool isPowerOn;

        private void Awake()
        {
            isPowerOn = startWithPowerOn;
            ApplyLightState();
        }

        private void Update()
        {
            if (Input.GetKeyDown(testToggleKey))
            {
                TogglePower();
            }
        }

        public void TurnPowerOff()
        {
            isPowerOn = false;
            ApplyLightState();
        }

        public void TurnPowerOn()
        {
            isPowerOn = true;
            ApplyLightState();
        }

        public void TogglePower()
        {
            isPowerOn = !isPowerOn;
            ApplyLightState();
        }

        private void ApplyLightState()
        {
            if (apartmentLights == null)
            {
                return;
            }

            foreach (Light apartmentLight in apartmentLights)
            {
                if (apartmentLight == null)
                {
                    continue;
                }

                apartmentLight.enabled = isPowerOn;
            }
        }
    }
}