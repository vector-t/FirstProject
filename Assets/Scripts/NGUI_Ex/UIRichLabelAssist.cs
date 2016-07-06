using UnityEngine;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class UIRichLabelAssist : UIWidget
{
	[SerializeField][HideInInspector] UIAtlas mAtlas;
	public UIAtlas atlas
	{
		get
		{
			return mAtlas;
		}
		set
		{
			mAtlas = value;
			this.MarkAsChanged();
		}
	}
	private int faceWidth = 80;
	public int FaceWidth
	{
		get
		{
			return faceWidth;
		}
		set
		{
			faceWidth = value;
		}
	}

	private int faceHeight = 80;
	public int FaceHeight
	{
		get
		{
			return faceHeight;
		}
		set
		{
			faceHeight = value;
		}
	}

	private List<FaceSyn> mFaces;
	public List<FaceSyn> Faces
	{
		get
		{
			return mFaces;
		}
		set
		{
			mFaces = value;

			this.MarkAsChanged();
		}
	}

	public override Material material
	{
		get
		{
			return (mAtlas != null) ? mAtlas.spriteMaterial : null;
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 offset = pivotOffset;

			float x0 = -offset.x * mWidth;
			float y0 = -offset.y * mHeight;
			float x1 = x0 + mWidth;
			float y1 = y0 + mHeight;

			Texture tex = mainTexture;
			int w = (tex != null) ? tex.width : mWidth;
			int h = (tex != null) ? tex.height : mHeight;

			if ((w & 1) != 0)
				x1 -= (1f / w) * mWidth;
			if ((h & 1) != 0)
				y1 -= (1f / h) * mHeight;

			return new Vector4(
				mDrawRegion.x == 0f ? x0 : Mathf.Lerp(x0, x1, mDrawRegion.x),
				mDrawRegion.y == 0f ? y0 : Mathf.Lerp(y0, y1, mDrawRegion.y),
				mDrawRegion.z == 1f ? x1 : Mathf.Lerp(x0, x1, mDrawRegion.z),
				mDrawRegion.w == 1f ? y1 : Mathf.Lerp(y0, y1, mDrawRegion.w));
		}
	}

	public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (mAtlas == null || mFaces == null || mFaces.Count == 0)
			return;

		Color col = mColor;
		Rect outerUV = new Rect();
		for (int i = 0, max = mFaces.Count; i < max; i++) {

			FaceSyn face = mFaces[i];
			UISpriteData spriteData = mAtlas.GetSprite(face.faceName);
			if (spriteData == null)
				continue;

			verts.Add(face.poses[0]);
			verts.Add(face.poses[1]);
			verts.Add(face.poses[2]);
			verts.Add(face.poses[3]);

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
