#pragma warning disable 108, 109, 114

using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace NTools
{
    [ShowOdinSerializedPropertiesInInspector]
    public abstract class SerializedLazyMonoBehaviour : LazyMonoBehaviour, ISerializationCallbackReceiver, ISupportsPrefabSerialization
    {
        [SerializeField]
        [HideInInspector]
        private SerializationData serializationData;

        public void OnBeforeSerialize()
            => UnitySerializationUtility.SerializeUnityObject(this, ref serializationData);

        public void OnAfterDeserialize()
            => UnitySerializationUtility.DeserializeUnityObject(this, ref serializationData);

        public SerializationData SerializationData
        {
            get => serializationData;
            set => serializationData = value;
        }
    }
}