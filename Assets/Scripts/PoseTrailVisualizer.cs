using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RosMessageTypes.Geometry;                     // Generated message classes
using Unity.Robotics.Visualizations;                // Visualizations
using Unity.Robotics.ROSTCPConnector.ROSGeometry;   // Coordinate space utilities

public class PoseTrailVisualizer : HistoryDrawingVisualizer<PoseStampedMsg>
{
    [SerializeField]
    Color m_Color = Color.white;
    [SerializeField]
    float m_Thickness = 0.1f;
    [SerializeField]
    string m_Label = "";

    public override Action CreateGUI(IEnumerable<Tuple<PoseStampedMsg, MessageMetadata>> messages)
    {
        return () =>
        {
            int count = 0;
            foreach (var (message, meta) in messages)
            {
                message.pose.GUI($"Goal #{count}");
                count++;
            }
        };
    }

    public override void Draw(Drawing3d drawing, IEnumerable<Tuple<PoseStampedMsg, MessageMetadata>> messages)
    {
        var firstPass = true;
        var prevPoint = Vector3.zero;
        var color = Color.white;
        var label = "";

        foreach (var (msg, meta) in messages)
        {
            var point = msg.pose.position.From<FLU>();
            if (firstPass)
            {
                color = VisualizationUtils.SelectColor(m_Color, meta);
                label = VisualizationUtils.SelectLabel(m_Label, meta);
                firstPass = false;
            }
            else
            {
                drawing.DrawLine(prevPoint, point, color, m_Thickness);
            }

            prevPoint = point;
        }

        drawing.DrawLabel(label, prevPoint, color);
    }
}
