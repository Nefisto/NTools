using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

/// <summary>
/// Responsable to serialize plain class on inspector allow me to work directly with plain class on code but also
///     use unity as an injector 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ScriptableObjectFactory<T> : SerializedScriptableObject
{
    [HideLabel]
    [InlineProperty]
    [SerializeField]
    public T serializedField;

    public virtual T GetInstance() => (T)SerializationUtility.CreateCopy(serializedField);
}