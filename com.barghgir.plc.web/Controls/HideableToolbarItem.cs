using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.Controls
{
    public partial class HideableToolbarItem : ToolbarItem
    {
        // SUPPORT:
        // https://support.serviceshub.microsoft.com/supportforbusiness/create?sapId=342cb585-b837-0fbc-41b7-7c3dc2a691c9
        // https://support.serviceshub.microsoft.com/supportforbusiness/onboarding?origin=/supportforbusiness/create
        // MVVM: https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/mvvm?view=net-maui-7.0
        // Freaky Controls: https://github.com/FreakyAli/MAUI.FreakyControls
        public HideableToolbarItem(): base()
        {
            //UpdateVisibility(null);
        }

        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(IsVisible),
                returnType: typeof(bool),
                declaringType: typeof(HideableToolbarItem),
                defaultValue: true,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: OnPropertyChanged,
                //validateValue: OnValidate,
                propertyChanging: OnPropertyChanging
                //coerceValue: OnCoerceValue,
                //defaultValueCreator: DefaultValueCreator
                );
        //private static object DefaultValueCreator(BindableObject bindable)
        //{
        //    // 1
        //    //throw new NotImplementedException();
        //    return false;
        //}
        //private static object OnCoerceValue(BindableObject bindable, object value)
        //{
        //    // 3
        //    return bool.TryParse(value.ToString(), out var result) ? result : false;
        //}
        //private static bool OnValidate(BindableObject bindable, object value)
        //{
        //    // 2
        //    return value.GetType() == typeof(bool);
        //}
        private static void OnPropertyChanging(BindableObject bindable, object oldValue, object newValue)
        {
            // do nothing
        }
        public bool IsVisible
        {
            // 6 (from 5)
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }
        protected override void OnParentChanged()
        {
            // 4
            base.OnParentChanged();
            UpdateVisibility(this, null, null);
        }
        public static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable == null) return;
            var item = (HideableToolbarItem)bindable;
            Task.Run(async delegate
            {
                await Task.Delay(1000);
                item.UpdateVisibility(bindable, oldValue, newValue);
            });
        }
        private void UpdateVisibility(BindableObject bindable, object oldValue, object newValue)
        {
            //if (item?.Parent == null) return;

            var item = (HideableToolbarItem)bindable ?? this;

            //bool isVisible = (bool)oldValue;
            //bool isHidden = !isVisible;
            bool wantVisible = (bool?)newValue ?? IsVisible;
            bool wantHidden = !wantVisible;

            //await Task.Delay(10000);

            IList<ToolbarItem> toolbarItems = ((ContentPage)Parent)?.ToolbarItems;
            bool isVisible = toolbarItems?.Contains(item) ?? false;
            bool isHidden = !isVisible;

            if (toolbarItems == null)
            {
                if (wantVisible && isHidden)
                    (item.Parent as ContentPage)?.ToolbarItems.Add(item);
                else if (wantHidden && isVisible)
                    (item.Parent as ContentPage)?.ToolbarItems.Remove(item);
            }
            else
            {
                if (wantVisible && isHidden)
                    Application.Current.Dispatcher.Dispatch(() => { toolbarItems.Add(item); });
                else if (wantHidden && isVisible)
                    Application.Current.Dispatcher.Dispatch(() => { toolbarItems.Remove(item); });
            }

        }
        private void UpdateVisibility(bool? newValue)
        {
            // 5 (from 4)
            if (Parent == null) return;

            bool wantVisible = newValue ?? IsVisible;
            bool wantHidden = !wantVisible;

            IList<ToolbarItem> toolbarItems = ((ContentPage)Parent)?.ToolbarItems;
            bool isVisible = toolbarItems?.Contains(this) ?? false;
            bool isHidden = !isVisible;

            if (isHidden && wantVisible)
                Application.Current.Dispatcher.Dispatch(() => { toolbarItems.Add(this); });
            else if (isVisible && wantHidden)
                Application.Current.Dispatcher.Dispatch(() => { toolbarItems.Remove(this); });
        }
    }
}
