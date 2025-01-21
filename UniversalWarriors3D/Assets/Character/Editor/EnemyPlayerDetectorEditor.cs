using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyPlayerDetector))]
public class EnemyPlayerDetectorEditor : Editor
{
    private void OnSceneGUI()
    {        
        EnemyPlayerDetector _EPD = (EnemyPlayerDetector)target;

        Color c = Color.green;

        if (_EPD.alertStage == AlertStage.Intrigued)
            c = Color.Lerp(Color.green, Color.red, _EPD.alertLevel / 100f);
        else if (_EPD.alertStage == AlertStage.Alerted)
            c = Color.red;

        Handles.color = new Color(c.r, c.g, c.b, 0.3f);
        Handles.DrawSolidArc(
            _EPD.transform.position,
            _EPD.transform.up,
            Quaternion.AngleAxis(-_EPD.fovAngle /2f, _EPD.transform.up) * _EPD.transform.forward,
            _EPD.fovAngle,
            _EPD.fov);

        Handles.color = c;
        _EPD.fov = Handles.ScaleValueHandle(
            _EPD.fov,
            _EPD.transform.position + _EPD.transform.forward * _EPD.fov,
            _EPD.transform.rotation,
            3,
            Handles.SphereHandleCap,
            1);
    }
}
