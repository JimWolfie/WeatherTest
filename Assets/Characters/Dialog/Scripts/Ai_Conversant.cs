using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogEngine
{
    public class Ai_Conversant: MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialog dialog = null;
        [SerializeField] string conversantName;

    public CursorType GetCursorType()
        {
            return CursorType.Dialog;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(dialog== null)
            {
                return false;
            }
            if(Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConversant>().StartDialog(dialog, this);
            }
            return true;
        }
        public string GetName()
        {
            return conversantName;
        }

    }
}

