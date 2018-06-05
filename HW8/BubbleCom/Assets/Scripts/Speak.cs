using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speak {
    public string content;
    //说话内容
    public string speaker;
    //发言者

    public Speak(string content, string speaker)
    {
        this.content = content;
        this.speaker = speaker;
    }
}
