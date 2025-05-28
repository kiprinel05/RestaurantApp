using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Restaurant.Models;

namespace Restaurant.Views.Controls
{
    public partial class StatusSelectorDialog : UserControl
    {
        public event Action<OrderStatus> StatusSelected;

        public List<string> Statuses { get; } = new List<string>(Enum.GetNames(typeof(OrderStatus)));

        public StatusSelectorDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && Enum.TryParse<OrderStatus>(btn.Content.ToString(), out var status))
            {
                StatusSelected?.Invoke(status);
            }
        }
    }
} 