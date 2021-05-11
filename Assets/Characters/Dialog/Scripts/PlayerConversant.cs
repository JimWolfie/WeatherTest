using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPG.Core;

namespace DialogEngine
{



    public class PlayerConversant : MonoBehaviour
    {
        //[SerializeField] Dialog testDialog;
    
        Dialog currentDialog;
        DialogNode currentNode= null;
        Ai_Conversant ai_Conversant = null;
        bool isChoosing = false;

        [SerializeField]string playerName;

        public event Action onConversationUpdated;

        public void StartDialog(Dialog newDialog, Ai_Conversant newConversant)
        {
           
            currentDialog = newDialog;
            TriggerEnterActions();
            ai_Conversant = newConversant;
            currentNode = currentDialog.GetRootNode();
            
            onConversationUpdated();
        }

        public void Quit()
        {
            
            currentDialog = null;
            TriggerExitActions();
            currentNode = null;
            
            isChoosing = false;
            ai_Conversant = null;
            
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return(currentDialog != null);
            
        }
       

        public bool IsChoosing()
        {
            return isChoosing;
        }
        public string GetText()
        {
            if(currentNode == null)
            {
                return "";
            }

            return currentNode.GetText();


        }

        public string GetCurrentConversantName()
        {
            if(IsChoosing())
            {
                return playerName;
            }else 
            {
                return ai_Conversant.GetName();
            }

        }

        public IEnumerable<DialogNode> GetChoices()
        {
            return FilterOnCondition(currentDialog.GetPlayerChildren(currentNode));
        }

        public void SelectChoice(DialogNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterActions();
            isChoosing = false;
            Next();

        }

        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(currentDialog.GetPlayerChildren(currentNode)).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitActions();
                onConversationUpdated();
                return;
            }

            DialogNode[] children = FilterOnCondition(currentDialog.GetAIChildren(currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            TriggerExitActions();
            currentNode= children[randomIndex];
            TriggerEnterActions();
            onConversationUpdated();
        }
        public bool HasNext()
        {
            return FilterOnCondition(currentDialog.GetAllChildren(currentNode)).Count()>0;
        }

        private IEnumerable<DialogNode> FilterOnCondition(IEnumerable<DialogNode> inputNode)
        {
            foreach(var node in inputNode)
            {
                if(node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }

        }
        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        private void TriggerEnterActions()
        {
            if(currentNode!=null)
            {
                TriggerAction(currentNode.OnEnterAction());
            }
        }
        private void TriggerExitActions()
        {
            if(currentNode!=null)
            {
                TriggerAction(currentNode.OnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if(currentNode.OnEnterAction()==""){return;}
            foreach(Dialog_Trigger trigger in ai_Conversant.GetComponents<Dialog_Trigger>())
            {
                Debug.Log(action);
                trigger.Trigger(action);
            }

        }
       
    }
}
