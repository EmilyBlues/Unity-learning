这次作业是在上次牧师与魔鬼的基础上加了AI的版本，实现的场景图如下：
![这里写图片描述](https://img-blog.csdn.net/20180619195022881?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
在点击Next之后，自动完成游戏的下一步。
AI实现的各个状态转化图如下：（偷懒了一下下，把师兄的搬过来了）
![这里写图片描述](https://img-blog.csdn.net/20180619195858772?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
需要添加的代码其实不多主要是一下几个：
##### 1、状态记录：
```C#
public enum BoatAction { P, D, PP, DD, PD};
//表示下一状态需要用船运走的人

//记录船的状态以及下一步的动作
public struct NextPassenger
{
    public bool isRight;
    public BoatAction boataction;
}
```

##### 2、寻找下一个状态
由于有些状态有两个可行解，所以下面采用随机数来选择到底使用哪个解：
```C#
private int randomValue()
{
    float num = Random.Range(0f, 1f);
    if (num <= 0.5f)
        return 1;
    else
        return 2;
}
```
用下面这个函数来寻找下一步解：
```C#
public void GetNextPassager()
{
    int from_priest = 0;
    int from_devil = 0;
    int to_priest = 0;
    int to_devil = 0;

    int[] fromCount = fromCoast.getCharacterNum();
    from_priest += fromCount[0];
    from_devil += fromCount[1];

    int[] toCount = toCoast.getCharacterNum();
    to_priest += toCount[0];
    to_devil += toCount[1];
    if (from_priest == 3 && from_devil == 3 && next.isRight)
    {
        int turn = randomValue();
        if (turn == 1)
        {
            next.boataction = BoatAction.PD;
        }
        else
        {
            next.boataction = BoatAction.DD;
        }
    }
    else if (next.isRight == false && from_priest == 2 && from_devil == 2)
    {
        next.boataction = BoatAction.P;
    }
    else if (next.isRight == false && from_priest == 3 && from_devil == 2)
    {
        next.boataction = BoatAction.D;
    }
    else if (next.isRight == false && from_priest == 3 && from_devil == 1)
    {
        next.boataction = BoatAction.D;
    }
    else if (next.isRight == true && from_priest == 3 && from_devil == 2)
    {
        next.boataction = BoatAction.DD;
    }
    else if (next.isRight == false && from_priest == 3 && from_devil == 0)
    {
        next.boataction = BoatAction.D;
    }
    else if(next.isRight == true && from_priest == 3 && from_devil == 1)
    {
        next.boataction = BoatAction.PP;
    }
    else if (next.isRight == false && from_priest == 1 && from_devil == 1)
    {
        next.boataction = BoatAction.PD;
    }
    else if(next.isRight == true && from_priest == 2 && from_devil == 2)
    {
        next.boataction = BoatAction.PP;
    }
    else if(next.isRight == false && from_priest == 0 && from_devil == 0)
    {
        next.boataction = BoatAction.D;
    }
    else if(next.isRight == true && from_priest == 0 && from_devil == 3)
    {
        next.boataction = BoatAction.DD;
    }
    else if(next.isRight == false && from_priest == 0 && from_devil == 1)
    {
        int turn = randomValue();
        if(turn == 1)
        {
            next.boataction = BoatAction.D;
        }
        else
        {
            next.boataction = BoatAction.P;
        }
    }
    else if(next.isRight == false && from_priest == 0 && from_devil == 2)
    {
        next.boataction = BoatAction.D;
    }
    else if(next.isRight == true && from_priest == 2 && from_devil == 1)
    {
        next.boataction = BoatAction.P;
    }
    else if(next.isRight == true && from_priest == 0 && from_devil == 2)
    {
        next.boataction = BoatAction.DD;
    }
    else if(next.isRight == true && from_priest == 1 && from_devil == 1)
    {
        next.boataction = BoatAction.PD;
    }
}
```

##### 3、完成动作的函数：
```C#
public void NextMove()
{
    Debug.Log(next.boataction);
    Debug.Log(gameStatus);
    if (gameStatus == 0)
    {
        GetNextPassager();
        if (next.isRight == true && next.boataction == BoatAction.PP)
        {
            MyCharacterController priest1 = fromCoast.FindCharacterOnTheCoast(0);
            characterIsClicked(priest1);
            MyCharacterController priest2 = fromCoast.FindCharacterOnTheCoast(0);
            characterIsClicked(priest2);
        }
        else if (next.isRight == true && next.boataction == BoatAction.P)
        {
            MyCharacterController priest1 = fromCoast.FindCharacterOnTheCoast(0);
            characterIsClicked(priest1);
        }
        else if (next.isRight == true && next.boataction == BoatAction.PD)
        {
            MyCharacterController priest1 = fromCoast.FindCharacterOnTheCoast(0);
            characterIsClicked(priest1);
            MyCharacterController devil1 = fromCoast.FindCharacterOnTheCoast(1);
            characterIsClicked(devil1);
        }
        else if (next.isRight == true && next.boataction == BoatAction.D)
        {
            MyCharacterController devil1 = fromCoast.FindCharacterOnTheCoast(1);
            characterIsClicked(devil1);
        }
        else if (next.isRight == true && next.boataction == BoatAction.DD)
        {
            MyCharacterController devil1 = fromCoast.FindCharacterOnTheCoast(1);
            characterIsClicked(devil1);
            MyCharacterController devil2 = fromCoast.FindCharacterOnTheCoast(1);
            characterIsClicked(devil2);
        }
        else if (next.isRight == false && next.boataction == BoatAction.PP)
        {
            MyCharacterController priest1 = toCoast.FindCharacterOnTheCoast(0);
            characterIsClicked(priest1);
            MyCharacterController priest2 = toCoast.FindCharacterOnTheCoast(0);
            characterIsClicked(priest2);
        }
        else if (next.isRight == false && next.boataction == BoatAction.P)
        {
            MyCharacterController priest1 = toCoast.FindCharacterOnTheCoast(0);
            characterIsClicked(priest1);
        }
        else if (next.isRight == false && next.boataction == BoatAction.PD)
        {
            MyCharacterController priest1 = toCoast.FindCharacterOnTheCoast(0);
            characterIsClicked(priest1);
            MyCharacterController devil1 = toCoast.FindCharacterOnTheCoast(1);
            characterIsClicked(devil1);
        }
        else if (next.isRight == false && next.boataction == BoatAction.D)
        {
            MyCharacterController devil1 = toCoast.FindCharacterOnTheCoast(1);
            characterIsClicked(devil1);
        }
        else if (next.isRight == false && next.boataction == BoatAction.DD)
        {
            MyCharacterController devil1 = toCoast.FindCharacterOnTheCoast(1);
            characterIsClicked(devil1);
            MyCharacterController devil2 = toCoast.FindCharacterOnTheCoast(1);
            characterIsClicked(devil2);
        }
        next.isRight = !next.isRight;
        gameStatus = 1;
    }
    else if(gameStatus == 1)
    {
        moveBoat();
        gameStatus = 2;
    }
    else if(gameStatus == 2)
    {
        MyCharacterController[] pass = boat.GetPassengerOnTheBoat();
        for(int i = 0; i < pass.Length; i++)
        {
            if(pass[i] != null)
                characterIsClicked(pass[i]);
        }
        gameStatus = 0;
    }
}
```
##### 4、给next的button添加函数：
```C#
void OnGUI() {
    if (status == 1)
    {
    //输了，游戏重来
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Gameover!", style);
        if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
        {
            status = 0;
            action.restart();
        }
    }
    else if (status == 2)
    {
    //赢了，游戏重来
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", style);
        if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
        {
            status = 0;
            action.restart();
        }
    }
    else if (status == 0)
    {
    //游戏正在继续，点击了next按钮
        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 200, 100, 50), "Next", buttonStyle))
        {
            action.NextMove();
        }
    }
}
```
以上就是需要添加的代码，下面是github[传送门]()。
这是视频[传送门]()。