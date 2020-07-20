using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvidencijaPrirodnihSpomenika.Komande
{
    class RoutedCommands
    {
        public static readonly RoutedUICommand AddSpomenik = new RoutedUICommand(
           "Add Spomenik",
           "AddSpomenik",
           typeof(RoutedCommand),
           new InputGestureCollection()
           {
                new KeyGesture(Key.D, ModifierKeys.Control)
           }
           );

        public static readonly RoutedUICommand ShowSpomenik = new RoutedUICommand(
            "Show Spomenik",
            "ShowSpomenik",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Alt)
            }
            );

        public static readonly RoutedUICommand AddType = new RoutedUICommand(
            "Add Type",
            "AddType",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.T, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand ShowTypes = new RoutedUICommand(
            "Show Types",
            "ShowTypes",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.T, ModifierKeys.Alt)
            }
            );

        public static readonly RoutedUICommand AddTag = new RoutedUICommand(
            "Add Tag",
            "AddTag",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand ShowTags = new RoutedUICommand(
            "Show Tags",
            "ShowTags",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Alt)
            }
            );

        public static readonly RoutedUICommand ExitWindow = new RoutedUICommand(
            "Exit Window",
            "ExitWindow",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
            );
    }
}
