using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogEngine;
using TMPro;

namespace ui
{
    public class DialogUI: MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField]TextMeshProUGUI AI_Text;

        private void Start()
        {
            playerConversant= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            AI_Text.text = playerConversant.GetText();
        }
    }
}

