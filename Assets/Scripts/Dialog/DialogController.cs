using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;

/// <summary>
/// Parser f�r Dialoge, die mit dem Online-Editor inklewriter erstellt wurden:
/// https://www.inklewriter.com/
/// </summary>
public class DialogController : MonoBehaviour
{
    Dictionary<string, DialogPart> dialog;

    string startKey = "";
    string activeKey;

    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button[] optionButtons;


    public void StartNewDialog(string dialogJSON)
    {
        ParseDialog(dialogJSON);
        activeKey = startKey;
        UpdateUI(activeKey);
    }

    private void ParseDialog(string dialogJSON)
    {
        Debug.Log("Parse Dialog");
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
                if (c.ContainsKey("option"))
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
        for(int i = 0; i < optionButtons.Length; i++)
        {
            if (i < dialogPart.options.Count)
            {
                DialogOption option = dialogPart.options[i];
                Button button = optionButtons[i].GetComponent<Button>();
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = option.optionText;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnButtonClicked(option.nextDialogPartKey));
                if (option.dropItem != null)
                {
                    button.onClick.AddListener(() => DropItem(option.dropItem));
                }
                optionButtons[i].gameObject.SetActive(true);
            } else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
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
}