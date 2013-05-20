namespace Zapos.Common.Styles
{
    public struct InheritStyle<T>
    {
        private T _value;

        private readonly T _baseValue;

        private bool _isBase;

        public InheritStyle(T baseValue)
        {
            _baseValue = baseValue;
            _value = baseValue;
            _isBase = true;
        }

        public InheritStyle(T baseValue, T value)
        {
            _baseValue = baseValue;
            _value = value;
            _isBase = false;
        }

        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    _isBase = false;
                }
            }
        }

        public bool IsBase
        {
            get { return _isBase; }
            set { _isBase = value; }
        }

        public override bool Equals(object obj)
        {
            return this == (InheritStyle<T>)obj;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public void ResetToBase()
        {
            _value = _baseValue;
            _isBase = true;
        }

        public static implicit operator T(InheritStyle<T> x)
        {
            return x._value;
        }

        public static implicit operator InheritStyle<T>(T x)
        {
            return new InheritStyle<T>(default(T), x);
        }

        public static bool operator ==(InheritStyle<T> a, InheritStyle<T> b)
        {
            return a._value.Equals(b._value);
        }

        public static bool operator !=(InheritStyle<T> a, InheritStyle<T> b)
        {
            return !(a == b);
        }

        public static bool operator ==(T a, InheritStyle<T> b)
        {
            return a.Equals(b._value);
        }

        public static bool operator !=(T a, InheritStyle<T> b)
        {
            return !(a == b);
        }

        public static bool operator ==(InheritStyle<T> a, T b)
        {
            return a._value.Equals(b);
        }

        public static bool operator !=(InheritStyle<T> a, T b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}