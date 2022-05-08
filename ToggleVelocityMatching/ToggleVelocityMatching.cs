using OWML.Common;
using OWML.ModHelper;
using UnityEngine.InputSystem;
using System.Collections.Generic;
namespace ModTemplate
{
    public class ToggleVelocityMatching : ModBehaviour
    {
        private bool matchingVelocity;
        private bool breakKey;
        private List<INotifiable> notifiables = new List<INotifiable>();
        private static ToggleVelocityMatching instance { get; set; }
        private void Start()
        {
            instance = this;
            ModHelper.HarmonyHelper.AddPrefix<Autopilot>("StopMatchVelocity", typeof(ToggleVelocityMatching), "StopMatchVelocityPatch");
            notifiables = NotificationManager.SharedInstance._notifiableElements;
        }
        private void Update()
        {
            if (OWInput.GetInputMode() != InputMode.ShipCockpit) { return; }
            breakKey = OWInput.IsNewlyPressed(InputLibrary.matchVelocity) ||
                       OWInput.IsNewlyPressed(InputLibrary.thrustUp) ||
                       OWInput.IsNewlyPressed(InputLibrary.thrustDown) ||
                       OWInput.IsNewlyPressed(InputLibrary.thrustX) ||
                       OWInput.IsNewlyPressed(InputLibrary.thrustZ);

            if (OWInput.IsNewlyPressed(InputLibrary.matchVelocity) && !matchingVelocity)
            {
                ModHelper.Console.WriteLine("Matching velocity trigger");
                matchingVelocity = true;
                NotificationManager.SharedInstance._notifiableElements = null;
            }
            else if (breakKey)
            {
                ModHelper.Console.WriteLine("Matching velocity trigger false");
                matchingVelocity = false;
                NotificationManager.SharedInstance._notifiableElements = notifiables;
            }
        }
        public static bool StopMatchVelocityPatch()
        {
            if (instance.matchingVelocity == true)
            {
                instance.ModHelper.Console.WriteLine("Matching velocity constantly");
                return false;
            }
            instance.ModHelper.Console.WriteLine("No longer matching velocity");
            return true;
        }
    }
}
