using TSGameDev.SavingSystem;
using UnityEngine;
using CodeMonkey.Utils;
using TSGameDev.Controls.MainPlayer;
using System;

namespace TSGameDev.Inventories.Pickups
{
    public class PickupMultiSpawner : MonoBehaviour, ISaveable, IInteractable
    {
        #region Serialized Variables

        [SerializeField] PlayerConnector playerConnector;
        [SerializeField] InventoryItem item;
        [SerializeField] string spawnerID = "Unique Spawner ID";
        [SerializeField] int minItemAmount = 1;
        [SerializeField] int maxItemAmount = 2;
        [SerializeField] float respawnTime = 1f;

        #endregion

        #region Private Variables

        //The defined and cache amount of items that will be given to the player upon interaction.
        int selectedItemAmount = 0;

        #endregion

        #region Life Cycle Functions

        private void Awake()
        {
            // Spawn in Awake so can be destroyed by save system after.
            SpawnPickup();
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Returns the pickup spawned by this class if it exists.
        /// </summary>
        /// <returns>Returns null if the pickup has been collected.</returns>
        public Pickup GetPickup()
        {
            return GetComponentInChildren<Pickup>();
        }

        /// <summary>
        /// True if the pickup was collected.
        /// </summary>
        public bool isCollected()
        {
            return GetPickup() == null;
        }

        /// <summary>
        /// Function that randomises the amount of items that will be given to the player upon pickup
        /// </summary>
        /// <returns>
        /// A random number, int, of the amount of items that will be given to the player upon being picked up.
        /// </returns>
        public int SelectItemAmount()
        {
            if (selectedItemAmount == 0)
                selectedItemAmount = UnityEngine.Random.Range(minItemAmount, maxItemAmount);

            return selectedItemAmount;
        }

        /// <summary>
        /// Sets up the spawners timer, when the timer has been completed the spawn pickup function is run respawning the item this spawner represents.
        /// </summary>
        public void BeginSpawnerTimer()
        {
            FunctionTimer.Create(SpawnPickup, respawnTime, spawnerID);
        }

        /// <summary>
        /// Function from IInteractable that impliments the functionlaity of the player interacting with the object.
        /// </summary>
        public void OnInteract()
        {
            if (GetPickup())
            {
                Pickup pickup = GetPickup();
                if (pickup.CanBePickedUp())
                {
                    pickup.PickupItem();
                }
            }
        }

        //Not required for this script
        public bool IsToggleable()
        {
            return false;
        }

        //Not required for this script
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Function that collects the pickup prefab from the item variable and spawns it as a child to this spawner and this spawners location.
        /// </summary>
        private void SpawnPickup()
        {
            var spawnedPickup = item.SpawnPickup(transform.position, SelectItemAmount());
            spawnedPickup.transform.SetParent(transform);
            selectedItemAmount = 0;
        }

        /// <summary>
        /// Destorys the child spawned pickup
        /// </summary>
        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }

        /// <summary>
        /// A simple struct to combine the 2 different data types that are required for something the spawner state since only a single object can be returned into the save system.
        /// </summary>
        [Serializable]
        private struct MultiSpawnerData
        {
            public float savedSpawnTime;
            public bool hasBeenCollected;
        }

        /// <summary>
        /// Function implimented by ISavable allows for this script to define saving functionlaity
        /// </summary>
        /// <returns>
        /// An object which is the spawned pickup gameobject if it is there.
        /// </returns>
        object ISaveable.CaptureState()
        {
            float currentSpawnTime = FunctionTimer.GetTime(spawnerID);
            bool hasBeenCollected = isCollected();
            MultiSpawnerData data = new()
            {
                savedSpawnTime = currentSpawnTime,
                hasBeenCollected = hasBeenCollected
            };

            return data;
        }

        /// <summary>
        /// Function implimented by ISavable allows for this script to define the functionlaity of loading save data.
        /// </summary>
        /// <param name="state">
        /// A bool for if this pickup was collected or not. If so then it destorys the awake spawned pickup.
        /// </param>
        void ISaveable.RestoreState(object state)
        {
            MultiSpawnerData data = (MultiSpawnerData)state;

            if(data.hasBeenCollected)
            {
                DestroyPickup();
                FunctionTimer.Create(SpawnPickup, data.savedSpawnTime, spawnerID);
            }

        }

        #endregion

    }
}
