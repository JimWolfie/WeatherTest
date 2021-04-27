using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            
            currentNode = null;
            TriggerExitActions();
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
            return currentDialog.GetPlayerChildren(currentNode);
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
            int numPlayerResponses = currentDialog.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitActions();
                onConversationUpdated();
                return;
            }

            DialogNode[] children = currentDialog.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            TriggerExitActions();
            currentNode= children[randomIndex];
            TriggerEnterActions();
            onConversationUpdated();
        }
        public bool HasNext()
        {
            return currentDialog.GetAllChildren(currentNode).Count()>0;
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
            if(currentNode.OnEnterAction()!=""){return;}
            foreach(Dialog_Trigger trigger in ai_Conversant.GetComponents<Dialog_Trigger>())
            {
                Debug.Log(action); //trigger.Trigger(action);
            }

        }
       
    }
}
