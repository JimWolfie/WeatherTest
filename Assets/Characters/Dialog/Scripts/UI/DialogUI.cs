using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogEngine;
using TMPro;
using UnityEngine.UI;

namespace ui
{
    public class DialogUI: MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField]TextMeshProUGUI AI_Text;
        [SerializeField]Button nextButton;
        [SerializeField]GameObject AI_Response;
        [SerializeField]Transform choiceRoot;
        [SerializeField]GameObject choicePrefab;
        [SerializeField]Button quitButton;

        private void Start()
        {
            playerConversant= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(()=>playerConversant.Next());

            quitButton.onClick.AddListener(() => playerConversant.Quit());
            UpdateUI();
            
        }

      

        void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            if(playerConversant.IsActive())
            {
                return;
            }
            AI_Text.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
            AI_Response.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            if(playerConversant.IsChoosing())
            {
                BuildChoiceList();
            } else
            {
                AI_Text.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach(Transform choice in choiceRoot)
            {
                Destroy(choice.gameObject);
            }

            foreach(DialogNode choice in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComp.text = choice.GetText();
                Button button = GetComponentInChildren<Button>();
                button.onClick.AddListener(()=>
                {
                    playerConversant.SelectChoice(choice);
                    
                }
                );
            }
        }
    }

}

