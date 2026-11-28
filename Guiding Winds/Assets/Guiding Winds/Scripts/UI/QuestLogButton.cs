using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Assets.Guiding_Winds.Scripts.Systems.Quests;

public class QuestLogButton : MonoBehaviour, ISelectHandler
{
    public Button Button { get; private set; }
    private TextMeshProUGUI ButtonText;
    private UnityAction onSelectAction;

    // because we're instantiating the button and it may be disabled when we
    // instantiate it, we need to manually initialize anything here.
    public void Initialize(string displayName, UnityAction selectAction) 
    {
        this.Button = this.GetComponent<Button>();
        this.ButtonText = this.GetComponentInChildren<TextMeshProUGUI>();

        this.ButtonText.text = displayName;
        this.onSelectAction = selectAction;
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction();
    }

    public void SetState(QuestState state)
    {
        switch (state)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
            case QuestState.CAN_START:
                ButtonText.color = Color.red;
                break;
            case QuestState.IN_PROGRESS:
            case QuestState.CAN_FINISH:
                ButtonText.color = Color.yellow;
                break;
            case QuestState.FINISHED:
                ButtonText.color = Color.green;
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement: " + state);
                break;
        }
    }
}