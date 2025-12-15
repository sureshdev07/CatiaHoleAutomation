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

        public void OpenPart(string partFilePath)
        {
            var doc = CatiaApp.Documents.Open(partFilePath);
            PartDoc = (PartDocument)doc;
            Part = PartDoc.Part;
        }

        public Part GetActivePart()
        {
            if (CatiaApp.ActiveDocument == null)
            {
                throw new Exception("No active document in CATIA.");
            }

            PartDoc = (PartDocument)CatiaApp.ActiveDocument;
            Part = PartDoc.Part;
            return Part;
        }

        /// <summary>
        /// Prompts the user to select a planar face in CATIA.
        /// Returns the selected face as a Reference object.
        /// </summary>
        public INFITF.Reference PromptUserToSelectFace()
        {
            var selection = PartDoc.Selection;
            selection.Clear();

            var status = selection.SelectElement2(
                new object[] { "Face" },
                "Select a planar face for hole placement",
                false);

            if (status != "Normal")
            {
                throw new Exception("Face selection cancelled or failed.");
            }

            if (selection.Count == 0)
            {
                throw new Exception("No face selected.");
            }

            var selectedFace = (INFITF.Reference)selection.Item(1).Value;
            selection.Clear();

            return selectedFace;
        }
    }
}
