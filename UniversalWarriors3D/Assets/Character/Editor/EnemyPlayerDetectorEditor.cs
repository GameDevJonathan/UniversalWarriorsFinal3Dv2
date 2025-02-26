using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyPlayerDetector))]
public class EnemyPlayerDetectorEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyPlayerDetector _EPD = (EnemyPlayerDetector)target;

        Color c = Color.green;

        Color erc = Color.cyan;

        Color arc = Color.magenta;

        if (_EPD.alertStage == AlertStage.Intrigued)
            c = Color.Lerp(Color.green, Color.red, _EPD.alertLevel / 100f);
        else if (_EPD.alertStage == AlertStage.Alerted)
            c = Color.red;

        Handles.color = new Color(c.r, c.g, c.b, 0.3f);
        Handles.DrawSolidArc(
            _EPD.transform.position,
            _EPD.transform.up,
            Quaternion.AngleAxis(-_EPD.fovAngle / 2f, _EPD.transform.up) * _EPD.transform.forward,
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

        erc.a = .3f;

        Handles.color = erc;

        Handles.DrawSolidDisc(_EPD.transform.position, _EPD.transform.up, _EPD.engageRange);

        Handles.color = Color.blue;
        _EPD.engageRange = Handles.ScaleValueHandle(
            _EPD.engageRange,
            _EPD.transform.position + _EPD.transform.forward * _EPD.engageRange,
            _EPD.transform.rotation,
            3,
            Handles.SphereHandleCap,
            1);

        arc.a = .3f;

        Handles.color = arc;

        Handles.DrawSolidDisc(_EPD.transform.position, _EPD.transform.up, _EPD.attackRange);

        Handles.color = Color.red;
        _EPD.attackRange = Handles.ScaleValueHandle(
            _EPD.attackRange,
            _EPD.transform.position + _EPD.transform.forward * _EPD.attackRange,
            _EPD.transform.rotation,
            3,
            Handles.SphereHandleCap,
            1);
    }
}
