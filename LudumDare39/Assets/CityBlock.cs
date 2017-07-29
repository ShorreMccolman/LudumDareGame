using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBlock : MonoBehaviour {

	SpriteRenderer renderer;
	public SpriteRenderer Renderer{
		get
		{
			if (renderer == null)
				renderer = GetComponent<SpriteRenderer> ();
			return renderer;
		}
	}
}
