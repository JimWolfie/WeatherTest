using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLIstUI : MonoBehaviour
{
    
    [SerializeField]QuestItemUI questPrefab;
    QuestList questList;
    // Start is called before the first frame update
    void Start()
    {
        questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        questList.onUpdate += Redraw;
        Redraw();

    }

    private void Redraw()
    {
        //transform.DetachChildren();
        foreach(Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        foreach(QuestStatus status in questList.GetStatus())
        {
            QuestItemUI UiInstance = Instantiate<QuestItemUI>(questPrefab, transform);
            UiInstance.Setup(status);
        }
    }
}
