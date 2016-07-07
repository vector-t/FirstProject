using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UISpriteMirror), true)]
public class UISpriteMirrorInspector : UIWidgetInspector {

	private UISpriteMirror mMirror;

	protected override void OnEnable () {
		base.OnEnable();
		mMirror = target as UISpriteMirror;
	}
	
	void OnSelectAtlas (Object obj) {
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("mAtlas");
		sp.objectReferenceValue = obj;
		serializedObject.ApplyModifiedProperties();
		NGUISettings.atlas = obj as UIAtlas;
	}

	void SelectSprite (string spriteName) {
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("mSpriteName");
		sp.stringValue = spriteName;
		serializedObject.ApplyModifiedProperties();
		NGUISettings.selectedSprite = spriteName;
	}

	protected override bool ShouldDrawProperties() {
		GUILayout.BeginHorizontal();
		if (NGUIEditorTools.DrawPrefixButton("Atlas"))
			ComponentSelector.Show<UIAtlas>(OnSelectAtlas);
		SerializedProperty atlas = NGUIEditorTools.DrawProperty("", serializedObject, "mAtlas", GUILayout.MinWidth(20f));
		
		if (GUILayout.Button("Edit", GUILayout.Width(40f)))
		{
			if (atlas != null)
			{
				UIAtlas atl = atlas.objectReferenceValue as UIAtlas;
				NGUISettings.atlas = atl;
				NGUIEditorTools.Select(atl.gameObject);
			}
		}
		GUILayout.EndHorizontal();
		
		SerializedProperty sp = serializedObject.FindProperty("mSpriteName");
		NGUIEditorTools.DrawAdvancedSpriteField(atlas.objectReferenceValue as UIAtlas, sp.stringValue, SelectSprite, false);

		NGUIEditorTools.DrawProperty("Source", serializedObject, "mSourcePosition");
		NGUIEditorTools.DrawProperty("Sliced", serializedObject, "mSliced");

		if (serializedObject.ApplyModifiedProperties()) mMirror.MarkAsChanged();

		return true;
	}

	public override bool HasPreviewGUI() { return true; }

	public override void OnPreviewGUI(Rect rect, GUIStyle background) {
		UISpriteMirror mirror = target as UISpriteMirror;
		if (mirror == null) return;
		
		Texture2D tex = mirror.mainTexture as Texture2D;
		if (tex == null) return;
		
		UISpriteData sd = mirror.atlas.GetSprite(mirror.spriteName);
		NGUIEditorTools.DrawSprite(tex, rect, sd, mirror.color);
	}
}
