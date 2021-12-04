using UnityEngine;

/// <summary>
/// Implements the IFactory interface for non-abstract types.
/// </summary>
/// <typeparam name="T">Specifies the non-abstract type to create.</typeparam>
public abstract class FactorySO<T> : ScriptableObject, IFactory<T>
{
    public abstract T Create();
}
