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
        [NonSerialized]
        GUIStyle nodeStyle;
        [NonSerialized]
        GUIStyle playerNodeStyle;
        [NonSerialized]
        DialogNode draggingNode = null;
        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        DialogNode creatingNode = null;
        [NonSerialized]
        DialogNode deletingNode = null;
        [NonSerialized]
        DialogNode linkingParentNode = null;

        Vector2 scrollPosition;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;

        const float canvasSize = 4000;
        const float backgroundSize = 50;

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

            Selection.selectionChanged +=OnSelectionChanged;
            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.normal.textColor = Color.white;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
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
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(canvasSize,canvasSize);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCords = new Rect(0,0, canvasSize/backgroundSize, canvasSize/backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCords);
                

                foreach(var node in selectedDialog.GetAllNodes())
                {
                    DrawNode(node);
                }
                foreach(var node in selectedDialog.GetAllNodes())
                {
                    DrawConnections(node);
                }
                EditorGUILayout.EndScrollView();
                if(creatingNode != null)
                {
                   
                    selectedDialog.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if(deletingNode != null)
                {
                    
                    selectedDialog.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }


        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode =GetNodeAtPoint(Event.current.mousePosition+scrollPosition);
                if(draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialog;
                }
            }
            else if(Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                
                draggingNode.SetPosition(Event.current.mousePosition +draggingOffset);
                GUI.changed = true;
            } 
            else if(Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if(Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }


        private void DrawNode(DialogNode node)
        {
            GUIStyle style = nodeStyle;
            if(node.IsPlayerSpeaking())
            {
                style = playerNodeStyle;
            }
            GUILayout.BeginArea(node.GetRect(), style);
            


           node.SetText(EditorGUILayout.TextField(node.GetText())); 



             
            
            
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("-"))
            {
                deletingNode = node;
            }
            DrawLinkButtons(node);

            if(GUILayout.Button("+"))
            {
                creatingNode = node;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogNode node)
        {
            if(linkingParentNode == null)
            {
                if(GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            } else if(linkingParentNode == node)
            {
                if(GUILayout.Button("cancel"))
                {
                    linkingParentNode = null;
                }
            }else if(linkingParentNode.GetChildren().Contains(node.name))
            {
                if(GUILayout.Button("unlink"))
                {
                    
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }

            }
            else
            {
                if(GUILayout.Button("child"))
                {
                    
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach(DialogNode childNode in selectedDialog.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y =0;
                controlPointOffset.x *= .8f;
                Handles.DrawBezier(
                    startPosition, endPosition, 
                    startPosition+controlPointOffset, endPosition+controlPointOffset, 
                    Color.white, null, 4f);
                
            }
        }


        private DialogNode GetNodeAtPoint(Vector2 mousePosition)
        {
            DialogNode foundNode = null;
            foreach (DialogNode node in selectedDialog.GetAllNodes())
            {
                if(node.GetRect().Contains(mousePosition))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }

}

