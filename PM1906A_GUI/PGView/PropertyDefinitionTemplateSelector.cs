using PM1906AHelper.Core;
using System.Windows;
using System.Windows.Controls;

namespace PM1906A_GUI.PGView
{
    public class PropertyDefinitionTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Property propDef = (Property)item;
            FrameworkElement element = (FrameworkElement)container;

            if (!string.IsNullOrEmpty(propDef.CollectionName))
            {
                return element.TryFindResource("CollectionTemplate") as DataTemplate;
            }
            return element.TryFindResource("PropertyTemplate") as DataTemplate;
        }
    }
}
