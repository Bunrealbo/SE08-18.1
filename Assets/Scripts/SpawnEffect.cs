using System;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    private void Start()
    {
        this.shaderProperty = Shader.PropertyToID("_cutoff");
        this._renderer = base.GetComponent<Renderer>();
        this.ps = base.GetComponentInChildren<ParticleSystem>();
        var temp = this.ps.main;

        temp.duration = this.spawnEffectTime;
        this.ps.Play();
    }

    private void Update()
    {
        if (this.timer < this.spawnEffectTime + this.pause)
        {
            this.timer += Time.deltaTime;
        }
        else
        {
            this.ps.Play();
            this.timer = 0f;
        }
        this._renderer.material.SetFloat(this.shaderProperty, this.fadeIn.Evaluate(Mathf.InverseLerp(0f, this.spawnEffectTime, this.timer)));
    }

    public float spawnEffectTime = 2f;

    public float pause = 1f;

    public AnimationCurve fadeIn;

    private ParticleSystem ps;

    private float timer;

    private Renderer _renderer;

    private int shaderProperty;
}
