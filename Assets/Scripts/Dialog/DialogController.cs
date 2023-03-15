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
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button[] optionButtons;

    public void Start()
    {
        GameEvents.Instance.OnDialogueStart += StartNewDialog;
    }


    public void StartNewDialog(string dialogJSON)
    {
        Debug.Log("Start Dialog");
        ParseDialog(dialogJSON);
        activeKey = startKey;
        UpdateUI(activeKey);
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

                    ParseOptionTextForDropItem(option);

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

    private void ParseOptionTextForDropItem(DialogOption option)
    {
        if (option.optionText.Contains("{"))
        {
            int index = option.optionText.IndexOf("{");

            option.dropItem = option.optionText.Substring(index + 1).Replace("}", "").Trim();

            option.optionText = option.optionText.Substring(0, index).Trim();
        }
    }

    private void UpdateUI(string key)
    { 
        DialogPart dialogPart = dialog[key];
        messageText.text = dialogPart.message;
        
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

                if (option.dropItem != null)
                {
                    button.onClick.AddListener(() => DropItem(option.dropItem));
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

    public void DropItem(string item)
    {
        Debug.Log($"Drop: {item}");
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
        GameEvents.Instance.OnDialogueClosed();
    }
}
    
}