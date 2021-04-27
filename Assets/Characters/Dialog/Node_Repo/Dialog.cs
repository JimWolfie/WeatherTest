using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace DialogEngine
{
    [CreateAssetMenu(fileName = "New Dialog", menuName = "Dialogs")]
    public class Dialog: ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        public List<DialogNode> nodes = new List<DialogNode>();
        [SerializeField]
        public Vector2 newNodeOffset = new Vector2(250,0);

        Dictionary<string, DialogNode> nodeLookup = new Dictionary<string, DialogNode>();


        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach(DialogNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
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
            foreach(string childID in parentNode.GetChildren())
            {
                if(nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }  
            }
        }

        public IEnumerable<DialogNode> GetPlayerChildren(DialogNode currentNode)
        {
            foreach(DialogNode node in GetAllChildren(currentNode))
            {
                if(node.IsPlayerSpeaking())
                {
                    yield return node;
                }
                
            }
        }

        public IEnumerable<DialogNode> GetAIChildren(DialogNode currentNode)
        {
            foreach(DialogNode node in GetAllChildren(currentNode))
            {
                if(!node.IsPlayerSpeaking())
                {
                    yield return node;
                }

            }
        }


#if UNITY_EDITOR
        public void CreateNode(DialogNode parent)
        {
            DialogNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialog Node");
            Undo.RecordObject(this, "Add Dialog Node");
            AddNode(newNode);
        }

        

        public void DeleteNode(DialogNode nodeToDelete)
        {
            Undo.RecordObject(this, "Remove Dialog Node");
            nodes.Remove(nodeToDelete);
            
            OnValidate();
            NodeVasectomy(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void AddNode(DialogNode newNode)
        {
            nodes.Add(newNode);
            OnValidate();
        }

        private  DialogNode MakeNode(DialogNode parent)
        {
            DialogNode newNode = CreateInstance<DialogNode>();

            newNode.name = Guid.NewGuid().ToString();


            if(parent!= null)
            {
                parent.AddChild(newNode.name);
                //AssetDatabase.AddObjectToAsset(this, this);
                newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking());
                newNode.SetPosition(parent.GetRect().position +newNodeOffset);
            }

            return newNode;
        }

        private void NodeVasectomy(DialogNode nodeToDelete)
        {
            foreach(DialogNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR

            if(nodes.Count == 0)
            {
                DialogNode newNode = MakeNode(null);
              
                AddNode(newNode);
            }
            if( AssetDatabase.GetAssetPath(this)!= "")
            {
                foreach(DialogNode node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node)=="")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }

    

}


