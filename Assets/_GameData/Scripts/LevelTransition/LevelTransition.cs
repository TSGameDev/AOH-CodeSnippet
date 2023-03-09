using TSGameDev.Controls.MainPlayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSGameDev.LevelManagment
{
    public class LevelTransition : MonoBehaviour, IInteractable
    {
        [SerializeField] private int levelIndex;
        [SerializeField] private string triggerNotificationMessage;
        [SerializeField] private LoadingUI loadingUI;
        [SerializeField] private MessageSystem messageManager;

        public void Cancel()
        {
            throw new System.NotImplementedException();
        }

        public bool IsToggleable()
        {
            return false;
        }

        public void OnInteract()
        {
            loadingUI.LoadLevel(levelIndex);
        }

        public void ExitApplication()
        {
            Application.Quit();
        }

        public void OnTriggerEnter(Collider other)
        {
            messageManager.NoticationMessageActive(triggerNotificationMessage);
        }

        public void OnTriggerExit(Collider other)
        {
            messageManager.NotificationMessageDeactive();
        }
    }
}
