using System.Collections.Generic;

public class DialogPart
{
    public string message;

    public string tag;

    public List<DialogOption> options;
}

public class DialogOption
{
    public string optionText;

    public string tag;

    public string nextDialogPartKey;

    public override string ToString()
    {
        return $"optionText: {optionText}, tag: {tag}, nextDialogPartKey: {nextDialogPartKey}";
    }
}
