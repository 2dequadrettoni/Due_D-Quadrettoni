using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public		enum			UsageType		{ NONE, INSTANT, ON_ACTION_END };


public class UsableObject : MonoBehaviour {

	[SerializeField]
	protected	UsageType			iUseType		= UsageType.NONE;
	public		UsageType UseType {
		get{ return iUseType; }
		set{ iUseType = value; }
	}

	public virtual	void	OnReset() {}

	public virtual	void	OnUse( Player User ) {}

}
