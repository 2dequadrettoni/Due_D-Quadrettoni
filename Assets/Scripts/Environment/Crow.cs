using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour {

	private		Animator	pAnimator		= null;

	public		bool		bIsInAnimaion	= false;

	public		float		fAnimationTimer	= 0;

	// Use this for initialization
	void Start () {
		
		pAnimator = GetComponent<Animator>();

		if ( !pAnimator ) {
			print("Crow destroy, no animator found");
			Destroy( transform.parent.gameObject );
		}

	}

	void	OnAnimationEnd() {
		bIsInAnimaion = false;
		fAnimationTimer = Random.Range( 1.0f, 3.0f );
	}
	
	// Update is called once per frame
	void Update() {

		fAnimationTimer -= Time.unscaledDeltaTime;

		if ( fAnimationTimer < 0.0f & !bIsInAnimaion ) {
			bIsInAnimaion = true;

			pAnimator.Play( "Animate", 0, 0.0f );

		}

	}

}
