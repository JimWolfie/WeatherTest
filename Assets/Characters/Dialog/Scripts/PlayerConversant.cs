using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogEngine
{



    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialog testDialog;
    
        Dialog currentDialog;
        DialogNode currentNode= null;
        bool isChoosing = false;

        public event Action onConversationUpdated;
        
        IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            StartDialog(testDialog);
            onConversationUpdated();
        }

        public void Quit()
        {
            currentDialog = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return(currentDialog != null);
            
        }
        public void StartDialog(Dialog newDialog)
        {
            currentDialog = newDialog;
            currentNode = currentDialog.GetRootNode();
            onConversationUpdated();
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

        public IEnumerable<DialogNode> GetChoices()
        {
            return currentDialog.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;
            Next();

        }

        public void Next()
        {
            int numPlayerResponses = currentDialog.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                onConversationUpdated();
                return;
            }

            DialogNode[] children = currentDialog.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            currentNode= children[randomIndex];
            onConversationUpdated();
        }
        public bool HasNext()
        {
            return currentDialog.GetAllChildren(currentNode).Count()>0;
        }

    }
}
