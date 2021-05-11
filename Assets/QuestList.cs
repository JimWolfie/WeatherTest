using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using GameDevTV.Inventories;
using RPG.Core;

namespace RPG.Quests
{
    public class QuestList: MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        List<QuestStatus> questStatuses = new List<QuestStatus>();

        public event Action onUpdate;

        public void AddQuest(Quest quest)
        {
            if(HasQuest(quest)) return;
            QuestStatus newQuest = new QuestStatus(quest);
            questStatuses.Add(newQuest);
            if(onUpdate!=null)
            {
                onUpdate();
            }
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            var status = GetQuestStatus(quest);
            status.CompleteObjective(objective);
            if(status.IsComplete())
            {
                GiveReward(quest);
            }
            if(onUpdate!=null)
            {
                onUpdate();
            }
        }

        

        public bool HasQuest(Quest quest)
        {
          
            return GetQuestStatus(quest)!=null;
        }

        public IEnumerable<QuestStatus> GetStatus()
        {
            return questStatuses;
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach(QuestStatus status in questStatuses)
            {
                if(status.GetQuest()==quest)
                { return status; }

            }
            return null;
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach(QuestStatus status in questStatuses)
            {
                state.Add(status.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if(stateList == null) return;
            questStatuses.Clear();
            foreach(object objectState in stateList)
            {
                questStatuses.Add(new QuestStatus(objectState));
                
            }
        }

        private void GiveReward(Quest quest)
        {
            foreach(var reward in quest.GetRewards())
            {
                var success= GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                if(!success)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        public bool? Evaluate(string predicate, string[] param)
        {
            //if(predicate != "HasQuest")return null;
            switch(predicate)
            {
                case "HasQuest":
                    return HasQuest(Quest.GetByName(param[0]));

                case "Completed":
                    return GetQuestStatus(Quest.GetByName(param[0])).IsComplete();
            }





            return null;
        }
    }
}
