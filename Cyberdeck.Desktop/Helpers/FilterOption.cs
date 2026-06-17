using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cyberdeck.Desktop.Helpers
{
    public partial class FilterOption<T> : ObservableObject
    {
        public T Value { get; }

        private readonly Action _onChanged;

        [ObservableProperty]
        private bool isSelected;

        public FilterOption(T value, Action onChanged)
        {
            Value = value;
            _onChanged = onChanged;
        }

        partial void OnIsSelectedChanged(bool oldValue, bool newValue)
        {
            _onChanged?.Invoke();
        }
    }
}
