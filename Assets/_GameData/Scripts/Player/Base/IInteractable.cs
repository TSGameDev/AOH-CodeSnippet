using UnityEngine;

namespace TSGameDev.Controls.MainPlayer
{
    public interface IInteractable
    {
        /// <summary>
        /// Getter for if the IInteractable is a toggleable effect. aka a UI that toggles on and off.
        /// </summary>
        /// <returns></returns>
        public bool IsToggleable();

        /// <summary>
        /// Function that contains the functionality when the player interacts with this object.
        /// </summary>
        public void OnInteract();

        /// <summary>
        /// Function that specifically turns off the IInteractable for when the player moves to far.
        /// </summary>
        public void Cancel();
    }
}
