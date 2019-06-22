using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLocation : MonoBehaviour
{
	[SerializeField] private float radius = default;

	public float Radius {  get { return radius; } }

	private void OnDrawGizmos()
	{
		var parent = this.GetComponentInParent<Spawner>();
		if (parent)
			Gizmos.color = parent.color;
		else
			Gizmos.color = Color.magenta;// new Color(1.0f, 0.9f, 0.02f, 0.25f);
		//Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(this.transform.position, radius);
	}
}
