using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YamlDotNet.RepresentationModel;

namespace Assets._Scripts
{
    public static class Extensions
    {
        public static Rect GetWorldRect(this RectTransform transform)
        {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);

            var width = corners[2].x - corners[0].x;
            var height = corners[2].y - corners[0].y;

            return new Rect(corners[0].x, corners[0].y, width, height);
        }

        public static T GetInterfaceComponent<T>(this Behaviour component)
        {
            return component.GetComponents<MonoBehaviour>().OfType<T>().FirstOrDefault();
        }

        public static T GetInterfaceComponent<T>(this GameObject component)
        {
            return component.GetComponents<MonoBehaviour>().OfType<T>().FirstOrDefault();
        }

        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static float DistanceTo(this Vector3 a, Vector3 b)
        {
            return Vector3.Distance(a, b);
        }

        public static Vector3 UnitVectorTo(this Vector3 from, Vector3 to)
        {
            var diff = (to - from);

            if (diff.IsZero())
                return Vector3.zero;

            return diff.normalized;
        }

        public static float DirectionToRadians(this Vector3 from, Vector3 to)
        {
            var unit = from.UnitVectorTo(to);
            return Mathf.Atan2(unit.y, unit.x);
        }

        public static float DirectionToDegrees(this Vector3 from, Vector3 to)
        {
            return from.DirectionToRadians(to) * Mathf.Rad2Deg;
        }

        public static bool IsZero(this Vector3 vector)
        {
            return vector.sqrMagnitude < 0.001f;
        }

        public static bool IsZero(this Vector2 vector)
        {
            return vector.sqrMagnitude < 0.001f;
        }

        public static bool ContainsKey(this IDictionary<YamlNode, YamlNode> dict, string key)
        {
            return dict.ContainsKey((YamlScalarNode)key);
        }

        public static string GetScalarValue(this IDictionary<YamlNode, YamlNode> dict, string key)
        {
            return ((YamlScalarNode)dict[(YamlScalarNode)key]).Value;
        }

        public static YamlMappingNode GetMappingValue(this IDictionary<YamlNode, YamlNode> dict, string key)
        {
            return (YamlMappingNode)dict[(YamlScalarNode)key];
        }

        public static string TryGetScalarValue(this IDictionary<YamlNode, YamlNode> dict, string key)
        {
            YamlNode value;
            return !dict.TryGetValue((YamlScalarNode)key, out value) ? null : ((YamlScalarNode)value).Value;
        } 
    }
}