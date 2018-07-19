using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public class SGF_Root_Setting
    {
        private string AP = string.Empty;
        private string CA = string.Empty;
        private int FF = -1;
        private string GM = string.Empty;
        private int ST = -1;
        private int SZ_x = -1;
        private int SZ_y = -1;

        private List<string> errors_ = new List<string>();

        public void PushError(string propertyName, string error)
        {
            errors_.Add(string.Format("ERROR in [{0}]: ", propertyName) + error);
        }

        internal void FilterProperty(SGF_Property property)
        {
            if (property.Name == "FF")
            {
                SGF_Property_FF ff = new SGF_Property_FF();
                ff.BindProperty(property);
                FF = ff.Value.Number;
            }
            else if (property.Name == "CA")
            {

            }
            else if (property.Name == "AP")
            {
            }
            else if (property.Name == "GM")
            {
            }
            else if (property.Name == "ST")
            {
            }
            else if (property.Name == "SZ")
            {
            }
        }
    }
}
