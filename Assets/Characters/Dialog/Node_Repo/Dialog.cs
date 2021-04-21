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

        Dictionary<string, DialogNode> nodeLookup = new Dictionary<string, DialogNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if(nodes.Count == 0)
            {
                nodes.Add(new DialogNode());
            }
            OnValidate();
        }
#endif
        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach(DialogNode node in GetAllNodes())
            {
                nodeLookup[node.uniqueID] = node;
            }

        }
           
       
        public IEnumerable<DialogNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogNode GetRootNode()
        {
            return nodes[0];
            
        }

        public IEnumerable<DialogNode> GetAllChildren(DialogNode parentNode)
        {
            
            foreach(string childID in parentNode.cildren)
            {
                if(nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
                
            }
            
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


