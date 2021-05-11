using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;
using System;
using System.Text;

namespace RPG.UI.Quests
{
    public class QuestToolTipUI: MonoBehaviour
    {
        [SerializeField]TextMeshProUGUI title;
        [SerializeField]Transform ObjectiveContainer;
        [SerializeField]GameObject ObjectivePrefab;
        [SerializeField] GameObject ObjectiveIncompletePrefab;
        [SerializeField] TextMeshProUGUI RewardText;

        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            title.text = quest.GetTitle();
            //ObjectiveContainer.DetachChildren();
            foreach (Transform item in ObjectiveContainer)
            {
                Destroy(item.gameObject);
            }
            foreach(var objective in quest.GetObjectives())
            {
                GameObject prefab = ObjectiveIncompletePrefab;
                if(status.IsObjectiveComplete(objective.reference))
                {
                    prefab = ObjectivePrefab;
                }
                GameObject objectiveInstance = Instantiate(prefab, ObjectiveContainer);
                var objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;
            }
            RewardText.text = GetRewardText(quest);

        }

        private string GetRewardText(Quest quest)
        {
            string str ="";
            foreach (var rewards in quest.GetRewards())
            {
                if(str != "")
                {
                    str +=", =";
                }
                if(rewards.number > 1)
                {
                    str += rewards.number +" ";
                }
                str += rewards.item.GetDisplayName();
            }
            if(str == "")
            {
                str = "No reward =(";
            } else
            {
                str += ". ";
            }
            return str;
            
        }
    }
}
