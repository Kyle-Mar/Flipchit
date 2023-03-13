using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Result<T>
{
    public Result(T result) : this()
    {
        Value = result;
        Success = Value is T;
    }

    public bool Success { get;  }
    public T Value { get; }
}