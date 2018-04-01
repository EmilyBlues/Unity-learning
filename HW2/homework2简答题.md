# 空间与运动_简答题

---

### 1、简答并用程序验证
>* 游戏对象运动的本质是什么？
    >>* 游戏对象运动的本质是：游戏对象在游戏每一帧的渲染过程中Transform属性在发生变化。这里物体的Transform属性是指Position与Rotation两个属性。

>* 请用三种方法以上方法，实现物体的抛物线运动。（如，修改Transform属性，使用向量Vector3的方法…）
    >>* 第一种方法，直接通过改变物体的Transform的position属性让物体实现抛物线运动。代码如下：  

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parabola : MonoBehaviour {
    public float speed1 = 4;
    public float speed2 = 0;
    public float speed = 3;
	// Use this for initialization
	void Start () {
        Debug.Log("Init start");
	}
	
	// Update is called once per frame
	void Update () {
        if(speed1 >= 0){
            speed1 = speed1 - 10 * Time.deltaTime;
            this.transform.position += Vector3.up * Time.deltaTime * speed1 / 2;
        }
        //此处为实现物体先上抛在竖直方向上位置的变化。
        else{
            speed2 = speed2 + 10 * Time.deltaTime;
            this.transform.position += Vector3.down * Time.deltaTime * speed2 / 2;
        }
        //此处为实现物体竖直方向速度减为0后
        //平抛运动在竖直方向上位置的变化。
        
        this.transform.position += Vector3.left * Time.deltaTime * speed;
        //此处为物体在水平上位置的变化。
	}
}
```

>>* 第二种方法，使用transform中的translate方法改变对象的位置。代码如下：
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parabola : MonoBehaviour {
    public float speed1 = 4;
    public float speed2 = 0;
    public float speed = 3;
	// Use this for initialization
	void Start () {
        Debug.Log("Init start");
	}
	
	// Update is called once per frame
	void Update () {
        if(speed1 >= 0)
        {
            speed1 = speed1 - 10 * Time.deltaTime;
            transform.Translate(Vector3.up * Time.deltaTime * speed1 / 2, Space.World);
        }
        //此处为实现物体先上抛在竖直方向上位置的变化。
        else
        {
            speed2 = speed2 + 10 * Time.deltaTime;
            transform.Translate(Vector3.down * Time.deltaTime * speed2 / 2, Space.World);
        }
        //此处为实现物体竖直方向速度减为0后
        //平抛运动在竖直方向上位置的变化。
        transform.Translate(Vector3.left * Time.deltaTime * speed);
		//此处为物体在水平上位置的变化。
	}
}

```

>>* 第三种方法，新建一个vector3对象，改变这个对象的值，然后直接赋给物体。代码如下：
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parabola : MonoBehaviour {
    public float speed1 = 4;
    public float speed2 = 0;
    public float speed = 3;
	// Use this for initialization
	void Start () {
        Debug.Log("Init start");
	}
	
	// Update is called once per frame
	void Update () {
        if(speed1 >= 0)
        {
            speed1 = speed1 - 10 * Time.deltaTime;
            Vector3 newOne = new Vector3(Time.deltaTime * speed, Time.deltaTime * speed1 / 2, 0);
            this.transform.position += newOne;
        }
        //此处为实现物体先上抛在竖直方向上位置的变化。
        else
        {
            speed2 = speed2 + 10 * Time.deltaTime;
            Vector3 newOne = new Vector3(Time.deltaTime * speed, -Time.deltaTime * speed2 / 2, 0);
            this.transform.position += newOne;
        }
        //此处为实现物体竖直方向速度减为0后
        //平抛运动在竖直方向上位置的变化。
		
	}
}

```
>* 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

>>* 
    首先，在游戏Scene中设置太阳、水星、金星、地球、火星、木星、土星、天王星和海王星。将图片放置在球体上面。完成这一步后Assets与GameObject分别如下两图所示：
![这里写图片描述](https://img-blog.csdn.net/20180401211437931?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
![这里写图片描述](https://img-blog.csdn.net/201804012116160?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)  
得到的最后九个球体的效果图如下（此时为运动前按直线排列好的图）：
![这里写图片描述](https://img-blog.csdn.net/20180401212155683?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
之后，我们要设置各个行星的公转与自转速度，以及各自的法平面。考虑到代码的相似性极高，我决定编写同一个脚本放到各个行星上去，脚本中考虑各行星的自转与公转周期不一致，参考地球的自转与公转周期，我定义了两个float型的变量：selfSpeed和Speed，还定义了一个运动中心参考物体，不过此处的参考物体全为太阳。  
又因为每个行星运动的法平面需要不同，所以在脚本中我又写了两个float型的随机数：rx与ry。用这两个随机数，构成一个Vector3变量axis，作为各行星运动的轴。  
整体的Rotation.cs脚本代码如下所示：
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    public Transform MyCenter;

    public float selfSpeed = 1;
    public float speed = 1;

    float ry, rx;

    // Use this for initialization
    void Start () {
        rx = Random.Range(10, 60);
        ry = Random.Range(10, 60);
	}
	

// Update is called once per frame
	void Update () {
        Vector3 axis = new Vector3(rx, ry, 0);
        this.transform.RotateAround(MyCenter.position, axis, speed * Time.deltaTime);
        this.transform.Rotate(Vector3.up * selfSpeed * Time.deltaTime);
    }
}

```
>>* 下一步就是将这段脚本挂在每一个行星上，并且计算出这个行星与地球自转公转的比值，设置速度。地球的自转周期大约是1天，地球的公转周期大约是366天。以火星为例，火星的自转周期大约是1.06天，公转周期大约是687天。  
我们设地球的公转速度在此处为10，自转速度为30；那么火星的公转速度可以计算得到应为5.3，自转速度应为29.2。由此得到地球与火星的设置分别为：
![这里写图片描述](https://img-blog.csdn.net/20180401213523409?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
(这是地球的设置）
![这里写图片描述](https://img-blog.csdn.net/20180401213600408?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
（这是火星的设置)
>>* 考虑到太阳只有自转没有公转，所以我把太阳的脚本单独了出来，只给太阳自转。代码如下：
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sun : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.up * 25 * Time.deltaTime);
    }
}

```

>>* 最后效果如图所示：
![这里写图片描述](https://img-blog.csdn.net/20180401213853141?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
![这里写图片描述](https://img-blog.csdn.net/20180401213901628?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)



>>* 太阳系的完成代码请看我的github，[请点击这里][https://github.com/EmilyBlues/Unity-learning/tree/master/HW2/SolarSystem]。




