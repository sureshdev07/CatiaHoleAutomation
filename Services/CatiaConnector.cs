using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using INFITF;
using MECMOD;

namespace CatiaHoleAutomation.Services
{
    public class CatiaConnector
    {
        public INFITF.Application CatiaApp { get; set; }

        public PartDocument PartDoc { get;  set; }

        public Part Part { get; set; }

        public void Connect()
        {
            try
            {
                CatiaApp = (Application)Marshal.GetActiveObject("CATIA.Application");

            }
            catch (Exception ex)
            {
                throw new Exception("CATIA connection failed: " + ex.Message);
            }
        }
    }
}
