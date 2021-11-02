using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RunUX
{
    public class ResultPresenterTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CalculatorTemplateSelector
        {
            get;
            set;
        }

        public DataTemplate FileTemplateSelector
        {
            get;
            set;
        }

        public DataTemplate AppTemplateSelector
        {
            get;
            set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            SearchResult selectedResult = item as SearchResult;
            if (selectedResult != null)
            {
     
                if (selectedResult.Type == SearchResultType.Files)
                {
                    return this.FileTemplateSelector;
                }
                else if (selectedResult.Type == SearchResultType.Calculator)
                {
                    return this.CalculatorTemplateSelector;
                }
                else
                {
                    return this.AppTemplateSelector;
                }
            }
            return this.AppTemplateSelector;
        }
    }
}
