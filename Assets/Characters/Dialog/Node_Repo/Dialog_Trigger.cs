using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogEngine
{
    public class Dialog_Trigger: MonoBehaviour
    {
        [SerializeField]string action;
        [SerializeField]UnityEvent onTrigger;


        public void Trigger (string actionToTrigger)
        {
            if(actionToTrigger == action)
            {
                onTrigger.Invoke();
            }
        }


    }
}

