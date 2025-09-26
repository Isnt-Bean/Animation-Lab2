using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    [SerializeField] PathManager pm;

    [SerializeField] private List<Waypoints> thePath;
    private List<int> toDelete;
    Waypoints selectedPoint = null;
    bool doRepaint = true;

    private void OnScreenGUI()
    {
        thePath = pm.getPath();
        DrawPath(thePath);
    }

    private void OnEnable()
    {
        pm = target as PathManager;
        toDelete = new List<int>();
    }

    override public void OnInspectorGUI()
    {
        this.serializedObject.Update();
        thePath = pm.getPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        DrawGUIForPoints();

        if (GUILayout.Button("Add Point to Path"))
        {
            pm.CreateAddPoint();
        }
        
        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    void DrawGUIForPoints()
    {
        if (thePath != null && thePath.Count > 0)
        {
            for (int i = 0; i < thePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                Waypoints p = thePath[i];
                
                Vector3 oldPos = p.GetPos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);

                if (EditorGUI.EndChangeCheck()) p.SetPos(newPos);

                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        if (toDelete.Count > 0)
        {
            foreach (int i in toDelete)
                thePath.RemoveAt(i);
            toDelete.Clear();
        }

        
    }
    void DrawPath(List<Waypoints> path)
    {
        if (path != null)
        {
            int current = 0;
            foreach (Waypoints wp in path)
            {
                doRepaint = DrawPoint(wp);
                int next = (current + 1) % thePath.Count;
                Waypoints wpnext = path[next];
                    
                DrawPathLine(wp, wpnext);

                current += 1;
            }
        }

        if (doRepaint) Repaint();
    }

    public void DrawPathLine(Waypoints p1, Waypoints p2)
    {
        Color c = Handles.color;
        Handles.color = Color.gray;
        Handles.DrawLine(p1.GetPos(), p2.GetPos());
        Handles.color = c;
    }

    public bool DrawPoint(Waypoints p)
    {
        bool isChanged = false;
        if (selectedPoint == p)
        {
            Color c = Handles.color;
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Vector3 oldPos = p.GetPos();
            Vector3 newPos = Handles.PositionHandle(oldPos, Quaternion.identity);
            
            float handleSize = HandleUtility.GetHandleSize(newPos);
            Handles.SphereHandleCap(-1, newPos, Quaternion.identity, 0.25f * handleSize, EventType.Repaint);
            if (EditorGUI.EndChangeCheck())
            {
                p.SetPos(newPos);
            }

            Handles.color = c;
        }
        else
        {
            Vector3 currPos = p.GetPos();
            float handleSize = HandleUtility.GetHandleSize(currPos);
                    
            if (Handles.Button(currPos, Quaternion.identity,0.25f * handleSize, 0.25f * handleSize, Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }

        }
        
        return isChanged;
    }
}
