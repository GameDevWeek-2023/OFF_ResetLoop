using System.Collections.Generic;

public class DialogPart
{
    public string message;

    public List<DialogOption> options;
}

public class DialogOption
{
    public string optionText;

    public string dropItem;

    public string nextDialogPartKey;
}
