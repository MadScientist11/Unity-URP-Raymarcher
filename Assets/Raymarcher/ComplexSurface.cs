using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Raymarcher;
using Unity.VisualScripting;
using UnityEngine;

public enum BlendType
{
    Union = 0,
    Subtract = 1,
    Intersect = 2,
}

[ExecuteAlways]
public class ComplexSurface : Surface
{
    public List<Surface> Children = new List<Surface>();
    public BlendType BlendType;
    public override string EquationSdf => Equation;
    public override int SurfaceTypeId { get; set; } = (int)SurfaceType.ComplexSurface;

    private string Equation;

    [ContextMenu(nameof(UpdateEquation))]
    private void UpdateEquation()
    {
        RenderSurface = true;
        foreach (Surface surface in Children)
        {
            surface.RenderSurface = false;
        }

        Equation = GenerateSDFEquation();
        SurfaceTypeId = HelperFunctions.Hash(Equation);

        RaymarcherSDFsInjector.UpdateSDFs();
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            UpdateEquation();
            transform.hasChanged = false;
        }

        foreach (Surface surface in Children)
        {
            if(surface.transform.hasChanged)
            {
                UpdateEquation();
                surface.transform.hasChanged = false;
            }
        }
    }

    private string GenerateSDFEquation()
    {
        if (Children.Count == 0)
        {
            return null;
        }

        if (Children.Count == 1)
        {
            return Children[0].EquationSdf;
        }

        string operation = BlendType switch
        {
            BlendType.Union => "opUnion",
            BlendType.Subtract => "opSubtraction",
            BlendType.Intersect => "opIntersection",
            _ => throw new ArgumentOutOfRangeException()
        };


        string resultSdf =
            $"{operation}({ConvertToChildrenEquation(Children[0])},{ConvertToChildrenEquation(Children[1])})";
        for (var i = 2; i < Children.Count; i++)
        {
            resultSdf = $"{operation}({resultSdf},{ConvertToChildrenEquation(Children[i])})";
        }

        return "return " + resultSdf + ";";
    }

    private string ConvertToChildrenEquation(Surface children)
    {
        string childrenEquation = TrimEquation(children.EquationSdf);
        childrenEquation = Regex.Replace(childrenEquation, @"\bsurfaceData.position\b", "float3" + children.Position);
        return childrenEquation;
    }

    private string TrimEquation(string equationSDF)
    {
        string equation = equationSDF.TrimEnd(';', ' ', '\t', '\n', '\r');
        equation = Regex.Replace(equation, @"^[\s\t\n\r]*return\s*", string.Empty);
        return equation;
    }

}