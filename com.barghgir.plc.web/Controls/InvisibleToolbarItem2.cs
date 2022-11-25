using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.Controls
{
    partial class InvisibleToolbarItem2 : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RefreshVisibilityCommand))]
        ToolbarItem toolbarItem;

        Label label;

        public InvisibleToolbarItem2(ToolbarItem toolbarItem) : base()
        {
            this.toolbarItem = toolbarItem;
        }

        //public static readonly BindableProperty IsVisibleProperty =
        //    BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(InvisableToolbarItem2), true, BindingMode.OneWay, propertyChanged: OnIsVisibleChanged);
        [NotifyCanExecuteChangedFor(nameof(RefreshVisibilityCommand))]
        [ObservableProperty]
        bool isVisible;

        //public bool IsVisible
        //{
        //    get => (bool)GetValue(IsVisibleProperty);
        //    set => SetValue(IsVisibleProperty, value);
        //}

        //protected override void OnParentChanged()
        //{
        //    base.OnParentChanged();
        //
        //    RefreshVisibility();
        //}

        //private static void OnIsVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        //{
        //    var item = bindable as InvisableToolbarItem2;
        //
        //    item.RefreshVisibility();
        //}

        [RelayCommand]
        public void RefreshVisibility()
        {
            var parent = toolbarItem.Parent;

            if (parent == null)
            {
                return;
            }

            bool value = IsVisible;

            var toolbarItems = ((ContentPage)parent).ToolbarItems;

            if (value && !toolbarItems.Contains(toolbarItem))
            {
                Application.Current.Dispatcher.Dispatch(() => { toolbarItems.Add(toolbarItem); });
            }
            else if (!value && toolbarItems.Contains(toolbarItem))
            {
                Application.Current.Dispatcher.Dispatch(() => { toolbarItems.Remove(toolbarItem); });
            }
        }
    }
}
