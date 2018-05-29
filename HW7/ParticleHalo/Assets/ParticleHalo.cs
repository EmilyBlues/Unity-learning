using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHalo : MonoBehaviour {

    private ParticleSystem particleSys;
    private ParticleSystem.Particle[] particles;
    private CirclePosition[] circles;
    private int tier = 10;

    public Gradient colorGradient;
    public int count = 10000;       
    // 粒子数量
    public float size = 0.03f;     
    // 粒子大小
    public float minRadius = 5.0f;  
    // 最小半径
    public float maxRadius = 12.0f; 
    // 最大半径
    public bool clockwise = true;   
    // 顺时针|逆时针
    public float speed = 2f;        
    // 速度
    public float pingPong = 0.02f;  
    // 游离范围

    // Use this for initialization
    void Start () {
        particles = new ParticleSystem.Particle[count];
        circles = new CirclePosition[count];
        //初始化粒子系统
        particleSys = this.GetComponent<ParticleSystem>();
        particleSys.startSpeed = 0;
        particleSys.startSize = size;
        particleSys.loop = false;
        particleSys.maxParticles = count;      // 设置最大粒子量
        particleSys.Emit(count);               // 发射粒子
        particleSys.GetParticles(particles);

        //初始化颜色
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[5];
        alphaKeys[0].time = 0.0f;
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[1].time = 0.4f;
        alphaKeys[1].alpha = 0.4f;
        alphaKeys[2].time = 0.6f;
        alphaKeys[2].alpha = 1.0f;
        alphaKeys[3].time = 0.9f;
        alphaKeys[3].alpha = 0.4f;
        alphaKeys[4].time = 1.0f;
        alphaKeys[4].alpha = 0.9f;
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0.0f;
        colorKeys[0].color = Color.white;
        colorKeys[1].time = 1.0f;
        colorKeys[1].color = Color.white;
        colorGradient.SetKeys(colorKeys, alphaKeys);

        RandomlySpread();   // 初始化各粒子位置
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            if (clockwise)
                circles[i].angle -= (i % tier + 1) * (speed / circles[i].radius / tier);
            else
                circles[i].angle += (i % tier + 1) * (speed / circles[i].radius / tier);

            circles[i].time += Time.deltaTime;
            circles[i].radius += Mathf.PingPong(circles[i].time / minRadius / maxRadius, pingPong) - pingPong / 2.0f;
            circles[i].angle = (360.0f + circles[i].angle) % 360.0f;
            float theta = circles[i].angle / 180 * Mathf.PI;
            particles[i].position = new Vector3(circles[i].radius * Mathf.Cos(theta), circles[i].radius * Mathf.Sin(theta), 0f);
            particles[i].color = colorGradient.Evaluate(circles[i].angle / 360.0f);
        }
        particleSys.SetParticles(particles, particles.Length);
    }

    void RandomlySpread()
    {
        for (int i = 0; i < count; i++)
        {
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);

            // 随机每个粒子的角度  
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;

            // 随机每个粒子的游离起始时间  
            float time = Random.Range(0.0f, 360.0f);

            circles[i] = new CirclePosition(radius, angle, time);

            particles[i].position = new Vector3(circles[i].radius * Mathf.Cos(theta), circles[i].radius * Mathf.Sin(theta), 0f);
        }

        particleSys.SetParticles(particles, particles.Length);
    }
}

public class CirclePosition
{
    public float radius = 0f, angle = 0f, time = 0f;
    public CirclePosition(float radius, float angle, float time)
    {
        this.radius = radius;
        this.angle = angle;
        this.time = time;
    }
}