using Box2DSharp.Common;
using Box2DSharp.Testbed.Unity.Inspection;
using System;
using System.Collections.Generic;
using UnityEngine;
using Color = Box2DSharp.Common.Color;
using Transform = Box2DSharp.Common.Transform;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace QkF.Box2D
{
    public class QkPhysicalDrawer : IDrawer
    {
        public QkUnityDrawer Drawer;

        public DrawFlag Flags { get; set; }

        public void DrawCircle(in Vector2 center, float radius, in Color color)
        {
            var lines = new List<(Vector3, Vector3)>();
            const int lineCount = 20;
            for (var i = 0; i <= lineCount; ++i) //割圆术画圆
            {
                lines.Add(
                    (
                        new UnityEngine.Vector2(
                            center.X + radius * (float)Math.Cos(2 * Mathf.PI / lineCount * i),
                            center.Y + radius * (float)Math.Sin(2 * Mathf.PI / lineCount * i)),
                        new UnityEngine.Vector2(
                            center.X + radius * (float)Math.Cos(2 * Mathf.PI / lineCount * (i + 1)),
                            center.Y + radius * (float)Math.Sin(2 * Mathf.PI / lineCount * (i + 1)))
                    ));
            }

            Drawer.PostLines(lines, color.ToUnityColor());
        }

        public void DrawPoint(in Vector2 p, float size, in Color color)
        {
            Drawer.PostPoint((p.ToUnityVector3(), size / 100, color.ToUnityColor()));
        }

        public void DrawPolygon(Span<Vector2> vertices, int vertexCount, in Color color)
        {
            var list = new List<(Vector3 begin, Vector3 end)>();

            for (var i = 0; i < vertexCount; i++)
            {
                if (i < vertexCount - 1)
                {
                    list.Add((vertices[i].ToUnityVector3(), vertices[i + 1].ToUnityVector3()));
                }
                else
                {
                    list.Add((vertices[i].ToUnityVector3(), vertices[0].ToUnityVector3()));
                }
            }

            Drawer.PostLines(list, color.ToUnityColor());
        }

        public void DrawSegment(in Vector2 p1, in Vector2 p2, in Color color)
        {
            Drawer.PostLines(
                new List<(Vector3, Vector3)> { (p1.ToUnityVector2(), p2.ToUnityVector2()) },
                color.ToUnityColor());
        }

        public void DrawSolidCircle(in Vector2 center, float radius, in Vector2 axis, in Color color)
        {
            DrawCircle(in center, radius, in color);
            var p = center + radius * axis;
            DrawSegment(center, p, color);
        }

        public void DrawSolidPolygon(Span<Vector2> vertices, int vertexCount, in Color color)
        {
            DrawPolygon(vertices, vertexCount, in color);
        }

        public void DrawTransform(in Transform xf)
        {
            const float axisScale = 0.4f;

            var p1 = xf.Position;
            var p2 = p1 + axisScale * xf.Rotation.GetXAxis();

            Drawer.PostLines(
                new List<(Vector3, Vector3)> { (p1.ToUnityVector2(), p2.ToUnityVector2()) },
                UnityEngine.Color.red);

            p2 = p1 + axisScale * xf.Rotation.GetYAxis();
            Drawer.PostLines(
                new List<(Vector3 begin, Vector3 end)> { (p1.ToUnityVector2(), p2.ToUnityVector2()) },
                UnityEngine.Color.green);
        }
    }
}