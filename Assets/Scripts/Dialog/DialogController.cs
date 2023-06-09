using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;

namespace Dialog
{

/// <summary>
/// Parser für Dialoge, die mit dem Online-Editor inklewriter erstellt wurden:
/// https://www.inklewriter.com/
/// Um Items zu Droppen, in dem Optionstext am Ende ergänzen {ITEM_NAME}
/// </summary>
public class DialogController : MonoBehaviour
{
    Dictionary<string, DialogPart> dialog;

    string startKey = "";
    string activeKey;

    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private Image speakerImage;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button[] optionButtons;

    public void Start()
    {
        GameEvents.Instance.OnDialogueStart += StartNewDialog;
    }


    public void StartNewDialog(string dialogJSON, Sprite speakerImage)
    {
        Debug.Log("Start Dialog");
        ParseDialog(dialogJSON);
        activeKey = startKey;
        UpdateUI(activeKey);
        this.speakerImage.sprite = speakerImage;
        dialogPanel.SetActive(true);
    }

    private void ParseDialog(string dialogJSON)
    {
        dialog = new Dictionary<string, DialogPart>();

        JObject parent = JObject.Parse(dialogJSON);
                
        foreach(JProperty p in parent["data"]["stitches"])
        {
            string key = p.Name;

            DialogPart value = new DialogPart();

            JArray content = (JArray) parent["data"]["stitches"][key]["content"];

            value.message = content.First.ToString();

            ParseMessageForTag(value);

            value.options = new List<DialogOption>();

            foreach (JObject c in content.Children<JObject>())
            {
                if (c.ContainsKey("option") && c["option"].ToString().Length > 0)
                {
                    DialogOption option = new DialogOption();

                    option.optionText = c["option"].ToString();

                    if (c.ContainsKey("linkPath"))
                    {
                        option.nextDialogPartKey = c["linkPath"].ToString();
                    }

                    ParseOptionForTag(option);

                    value.options.Add(option);
                }
                if (c.ContainsKey("pageNum") && c["pageNum"].ToString() == "1")
                {
                    startKey = key;
                }
                
            }

            dialog.Add(key, value);
        }
    }

    private void ParseOptionForTag(DialogOption option)
    {
        if (option.optionText.Contains("{"))
        {
            int index = option.optionText.IndexOf("{");

            option.tag = option.optionText.Substring(index + 1).Replace("}", "").Trim();

            option.optionText = option.optionText.Substring(0, index).Trim();
        }     
    }

    private void ParseMessageForTag(DialogPart dialogPart)
    {
        if (dialogPart.message.Contains("{"))
        {
            int index = dialogPart.message.IndexOf("{");

            dialogPart.tag = dialogPart.message.Substring(index + 1).Replace("}", "").Trim();

            dialogPart.message = dialogPart.message.Substring(0, index).Trim();
        }
    }


    private void UpdateUI(string key)
    { 
        DialogPart dialogPart = dialog[key];
        messageText.text = dialogPart.message;

        if(dialogPart.tag != null)
        {
            GameEvents.Instance.OnDialogueTag?.Invoke(dialogPart.tag);
        }
        
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < dialogPart.options.Count)
            {
                DialogOption option = dialogPart.options[i];
                Button button = optionButtons[i].GetComponent<Button>();
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = option.optionText;
                button.onClick.RemoveAllListeners();
                if (option.nextDialogPartKey == null || !dialog.ContainsKey(option.nextDialogPartKey))
                {
                    button.onClick.AddListener(() => CloseDialog());
                }
                else
                {
                    button.onClick.AddListener(() => OnButtonClicked(option.nextDialogPartKey));
                }

                if (option.tag != null)
                {
                    button.onClick.AddListener(() => CreateDialogTagAction(option.tag));
                }

                optionButtons[i].gameObject.SetActive(true);
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
        if (dialogPart.options.Count == 0) // no options => add close button
        {
            Debug.Log("Keine Optionen");
            Button button = optionButtons[0].GetComponent<Button>();
            optionButtons[0].GetComponentInChildren<TMP_Text>().text = "Gespräch beenden";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => CloseDialog());
            optionButtons[0].gameObject.SetActive(true);
        }
    }

    public void OnButtonClicked(string nextDialogPartKey)
    {
        activeKey = nextDialogPartKey;
        UpdateUI(activeKey);
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
        GameEvents.Instance.OnDialogueClosed?.Invoke();
    }

    public void CreateDialogTagAction(string tag)
    {
        GameEvents.Instance.OnDialogueTag?.Invoke(tag);
    }

    public void OnDestroy()
    {
        GameEvents.Instance.OnDialogueStart -= StartNewDialog;
    }
}
    
}