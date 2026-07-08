using System;

namespace NTools
{
    public class BasicVariable<T> : EventArgs
    {
        public T Value { get; set; }
        private BasicVariable (T value) => Value = value;

        public static implicit operator T(BasicVariable<T> variable) => variable.Value;
        public static implicit operator BasicVariable<T>(T value) => new(value);
    }
}
