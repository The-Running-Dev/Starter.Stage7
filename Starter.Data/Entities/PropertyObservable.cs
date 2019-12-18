using System.ComponentModel;

namespace Starter.Data.Entities
{
    /// <summary>
    /// Implements a generic property observable based on INotifyPropertyChanged
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyObservable<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private T _value;

        public T Value
        {
            get => _value;

            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public PropertyObservable(T defaultValue)
        {
            _value = defaultValue;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}