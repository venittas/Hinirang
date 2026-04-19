using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string characterName = "?????????????";
    [TextArea(3, 10)]
    public string line = "................................................................";

    public DialogueLine(string characterName, string line)
    {
        this.characterName = (!string.IsNullOrWhiteSpace(characterName)) ? characterName : this.characterName;
        this.line = (!string.IsNullOrWhiteSpace(line)) ? line : this.line;
    }
}
