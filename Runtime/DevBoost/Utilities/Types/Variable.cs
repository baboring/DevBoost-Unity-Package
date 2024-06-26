﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevBoost.Extensions;

namespace DevBoost {
    
	public interface IVariable
	{
		System.Type Type {get;}
		object Value {get;}
	}

	public class Variable : IVariable
	{
		public System.Type Type {get; private set;}
		public object Value { get; set; }

		public Variable(System.Type type, object value = null) {
			Type = type;
			Value = (value == null)? type.GetDefault() : value;
		}

		public T GetValue<T>() {
			if(typeof(T) == this.Type)
				return (T)Value;
			throw new System.ArgumentException("Type Missmatch or undefined !!");
		}

		public bool SetValue<T>(T val) {
			if(typeof(T) != this.Type)
				return false;
			Value = val;
			return true;
		}
	}		

}