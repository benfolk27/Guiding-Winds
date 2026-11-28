using UnityEngine;
using UnityEngine.Events;
using Assets.Guiding_Winds.Scripts.Systems.Quests;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject ContentParent;

    [Header("Rect Transforms")]
    [SerializeField] private RectTransform ScrollRectTransform;
    [SerializeField] private RectTransform ContentRectTransform;
    
    [Header("Quest Log Button")]
    [SerializeField] private GameObject QuestLogButtonPrefab;

    private readonly Dictionary<string, QuestLogButton> IdToButtonMap = new();

    public QuestLogButton CreateButtonIfNotExists(Quest quest, UnityAction selectAction) 
    {
        QuestLogButton questLogButton = null;
        // only create the button if we haven't seen this quest id before
        if (!IdToButtonMap.ContainsKey(quest.info.Id))
        {
            questLogButton = InstantiateQuestLogButton(quest, selectAction);
        }
        else 
        {
            questLogButton = IdToButtonMap[quest.info.Id];
        }
        return questLogButton;
    }

    private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
    {
        // create the button
        QuestLogButton questLogButton = Instantiate(
            QuestLogButtonPrefab,
            ContentParent.transform).GetComponent<QuestLogButton>();
        // game object name in the scene
        questLogButton.gameObject.name = quest.info.Id + "_button";
        // initialize and set up function for when the button is selected
        RectTransform buttonRectTransform = questLogButton.GetComponent<RectTransform>();
        questLogButton.Initialize(quest.info.name, () => {
            selectAction();
            UpdateScrolling(buttonRectTransform);
        });
        // add to map to keep track of the new button
        IdToButtonMap[quest.info.Id] = questLogButton;
        return questLogButton;
    }

    private void UpdateScrolling(RectTransform buttonRectTransform)
    {
        // calculate the min and max for the selected button
        float buttonYMin = Mathf.Abs(buttonRectTransform.anchoredPosition.y);
        float buttonYMax = buttonYMin + buttonRectTransform.rect.height;

        // calculate the min and max for the content area
        float contentYMin = ContentRectTransform.anchoredPosition.y;
        float contentYMax = contentYMin + ScrollRectTransform.rect.height;

        // handle scrolling down
        if (buttonYMax > contentYMax)
        {
            ContentRectTransform.anchoredPosition = new Vector2(
                ContentRectTransform.anchoredPosition.x,
                buttonYMax - ScrollRectTransform.rect.height
            );
        }
        // handle scrolling up
        else if (buttonYMin < contentYMin) 
        {
            ContentRectTransform.anchoredPosition = new Vector2(
                ContentRectTransform.anchoredPosition.x,
                buttonYMin
            );
        }
    }
}
