using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class DialogParser : MonoBehaviour
{
    [SerializeField] private TextAsset dialogFile;

    private void Start()
    {
        ParseDialog();
    }

    public void ParseDialog()
    {
        Dictionary<string, DialogPart> dialog = new Dictionary<string, DialogPart>();
        
        JObject parent = JObject.Parse(dialogFile.text);

        foreach (JToken token in parent["data"]["stitches"].Children<JObject>())
        {
            
        }


        
    }
}
