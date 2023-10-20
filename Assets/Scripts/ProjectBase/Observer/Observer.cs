using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer<T>
{
    // Update is called once per frame
    public abstract void ToUpdate(T value);
}
