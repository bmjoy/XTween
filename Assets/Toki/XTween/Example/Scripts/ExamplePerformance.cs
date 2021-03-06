﻿/**********************************************************************************
/*		File Name 		: XTweenExporter.cs
/*		Author 			: Robin
/*		Description 	: 
/*		Created Date 	: 2016-7-27
/*		Modified Date 	: 
/**********************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ExamplePerformance : ExampleBase
{
	/************************************************************************
	*	 	 	 	 	Static Variable Declaration	 	 	 	 	 	    *
	************************************************************************/
	
	/************************************************************************
	*	 	 	 	 	Static Method Declaration	 	 	 	     	 	*
	************************************************************************/
	
	/************************************************************************
	*	 	 	 	 	Private Variable Declaration	 	 	 	 	 	*
	************************************************************************/
	private Vector3 _position2D;
	private Vector3 _position3D;
	private bool _isBreak = false;
    
	/************************************************************************
	*	 	 	 	 	Protected Variable Declaration	 	 	 	 	 	*
	************************************************************************/
		

	/************************************************************************
	*	 	 	 	 	Public Variable Declaration	 	 	 	 	 		*
	************************************************************************/
	public Text textCode;
	public RectTransform rectUI;
		
	/************************************************************************
	*	 	 	 	 	Getter & Setter Declaration	 	 	 	 	 		*
	************************************************************************/
	
	/************************************************************************
	*	 	 	 	 	Initialize & Destroy Declaration	 	 	 		*
	************************************************************************/
	
	/************************************************************************
	*	 	 	 	 	Life Cycle Method Declaration	 	 	 	 	 	*
	************************************************************************/
	protected override IEnumerator StartExample()
	{
		yield return null;
		this._position2D = this.target2D.transform.localPosition;
		this._position3D = this.target3D.transform.localPosition;
	}
    
	/************************************************************************
	*	 	 	 	 	Coroutine Declaration	 	  			 	 		*
	************************************************************************/
	protected override IEnumerator CoroutineStart()
	{
		if( this._tween != null )
		{
			this._tween.Stop();
			this._tween = null;
		}
		this.target2D.transform.localPosition = this._position2D;
		this.target3D.transform.localPosition = this._position3D;
		List<GameObject> targetList = new List<GameObject>();
		const int COUNT = 100;
		for ( int i = 0; i < COUNT; ++i )
		{
			GameObject target = GameObject.Instantiate(this.target3D);
			Transform trans = target.transform;
			trans.SetParent( this.target3D.transform.parent );
			trans.localPosition = new Vector3( UnityEngine.Random.Range(-1200f,1200f), UnityEngine.Random.Range(-500f,500f), -100f );
			trans.localScale = Vector3.one * 100f;
			targetList.Add(target);
		}
		GC.Collect();
		yield return new WaitForSeconds(0.5f);
		TweenUIData data = this.uiContainer.Data;

		for ( int i = 0; i < COUNT; ++i )
		{
			this.StartXTween(targetList[i]);
			// this.StartiTween(targetList[i]);
		}
	
		while( true )
		{
			if( _isBreak )
			{
				GC.Collect();
				yield return new WaitForSeconds(0.1f);
				Debug.Break();
			}
			yield return null;
		}
	}

	private void StartXTween(GameObject target)
	{
		IAni ani = XTween.To(target, XHash.New.Position(0f,0f,-400f), 1f, Elastic.easeOut);
		ani.OnComplete = Executor.New(() => _isBreak = true);
		ani.Play();
	}

	private void StartiTween(GameObject target)
	{
		Hashtable hash = new Hashtable();
		hash.Add("x", 0f);
		hash.Add("y", 0f);
		hash.Add("z", -200f);
		hash.Add("time", 1f);
		hash.Add("islocal", true);
		hash.Add("easetype", "easeOutElastic");
		hash.Add("oncomplete", "OnComplete");
		hash.Add("oncompletetarget", this.gameObject);
		// iTween.MoveTo(target, hash);
	}
	
	void OnComplete()
	{
		_isBreak = true;
	}
	
	/************************************************************************
	*	 	 	 	 	Private Method Declaration	 	 	 	 	 		*
	************************************************************************/
    
	/************************************************************************
	*	 	 	 	 	Protected Method Declaration	 	 	 	 	 	*
	************************************************************************/
	
	/************************************************************************
	*	 	 	 	 	Public Method Declaration	 	 	 	 	 		*
	************************************************************************/
	public override void UIChangeHandler()
	{
		TweenUIData data = this.uiContainer.Data;
		string easing = data.easingType.ToString() + ".ease" + data.inOutType.ToString();
		string input = this.uiContainer.is3D ?
			"XTween<color=#DCDC9D>.To(</color>target3D, XHash.New<color=#DCDC9D>.AddX(</color><color=#A7CE89>800f</color><color=#DCDC9D>).AddY(</color><color=#A7CE89>300f</color><color=#DCDC9D>).AddZ(</color><color=#A7CE89>-1500f</color><color=#DCDC9D>), "+ data.time +"f,</color> "+ easing +"<color=#DCDC9D>).Play()</color>;" :
			"XTween<color=#DCDC9D>.To(</color>target2D, XHash.New<color=#DCDC9D>.AddX(</color><color=#A7CE89>800f</color><color=#DCDC9D>).AddY(</color><color=#A7CE89>300f</color><color=#DCDC9D>), "+ data.time +"f,</color> "+ easing +"<color=#DCDC9D>).Play()</color>;";
		this.textCode.text = input;
	}
}