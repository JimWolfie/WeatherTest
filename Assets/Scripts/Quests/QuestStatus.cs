
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        List<string> completedObjective = new List<string>();
        private object objectState;

        [System.Serializable]
        class QuestStatusRecord
        {
            public string name;
            public List<string> CompletedObjectives;
        }

        public QuestStatus(Quest quest)
        {
            this.quest=quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = Quest.GetByName(state.name);
            completedObjective = state.CompletedObjectives;
        }

        public bool IsComplete()
        {
            foreach(var objectives in quest.GetObjectives())
            {
                if(!completedObjective.Contains(objectives.reference))
                {
                    return false;
                }
            }
            return true;
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public int GetCompletedCount()
        {
            return completedObjective.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return completedObjective.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if(quest.HasObjective(objective))
            {
                completedObjective.Add(objective);
            }
            
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.name = quest.name;
            state.CompletedObjectives = completedObjective;
            return state;
        }
    }
}