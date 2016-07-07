using UnityEngine;
using System.Collections.Generic;

public class AssistData
{
	public int index;
	public string spriteName;
	public Vector3 pos1;
	public Vector3 pos2;
	public Vector3 pos3;
	public Vector3 pos4;
}

[ExecuteInEditMode]
public class UIRichLabelAssist : UIWidget
{
	private UIAtlas mAtlas;
	
	private List<AssistData> mAssistDataList = null;
	override public int minWidth { get { return 0; } }
	override public int minHeight { get { return 0; } }
	
	public override Material material
	{
		get { return (mAtlas != null) ? mAtlas.spriteMaterial : null; }
	}
	
	public List<AssistData> assistDataList 
	{
		get { return mAssistDataList; }
		set
		{
			mAssistDataList = value;
			this.MarkAsChanged();
		}
	}
	
	public UIAtlas atlas
	{
		get { return mAtlas; }
		set
		{
			mAtlas = value;
			this.MarkAsChanged();
		}
	}

	protected override void OnStart ()
	{
		#if UNITY_EDITOR
		if (GetComponent<UIPanel>() != null)
		{
			Debug.LogError("Widgets and panels should not be on the same object! Widget must be a child of the panel.", this);
		}
		else if (!Application.isPlaying && GetComponents<UIWidget>().Length > 1)
		{
			//Debug.LogError("You should not place more than one widget on the same object. Weird stuff will happen!", this);
		}
		#endif
		CreatePanel();
	}
	
	public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (mAtlas == null || assistDataList == null || assistDataList.Count == 0)
			return;
		
		Color col = mColor;
		Rect outerUV = new Rect();
		for (int i = 0, max = assistDataList.Count; i < max; i++) {
			
			AssistData face = assistDataList[i];
			UISpriteData spriteData = mAtlas.GetSprite(face.spriteName);
			if (spriteData == null)
				continue;
			
			verts.Add(face.pos1);
			verts.Add(face.pos2);
			verts.Add(face.pos3);
			verts.Add(face.pos4);
			
			outerUV.Set(spriteData.x, spriteData.y, spriteData.width, spriteData.height);
			outerUV = NGUIMath.ConvertToTexCoords(outerUV, mAtlas.texture.width, mAtlas.texture.height);
			
			Vector2 uv0 = new Vector2(outerUV.xMin, outerUV.yMin);
			Vector2 uv1 = new Vector2(outerUV.xMax, outerUV.yMax);
			
			uvs.Add(uv0);
			uvs.Add(new Vector2(uv0.x, uv1.y));
			uvs.Add(uv1);
			uvs.Add(new Vector2(uv1.x, uv0.y));
			
			cols.Add(col);
			cols.Add(col);
			cols.Add(col);
			cols.Add(col);
		}
	}
}