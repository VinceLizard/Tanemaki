using UnityEngine;

[System.Serializable]
public struct RangeFloat
{
	public float min;
	public float max;

	public RangeFloat(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	public bool Contains(float v)
	{
		return v >= this.min && v <= this.max;
	}

	public float Random()
	{
		return UnityEngine.Random.Range(this.min, this.max);
	}

	public float Clamp(float v)
	{
		return Mathf.Clamp(v, this.min, this.max);
	}

	public float Lerp(float t)
	{
		return Mathf.Lerp(this.min, this.max, t);
	}
}

