using System;

namespace Zapos.Common.Styles
{
    [Serializable]
    public struct InheritStyle<T>
    {
        private readonly bool _hasValue;

        private readonly T _value;

        public bool HasValue
        {
            get
            {
                return this._hasValue;
            }
        }

        public T Value
        {
            get
            {
                if (!this.HasValue)
                {
                    throw new InvalidOperationException("Style is default.");
                }

                return this._value;
            }
        }

        public InheritStyle(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public static implicit operator InheritStyle<T>(T value)
        {
            return new InheritStyle<T>(value);
        }

        /// <exception cref="InvalidOperationException">If Style have a default value</exception>
        public static implicit operator T(InheritStyle<T> value)
        {
            if (!value.HasValue)
            {
                throw new InvalidOperationException("Style is default.");
            }

            return value.Value;
        }

        public T GetValueOrDefault()
        {
            return this._value;
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return this.HasValue ? this._value : defaultValue;
        }

        public override bool Equals(object other)
        {
            if (!this.HasValue)
            {
                return other == null;
            }

            if (other == null)
            {
                return false;
            }

            return this._value.Equals(other);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyFieldInGetHashCode
            return !this.HasValue ? 0 : this._value.GetHashCode();
        }

        public override string ToString()
        {
            return this.HasValue ? this._value.ToString() : string.Empty;
        }

        public static bool operator ==(InheritStyle<T> a, InheritStyle<T> b)
        {
            if (!a._hasValue || !b._hasValue)
            {
                return false;
            }

            return a._value.Equals(b._value);
        }

        public static bool operator !=(InheritStyle<T> a, InheritStyle<T> b)
        {
            return !(a == b);
        }

        public static bool operator ==(T a, InheritStyle<T> b)
        {
            if (!b._hasValue)
            {
                return false;
            }

            return a.Equals(b._value);
        }

        public static bool operator !=(T a, InheritStyle<T> b)
        {
            return !(a == b);
        }

        public static bool operator ==(InheritStyle<T> a, T b)
        {
            if (!a._hasValue)
            {
                return false;
            }

            return a._value.Equals(b);
        }

        public static bool operator !=(InheritStyle<T> a, T b)
        {
            return !(a == b);
        }
    }
}
