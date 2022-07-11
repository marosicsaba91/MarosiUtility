﻿using System;
using System.Reflection;
using UnityEngine;

namespace MUtility
{
public class ValueChangeCallbackAttribute : FormattingAttribute
{
	readonly string _callbackMember;
	MethodInfo _methodInfo;
	int _parameterLength;
	bool _initialized;

	public ValueChangeCallbackAttribute(string callbackName)
	{
		_callbackMember = callbackName;
		_initialized = false;
	}

	static BindingFlags Binding => InspectorDrawingUtility.bindings;

	public void Initialize(object owner)
	{
		if (_initialized) return;
		
		_methodInfo = owner.GetType().GetMethod(_callbackMember, Binding);
		if(_methodInfo!= null)
			_parameterLength = _methodInfo.GetParameters().Length;
		_initialized = true;
	}

	public void CallBack(object target, object oldValue, object newValue)
	{
			if (_methodInfo == null)
				Debug.LogWarning("No callback method");
			else if (_parameterLength == 0)
				_methodInfo.Invoke(target, Array.Empty<object>());
			else if (_parameterLength == 1)
				_methodInfo.Invoke(target, new []{oldValue});
			else if (_parameterLength == 2)
				_methodInfo.Invoke(target, new []{oldValue, newValue});
			else
				Debug.LogWarning($"Too many parameters  to the callback function: {_methodInfo.Name}");
	}
}
}