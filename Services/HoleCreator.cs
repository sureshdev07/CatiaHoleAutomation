using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CatiaHoleAutomation.Models;
using INFITF;
using MECMOD;
using PARTITF;

namespace CatiaHoleAutomation.Services
{
    public class HoleCreator
    {
        private readonly CatiaConnector _connector;

        public HoleCreator(CatiaConnector connector)
        {
            _connector = connector;
        }

        /// <summary>
        /// Creates holes on the specified face using data from the list.
        /// X, Y are absolute coordinates in the part's coordinate system.
        /// </summary>
        public void CreateHolesOnFace(Reference targetFace, List<HoleData> holes)
        {
            var part = _connector.Part;
            var shapeFactory = (ShapeFactory)part.ShapeFactory;

            foreach (var hole in holes)
            {
                try
                {
                    // CATIA AddNewHoleFromPoint signature: (X, Y, Depth, Face, Diameter)
                    var diameter = hole.Radius * 2; // CATIA uses diameter, not radius
                    
                    var holeFeature = shapeFactory.AddNewHoleFromPoint(
                        hole.X,        // X coordinate in mm
                        hole.Y,        // Y coordinate in mm  
                        hole.Depth,    // Hole depth in mm
                        targetFace,    // Support face reference
                        diameter       // Hole diameter in mm
                    );

                    // Set hole type
                    holeFeature.Type = GetHoleTypeCode(hole.HoleType);
                    
                    // Set anchor mode to extremity point
                    holeFeature.AnchorMode = CatHoleAnchorMode.catExtremPointHoleAnchor;
                    
                    // Set bottom type to blind (depth already set in AddNewHoleFromPoint)
                    holeFeature.BottomType = (CatHoleBottomType)0;

                    part.UpdateObject(holeFeature);
                }
                catch (Exception ex)
                {
                    var comEx = ex as COMException;
                    var errorCode = comEx != null ? $" (COM Error: 0x{comEx.ErrorCode:X})" : "";
                    throw new Exception(
                        $"Failed to create hole SlNo {hole.SlNo}: {ex.Message}{errorCode}\n" +
                        $"X={hole.X}, Y={hole.Y}, Diameter={hole.Radius * 2}mm, Depth={hole.Depth}mm\n\n" +
                        $"Note: Coordinates must be in 3D space relative to the part origin.\n" +
                        $"If the selected face is not at Z=0, holes may not appear correctly.");
                }
            }

            part.Update();
        }

        private CatHoleType GetHoleTypeCode(string holeType)
        {
            if (string.IsNullOrWhiteSpace(holeType))
            {
                return CatHoleType.catSimpleHole;
            }

            switch (holeType.Trim().ToLowerInvariant())
            {
                case "simple":
                case "straight":
                    return CatHoleType.catSimpleHole;
                case "tapered":
                    return CatHoleType.catTaperedHole;
                case "counterbored":
                case "counterbore":
                    return CatHoleType.catCounterboredHole;
                case "countersunk":
                case "countersink":
                    return CatHoleType.catCountersunkHole;
                default:
                    return CatHoleType.catSimpleHole;
            }
        }
    }
}
