using System;

namespace chess_shared.Model
{
    public class Holder<T>
    {
        private T _value;
        public event Action<T, T> ValueChanged;

        private bool _isSetting;

        public T Value
        {
            get => _value;

            set
            {
                if (_isSetting)
                {
                    throw new Exception("Shouldn't be recursive");
                }
                if (!Equals(value, _value))
                {
                    _isSetting = true;
                    try
                    {
                        var oldValue = _value;
                        ValueChanged?.Invoke(_value = value, oldValue);
                    }
                    finally
                    {
                        _isSetting = false;
                    }
                }
            }
        }
    }
}
