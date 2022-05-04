using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable 
{
	Action<float> OnHit { get; set; }
}
