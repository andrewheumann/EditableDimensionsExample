using System;
using System.Collections.Generic;
using System.Linq;
using EditableDimensionsExample;
using Elements.Annotations;
using Elements.Geometry;
using Elements.Geometry.Solids;

namespace Elements
{
    public class BoxElement : GeometricElement
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }

        public override void UpdateRepresentations()
        {
            var rect = Polygon.Rectangle((0, 0), (Width, Length));
            Representation = new Extrude(rect, Height, Vector3.ZAxis, false);
        }

        public List<AlignedDimension> CreateDimensions()
        {
            var lines = new[] {
                (name: nameof(Width), l: new Line((0,0), (Width, 0, 0)).TransformedLine(Transform)),
                (name: nameof(Length), l: new Line((Width, 0, 0), (Width, Length, 0)).TransformedLine(Transform)),
                (name: nameof(Height), l: new Line((Width, Length, 0), (Width, Length, Height)).TransformedLine(Transform))
            };

            var dims = lines.Select((line) => new AlignedDimension(
             line.l.Start, line.l.End, 0.3)
            {
                LinkedProperty = new LinkedPropertyInfo
                {
                    ElementId = Id,
                    PropertyName = line.name,
                    OverrideName = "Box Dimensions",
                    VisibleOnlyOnSelection = true
                }
            }).ToList();
            return dims;
        }

        public BoxElement Update(BoxDimensionsOverride edit)
        {
            Width = edit.Value.Width ?? Width;
            Length = edit.Value.Length ?? Length;
            Height = edit.Value.Height ?? Height;
            return this;
        }
    }
}