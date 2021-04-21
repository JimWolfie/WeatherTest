using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "IEnumerableTest")]
public class Quest : ScriptableObject
{
    [SerializeField]string[] tasks;

    public IEnumerable<string> GetTasks()
    {
        return tasks;
    }
   
}
