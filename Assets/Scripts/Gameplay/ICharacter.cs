using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter 
{
	Action<float> OnHit { get; set; }
}
