using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningGenerator
	{
		private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
		{
			if (directionNormalized == Vector3.zero)
			{
				side = Vector3.right;
				return;
			}
			float x = directionNormalized.x;
			float y = directionNormalized.y;
			float z = directionNormalized.z;
			float num = Mathf.Abs(x);
			float num2 = Mathf.Abs(y);
			float num3 = Mathf.Abs(z);
			float num4;
			float num5;
			float num6;
			if (num >= num2 && num2 >= num3)
			{
				num4 = 1f;
				num5 = 1f;
				num6 = -(y * num4 + z * num5) / x;
			}
			else if (num2 >= num3)
			{
				num6 = 1f;
				num5 = 1f;
				num4 = -(x * num6 + z * num5) / y;
			}
			else
			{
				num6 = 1f;
				num4 = 1f;
				num5 = -(x * num6 + y * num4) / z;
			}
			side = new Vector3(num6, num4, num5).normalized;
		}

		protected virtual void OnGenerateLightningBolt(LightningBolt bolt, Vector3 start, Vector3 end, LightningBoltParameters p)
		{
			this.GenerateLightningBoltStandard(bolt, start, end, p.Generations, p.Generations, 0f, p);
		}

		public bool ShouldCreateFork(LightningBoltParameters p, int generation, int totalGenerations)
		{
			return generation > p.generationWhereForksStop && generation >= totalGenerations - p.forkednessCalculated && (float)p.Random.NextDouble() < p.Forkedness;
		}

		public void CreateFork(LightningBolt bolt, LightningBoltParameters p, int generation, int totalGenerations, Vector3 start, Vector3 midPoint)
		{
			if (this.ShouldCreateFork(p, generation, totalGenerations))
			{
				Vector3 b = (midPoint - start) * p.ForkMultiplier();
				Vector3 end = midPoint + b;
				this.GenerateLightningBoltStandard(bolt, midPoint, end, generation, totalGenerations, 0f, p);
			}
		}

		public void GenerateLightningBoltStandard(LightningBolt bolt, Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount, LightningBoltParameters p)
		{
			if (generation < 1)
			{
				return;
			}
			LightningBoltSegmentGroup lightningBoltSegmentGroup = bolt.AddGroup();
			lightningBoltSegmentGroup.Segments.Add(new LightningBoltSegment
			{
				Start = start,
				End = end
			});
			float num = (float)generation / (float)totalGenerations;
			num *= num;
			lightningBoltSegmentGroup.LineWidth = p.TrunkWidth * num;
			lightningBoltSegmentGroup.Generation = generation;
			lightningBoltSegmentGroup.Color = p.Color;
			lightningBoltSegmentGroup.Color.a = (byte)(255f * num);
			lightningBoltSegmentGroup.EndWidthMultiplier = p.EndWidthMultiplier * p.ForkEndWidthMultiplier;
			if (offsetAmount <= 0f)
			{
				offsetAmount = (end - start).magnitude * ((generation == totalGenerations) ? p.ChaosFactor : p.ChaosFactorForks);
			}
			while (generation-- > 0)
			{
				int startIndex = lightningBoltSegmentGroup.StartIndex;
				lightningBoltSegmentGroup.StartIndex = lightningBoltSegmentGroup.Segments.Count;
				for (int i = startIndex; i < lightningBoltSegmentGroup.StartIndex; i++)
				{
					start = lightningBoltSegmentGroup.Segments[i].Start;
					end = lightningBoltSegmentGroup.Segments[i].End;
					Vector3 vector = (start + end) * 0.5f;
					Vector3 b;
					this.RandomVector(bolt, ref start, ref end, offsetAmount, p.Random, out b);
					vector += b;
					lightningBoltSegmentGroup.Segments.Add(new LightningBoltSegment
					{
						Start = start,
						End = vector
					});
					lightningBoltSegmentGroup.Segments.Add(new LightningBoltSegment
					{
						Start = vector,
						End = end
					});
					this.CreateFork(bolt, p, generation, totalGenerations, start, vector);
				}
				offsetAmount *= 0.5f;
			}
		}

		public Vector3 RandomDirection3D(System.Random r)
		{
			float num = 2f * (float)r.NextDouble() - 1f;
			Vector3 result = this.RandomDirection2D(r) * Mathf.Sqrt(1f - num * num);
			result.z = num;
			return result;
		}

		public Vector3 RandomDirection2D(System.Random r)
		{
			float f = (float)r.NextDouble() * 2f * 3.14159274f;
			return new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
		}

		public Vector3 RandomDirection2DXZ(System.Random r)
		{
			float f = (float)r.NextDouble() * 2f * 3.14159274f;
			return new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
		}

		public void RandomVector(LightningBolt bolt, ref Vector3 start, ref Vector3 end, float offsetAmount, System.Random random, out Vector3 result)
		{
			if (bolt.CameraMode == CameraMode.Perspective)
			{
				Vector3 vector = (end - start).normalized;
				Vector3 vector2 = Vector3.Cross(start, end);
				if (vector2 == Vector3.zero)
				{
					this.GetPerpendicularVector(ref vector, out vector2);
				}
				else
				{
					vector2.Normalize();
				}
				float d = ((float)random.NextDouble() + 0.1f) * offsetAmount;
				float num = (float)random.NextDouble() * 3.14159274f;
				vector *= (float)Math.Sin((double)num);
				Quaternion rotation;
				rotation.x = vector.x;
				rotation.y = vector.y;
				rotation.z = vector.z;
				rotation.w = (float)Math.Cos((double)num);
				result = rotation * vector2 * d;
				return;
			}
			if (bolt.CameraMode == CameraMode.OrthographicXY)
			{
				end.z = start.z;
				Vector3 normalized = (end - start).normalized;
				Vector3 a = new Vector3(-normalized.y, normalized.x, 0f);
				float d2 = (float)random.NextDouble() * offsetAmount * 2f - offsetAmount;
				result = a * d2;
				return;
			}
			end.y = start.y;
			Vector3 normalized2 = (end - start).normalized;
			Vector3 a2 = new Vector3(-normalized2.z, 0f, normalized2.x);
			float d3 = (float)random.NextDouble() * offsetAmount * 2f - offsetAmount;
			result = a2 * d3;
		}

		public void GenerateLightningBolt(LightningBolt bolt, LightningBoltParameters p, out Vector3 start, out Vector3 end)
		{
			start = p.ApplyVariance(p.Start, p.StartVariance);
			end = p.ApplyVariance(p.End, p.EndVariance);
			this.OnGenerateLightningBolt(bolt, start, end, p);
		}

		public static readonly LightningGenerator GeneratorInstance = new LightningGenerator();
	}
}
