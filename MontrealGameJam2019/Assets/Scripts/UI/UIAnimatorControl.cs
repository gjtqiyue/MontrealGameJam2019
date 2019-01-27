using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimatorControl : MonoBehaviour
{
	public Animator uiAnimator;
    
	public void DeactivateAnimator() {
		uiAnimator.enabled = false;
	}
}
