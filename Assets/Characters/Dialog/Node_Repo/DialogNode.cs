using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogEngine
{
    
    public class DialogNode :ScriptableObject
    {
        [SerializeField] private bool isPlayerSpeaking = false;
        [SerializeField] private string text;
        [SerializeField] private List<string> cildren = new List<string>();
        [SerializeField] private Rect rect = new Rect(0, 0, 200, 100);
        [SerializeField] string onEntryAction;
        [SerializeField] string onExitAction;

        public Rect GetRect()
        {
            return rect;
        }
        public string GetText()
        {
            return text;
        }
        public List<string> GetChildren()
        {
            return cildren;
        }
        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }
        public string OnEnterAction()
        {
            return onEntryAction;
        }
        public string OnExitAction()
        {
            return onExitAction;
        }

#if UNITY_EDITOR

        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialog Node");
            rect.position = newPosition;

            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if(newText != text)
            {
                Undo.RecordObject(this, "Update Dialog Text");
                text= newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialog Link");
            cildren.Add(childID);
            EditorUtility.SetDirty(this);
        }
        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "remove Dialog Link");
            cildren.Remove(childID);
            EditorUtility.SetDirty(this);
        }
        public void SetPlayerSpeaking(bool isSpeaking)
        {
            Undo.RecordObject(this, "Set Speaker");
            isPlayerSpeaking = isSpeaking;
            EditorUtility.SetDirty(this);
        }
       

#endif

    }
}
   

