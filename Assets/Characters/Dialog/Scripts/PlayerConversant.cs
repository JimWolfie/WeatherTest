using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogEngine
{

    

    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialog currentDialog;

        public string GetText()
        {
            if(currentDialog == null)
            {
                return "";
            }

            return currentDialog.GetRootNode().GetText();
        }

    }
}
