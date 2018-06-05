using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComFactory {
    public List<Speak> com;
    //储存所有对话

    private static ComFactory instance;

	// Use this for initialization
    private ComFactory () {
        com = new List<Speak>();
        com.Add(new Speak("你好呀，你喜欢看电影吗？", "left"));
        com.Add(new Speak("我喜欢呢！", "right"));
        com.Add(new Speak("你都喜欢什么电影？", "left"));
        com.Add(new Speak("《大话西游》", "right"));
        com.Add(new Speak("我也喜欢呢！", "left"));
        com.Add(new Speak("你还喜欢什么？", "right"));
        com.Add(new Speak("《2046》", "left"));
        com.Add(new Speak("都是文艺片呢~", "right"));
        com.Add(new Speak("曾经我也是文艺青年", "left"));
        com.Add(new Speak("都过去了。", "right"));
        com.Add(new Speak("你单身吗？", "left"));
        com.Add(new Speak("emmm，没有", "right"));
        com.Add(new Speak("……，再见吧！", "left"));
	}

    public static ComFactory GetInstance()
    {
        if(instance == null)
        {
            instance = new ComFactory();
        }
        return instance;
    }
}
