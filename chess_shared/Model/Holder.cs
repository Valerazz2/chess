using System;

namespace chess_shared.Model
{
    public class Holder<T>
    {
        private T _value;
        public event Action<T, T> ValueChanged;

        private bool _isSetting;

        public bool EventsCall = true;

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
                        _value = value;
                        if (EventsCall) ValueChanged?.Invoke(_value, oldValue);
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
