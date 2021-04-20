using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using System;

namespace DialogEngine.Editor
{
    public class DialogEditor: EditorWindow
    {
        Dialog selectedDialog = null;
        GUIStyle nodeStyle;
        
        DialogNode draggingNode = null;
        Vector2 draggingOffset;

        [MenuItem("Window/Dialog Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogEditor), false, "Dialog Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialog dialog = EditorUtility.InstanceIDToObject(instanceID) as Dialog;
            if(dialog != null)
            {
                ShowEditorWindow();
                return true;
                
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged +=OnSelectionChanged;
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.padding = new RectOffset(20,20,20,20);
            nodeStyle.border = new RectOffset(12,12,12,12);
        }

        private void OnSelectionChanged()
        {
            Dialog newDialog = Selection.activeObject as Dialog;
            if(newDialog != null)
            {
                selectedDialog = newDialog;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if(selectedDialog == null)
            {
                EditorGUILayout.LabelField("No dialog selected");
            } else
            {
                ProcessEvents();
                foreach(var node in selectedDialog.GetAllNodes())
                {
                    OnGUINode(node);

                }
            }
            

        }

        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode =GetNodeAtPoint(Event.current.mousePosition);
                if(draggingNode != null)
                {
                    draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
                }
            }
            else if(Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialog, "Move Dialog Node");
                draggingNode.rect.position = Event.current.mousePosition +draggingOffset;
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;


            }
        }

        private void OnGUINode(DialogNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node: ", EditorStyles.whiteLabel);
            string newText = EditorGUILayout.TextField(node.text);
            string newUniqueID = EditorGUILayout.TagField(node.uniqueID);


            if(EditorGUI.EndChangeCheck())
            {

                Undo.RecordObject(selectedDialog, "Update Dialog Text");
                node.text = newText;
            }

            GUILayout.EndArea();
        }

         private DialogNode GetNodeAtPoint(Vector2 mousePosition)
        {
            DialogNode foundNode = null;
            foreach (DialogNode node in selectedDialog.GetAllNodes())
            {
                if(node.rect.Contains(mousePosition))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }

}

