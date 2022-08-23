using System.Collections.Generic;
using System.Linq;
using Elements.Annotations;
using Elements.Geometry;
using Elements.Geometry.Solids;

namespace Elements
{
    public class Stack : GeometricElement
    {
        public List<double> Heights { get; set; }

        private Profile Profile => Polygon.Star(3, 1.5, 5);
        public override void UpdateRepresentations()
        {
            var ops = new List<SolidOperation>();
            var currHeight = 0.0;
            foreach (var h in Heights)
            {
                ops.Add(new Extrude(Profile.Transformed(new Transform(0, 0, currHeight + 0.05)), h - 0.1, Vector3.ZAxis, false));
                currHeight += h;
            }
            Representation = new Representation(ops);
        }

        public List<AlignedDimension> CreateDimensions()
        {
            var dims = new List<AlignedDimension>();
            var currHeight = 0.0;
            var dimLoc = Transform.OfPoint(Profile.Perimeter.Vertices.First());
            for (int i = 0; i < Heights.Count; i++)
            {
                double h = Heights[i];
                dims.Add(new AlignedDimension(dimLoc + (0, 0, currHeight), dimLoc + (0, 0, currHeight + h), 0.3)
                {
                    LinkedProperty = new LinkedPropertyInfo
                    {
                        ElementId = Id,
                        PropertyName = nameof(Heights),
                        OverrideName = "Array Edit",
                        VisibleOnlyOnSelection = true,
                        Index = i
                    }
                });
                currHeight += h;
            }
            return dims;
        }
    }
}