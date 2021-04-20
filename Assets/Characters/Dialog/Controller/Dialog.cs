using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DialogEngine
{
    [CreateAssetMenu(fileName = "New Dialog", menuName = "Dialogs")]
    public class Dialog: ScriptableObject
    {
        [SerializeField]
        public List<DialogNode> nodes = new List<DialogNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if(nodes.Count == 0)
            {
                nodes.Add(new DialogNode());
            }
        }
#endif

        public IEnumerable<DialogNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogNode GetRootNode()
        {
            return nodes[0];
            
        }

       
    }

    [Serializable]
    public class DialogNode
    {
        public string uniqueID;
        public string text;
        public string[] cildren;
        public Rect rect = new Rect(0,0, 200,100);
    }

}


