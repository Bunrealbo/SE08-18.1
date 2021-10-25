using System;
using UnityEngine;

[ExecuteInEditMode]
public class _2DC_ShaderLerpDemo : MonoBehaviour
{
	private void Update()
	{
		if (this.mat != null)
		{
			this.mat.SetFloat(this.variable, this.anm.Evaluate(Time.time * this.Speed) * this.Mul);
		}
	}

	public Material mat;

	public string variable;

	public AnimationCurve anm;

	public float Mul = 1f;

	public float Speed = 1f;
}
