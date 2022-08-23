using Elements;
using Elements.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace EditableDimensionsExample
{
    public static class EditableDimensionsExample
    {
        /// <summary>
        /// The EditableDimensionsExample function.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A EditableDimensionsExampleOutputs instance containing computed results and the model with any new elements.</returns>
        public static EditableDimensionsExampleOutputs Execute(Dictionary<string, Model> inputModels, EditableDimensionsExampleInputs input)
        {
            var output = new EditableDimensionsExampleOutputs();

            var boxes = new List<BoxElement>();
            // Create elements and handle overrides normally.
            var xPosition = 0.0;
            for (int i = 0; i < 4; i++)
            {
                var name = $"Box {i}";
                var matchingOverride = input.Overrides.BoxDimensions.FirstOrDefault(d => d.Identity.Name == name);
                var width = matchingOverride?.Value.Width ?? 1.0;
                var length = matchingOverride?.Value.Length ?? 1.0;
                var height = matchingOverride?.Value.Height ?? 1.0;
                var box = new BoxElement
                {
                    Width = width,
                    Length = length,
                    Height = height,
                    Name = name,
                    Transform = new Transform(xPosition, 0, 0)
                };
                xPosition += (width + 1);
                if (matchingOverride != null)
                {
                    Identity.AddOverrideIdentity(box, matchingOverride);
                }
                boxes.Add(box);
            }

            output.Model.AddElements(boxes);
            output.Model.AddElements(boxes.SelectMany(b => b.CreateDimensions()));


            // "Stack" override:

            var matchingStackOverride = input.Overrides.ArrayEdit.FirstOrDefault(d => d.Identity.Name == "Stack");
            var heights = matchingStackOverride?.Value?.Heights?.ToList() ?? new List<double> { 4.0, 1.0, 2.0, 1.0 };
            var stack = new Stack
            {
                Transform = new Transform(0, -10, 0),
                Material = new Material { Color = Colors.Red },
                Name = "Stack",
                Heights = heights
            };
            if (matchingStackOverride != null)
            {
                Identity.AddOverrideIdentity(stack, matchingStackOverride);
            }
            output.Model.AddElement(stack);
            output.Model.AddElements(stack.CreateDimensions());

            return output;
        }
    }
}