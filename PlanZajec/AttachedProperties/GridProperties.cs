using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PlanZajec
{
    public class GridProperties
    {
        public static uint GetRowAmount(DependencyObject obj)
        {
            return (uint)obj.GetValue(RowAmountProperty);
        }

        public static void SetRowAmount(DependencyObject obj, uint value)
        {
            obj.SetValue(RowAmountProperty, value);
        }

        // Using a DependencyProperty as the backing store for RowAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowAmountProperty =
            DependencyProperty.RegisterAttached("RowAmount", typeof(uint), typeof(GridProperties), new PropertyMetadata((uint)0, RowAmountChanged));

        /// <summary>
        /// Adjustes the amount of rows when the property is changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void RowAmountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Cast dependancy object to grid
            var grid = d as Grid;

            //If the object isn't a grid there is nothing to do
            if (grid == null) return;

            //Get current row amount
            var current_row_amount = grid.RowDefinitions.Count;

            //Remove rows if needed
            while (current_row_amount > (uint)e.NewValue)
            {
                grid.RowDefinitions.RemoveAt(--current_row_amount);
            }

            //Add rows if needed
            while (current_row_amount < (uint)e.NewValue)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                current_row_amount++;
            }
        }
    }
}
