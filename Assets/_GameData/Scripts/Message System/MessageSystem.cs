using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TSGameDev.UI.Tween;

public class MessageSystem : MonoBehaviour
{
    [SerializeField] RectTransform parentCanvas;

    #region Level Up Variables

    [SerializeField] GameObject levelUpMessage;
    TweenProfile levelUpTweenProfile;
    TextMeshProUGUI levelUpTxt;

    #endregion

    #region Notication Message Variables

    [SerializeField] GameObject notificationMessage;
    TweenProfile notificationTweenProfile;
    TextMeshProUGUI notificationTxt;

    #endregion

    #region Confirmation Message Variables

    [SerializeField] GameObject confirmationMessage;
    TweenProfile confirmationTweenProfile;
    TextMeshProUGUI confirmationTxt;
    Button confirmationButton;
    Button nonConfirmationButton;

    #endregion

    #region Enter Location Variables

    [SerializeField] GameObject locationMessage;
    TweenProfile locationTweenProfile;
    TextMeshProUGUI locationTxt;

    #endregion

    #region Public Functions

    /// <summary>
    /// Function that is resonable for posting a level up message for any skills.
    /// </summary>
    /// <param name="skill">
    /// The skill, string, that has leveled up.
    /// </param>
    /// <param name="level">
    /// The new Level, int, the passed in string is now at.
    /// </param>
    public void LevelUpMessage(string skill, int level)
    {
        LevelMessageSetup();

        levelUpTxt.text = $"{skill} has advanced to {level}";

        levelUpTweenProfile.ActivateTween();
    }

    /// <summary>
    /// Function that is responsable for posting a notification message.
    /// </summary>
    /// <param name="notification">
    /// The notification message, string, to be displayed.
    /// </param>
    public void NoticationMessageActive(string notification)
    {
        NotificationMessageSetup();

        notificationTxt.text = notification;

        notificationTweenProfile.ActivateTween();
    }

    public void NotificationMessageDeactive()
    {
        notificationTxt.text = "";
        notificationTweenProfile.DeactivateTween();
    }

    /// <summary>
    /// Function that is responsable for posting a confirmation message. This is a large UI format, best suited for more important or longer text actions.
    /// </summary>
    /// <param name="confirmationstring">
    /// The action, string, you want to confirm the player wants to do (e.g. delete an item).
    /// </param>
    /// <param name="confirmedFunctionlity">
    /// The functionality to be performed if the player wants to continue with the action.
    /// </param>
    public void ConfirmationMessage(string confirmationstring, UnityAction confirmedFunctionlity)
    {
        ConfirmationMessageSetup();

        confirmationTxt.text = confirmationstring;

        confirmationButton.onClick.AddListener(confirmedFunctionlity);
        confirmationButton.onClick.AddListener(() => ConfirmationDeletion(confirmationButton, confirmationTweenProfile));
        confirmationButton.onClick.AddListener(() => ConfirmationDeletion(nonConfirmationButton, confirmationTweenProfile));

        nonConfirmationButton.onClick.AddListener(() => ConfirmationDeletion(confirmationButton, confirmationTweenProfile));
        nonConfirmationButton.onClick.AddListener(() => ConfirmationDeletion(nonConfirmationButton, confirmationTweenProfile));

        confirmationTweenProfile.ActivateTween();
    }

    /// <summary>
    /// Function that is responsable for posting a confirmation message. This is a smaller UI format, best suited for less important or short text actions.
    /// </summary>
    /// <param name="confirmationstring">
    /// The action, string, you want to confirm the player wants to do (e.g. delete an item).
    /// </param>
    /// <param name="confirmedFunctionlity">
    /// The functionality to be performed if the player wants to continue with the action.
    /// </param>
    public void LocationMessage(string confirmationstring)
    {
        LocationMessageSetup();

        locationTxt.text = confirmationstring;

        locationTweenProfile.ActivateTween();

    }

    #endregion

    #region Private Functions

    void LevelMessageSetup()
    {
        GameObject levelMessageGO = Instantiate(levelUpMessage, parentCanvas);

        levelUpTweenProfile = levelMessageGO.GetComponent<TweenProfile>();
        levelUpTxt = levelMessageGO.GetComponentInChildren<TextMeshProUGUI>();
    }

    void NotificationMessageSetup()
    {
        notificationTweenProfile = notificationMessage.GetComponent<TweenProfile>();
        notificationTxt = notificationMessage.GetComponentInChildren<TextMeshProUGUI>();
    }

    void ConfirmationMessageSetup()
    {
        GameObject confirmationMessageGO = Instantiate(confirmationMessage, parentCanvas);

        confirmationTweenProfile = confirmationMessageGO.GetComponent<TweenProfile>();
        confirmationTxt = confirmationMessageGO.GetComponentInChildren<TextMeshProUGUI>();

        //Collects both yes and no button. The yes button is first in the hiarchy so it will be element 0, no will be element 1
        List<Button> confirmButtons = confirmationMessageGO.GetComponentsInChildren<Button>().ToList();

        confirmationButton = confirmButtons[0];
        nonConfirmationButton = confirmButtons[1];
    }

    void LocationMessageSetup()
    {
        GameObject locationMessageGO = Instantiate(locationMessage, parentCanvas);

        locationTweenProfile = locationMessageGO.GetComponent<TweenProfile>();
        locationTxt = locationMessageGO.GetComponentInChildren<TextMeshProUGUI>();
    }

    void ConfirmationDeletion(Button button, TweenProfile tween)
    {
        button.onClick.RemoveAllListeners();
        tween.DeactivateTween();
    }

    #endregion

}
