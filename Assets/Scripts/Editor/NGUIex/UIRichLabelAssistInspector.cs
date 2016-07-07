using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
#if UNITY_3_5
[CustomEditor(typeof(UIRichLabelAssist))]
#else
[CustomEditor(typeof(UIRichLabelAssist), true)]
#endif
public class UIRichLabelAssistInspector : UIWidgetInspector
{
	protected override void DrawCustomProperties (){}
	
	protected override void DrawFinalProperties (){}
}

