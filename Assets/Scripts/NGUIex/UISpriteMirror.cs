using UnityEngine;
using System.Collections;

public class UISpriteMirror : UIWidget {

	public enum eSourcePosition {
		Top, Bottom, Left, Right,
		TopLeft, TopRight, BottomLeft, BottomRight
	}

	[SerializeField][HideInInspector]
	private UIAtlas mAtlas;
	[SerializeField][HideInInspector]
	private string mSpriteName;
	[SerializeField][HideInInspector]
	private eSourcePosition mSourcePosition = eSourcePosition.Left;
	[SerializeField][HideInInspector]
	private bool mSliced = true;

	public override Material material {
		get {
			return mAtlas == null ? null : mAtlas.spriteMaterial;
		}
	}

	public eSourcePosition SourcePosition {
		set {
			mSourcePosition = value;
		}
	}

	public UIAtlas atlas {
		get {
			return mAtlas;
		}
		set {
			mAtlas = value;
			MarkAsChanged();
		}
	}

	public string spriteName {
		get {
			return mSpriteName;
		}
		set {
			mSpriteName = value;
			MarkAsChanged();
		}
	}
	
	public override void MakePixelPerfect() {
		UISpriteData sprite = null;
		if (mAtlas != null) {
			sprite = mAtlas.GetSprite(mSpriteName);
		}

		if (sprite != null && !mSliced) {
			int x = sprite.width;
			if (mSourcePosition == eSourcePosition.Left ||
			    mSourcePosition == eSourcePosition.TopLeft ||
			    mSourcePosition == eSourcePosition.BottomLeft) {
				x = (x - sprite.borderRight) << 1;
			} else if (mSourcePosition == eSourcePosition.Right ||
			           mSourcePosition == eSourcePosition.TopRight ||
			           mSourcePosition == eSourcePosition.BottomRight) {
				x = (x - sprite.borderLeft) << 1;
			}
			if ((x & 1) == 1) ++x;

			int y = sprite.height;
			if (mSourcePosition == eSourcePosition.Top ||
			    mSourcePosition == eSourcePosition.TopLeft ||
			    mSourcePosition == eSourcePosition.TopRight) {
				y = (y - sprite.borderBottom) << 1;
			} else if (mSourcePosition == eSourcePosition.Bottom ||
			           mSourcePosition == eSourcePosition.BottomLeft ||
			           mSourcePosition == eSourcePosition.BottomRight) {
				y = (y - sprite.borderTop) << 1;
			}
			if ((y & 1) == 1) ++y;
			
			width = x;
			height = y;
		}
		base.MakePixelPerfect();
	}

	public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols) {
		UISpriteData spriteData = mAtlas.GetSprite(mSpriteName);
		if (spriteData == null || mAtlas.texture == null) return;

		Color colF = color;
		colF.a = finalAlpha;
		Color32 col = NGUITools.ApplyPMA(colF);
		Vector4 v = drawingDimensions;

		float vl = v.x;
		float vr = v.z;
		float vm = 0.5f * (vl + vr);
		float vt = v.w;
		float vb = v.y;
		float vc = 0.5f * (vt + vb);
		float vll = vl;
		if (mSourcePosition == eSourcePosition.Right || mSourcePosition == eSourcePosition.BottomRight || mSourcePosition == eSourcePosition.TopRight) {
			vll += spriteData.borderRight;
		} else {
			vll += spriteData.borderLeft;
		}
		float vrr = vr;
		if (mSourcePosition == eSourcePosition.Left || mSourcePosition == eSourcePosition.TopLeft || mSourcePosition == eSourcePosition.BottomLeft) {
			vrr -= spriteData.borderLeft;
		} else {
			vrr -= spriteData.borderRight;
		}
		float vtt = vt;
		if (mSourcePosition == eSourcePosition.Bottom || mSourcePosition == eSourcePosition.BottomLeft || mSourcePosition == eSourcePosition.BottomRight) {
			vtt -= spriteData.borderBottom;
		} else {
			vtt -= spriteData.borderTop;
		}
		float vbb = vb;
		if (mSourcePosition == eSourcePosition.Top || mSourcePosition == eSourcePosition.TopLeft || mSourcePosition == eSourcePosition.TopRight) {
			vbb += spriteData.borderTop;
		} else {
			vbb += spriteData.borderBottom;
		}

		float tw = 1f / mAtlas.texture.width;
		float th = 1f / mAtlas.texture.height;

		float sul = spriteData.x * tw;
		float sur = (spriteData.x + spriteData.width) * tw;
		float svt = 1f - spriteData.y * th;
		float svb = 1f - (spriteData.y + spriteData.height) * th;
		float sull = (spriteData.x + spriteData.borderLeft) * tw;
		float surr = (spriteData.x + spriteData.width - spriteData.borderRight) * tw;
		float svtt = 1f - (spriteData.y + spriteData.borderTop) * th;
		float svbb = 1f - (spriteData.y + spriteData.height - spriteData.borderBottom) * th;

		switch (mSourcePosition) {
		case eSourcePosition.Left:
			if (mSliced) {
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, vb, vbb, vtt, vt, sul, sull, surr, 0f, svb, svbb, svtt, svt,
				           spriteData.borderLeft != 0, false, spriteData.borderBottom != 0, spriteData.borderTop != 0, col);
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, vb, vbb, vtt, vt, 0f, surr, sull, sul, svb, svbb, svtt, svt,
				           false, spriteData.borderLeft != 0, spriteData.borderBottom != 0, spriteData.borderTop != 0, col);
			} else {
				drawRect(verts, uvs, cols, vl, vm, vb, vt, sul, surr, svb, svt, col);
				drawRect(verts, uvs, cols, vm, vr, vb, vt, surr, sul, svb, svt, col);
			}
			break;
		case eSourcePosition.Right:
			if (mSliced) {
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, vb, vbb, vtt, vt, 0f, sull, surr, sur, svb, svbb, svtt, svt,
				           false, spriteData.borderRight != 0, spriteData.borderBottom != 0, spriteData.borderTop != 0, col);
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, vb, vbb, vtt, vt, sur, surr, sull, 0f, svb, svbb, svtt, svt,
				           spriteData.borderRight != 0, false, spriteData.borderBottom != 0, spriteData.borderTop != 0, col);
			} else {
				drawRect(verts, uvs, cols, vm, vr, vb, vt, sull, sur, svb, svt, col);
				drawRect(verts, uvs, cols, vl, vm, vb, vt, sur, sull, svb, svt, col);
			}
			break;
		case eSourcePosition.Bottom:
			if (mSliced) {
				drawSliced(verts, uvs, cols, vl, vll, vrr, vr, vb, vbb, vc, 0f, sul, sull, surr, sur, svb, svbb, svt, 0f,
				           spriteData.borderLeft != 0, spriteData.borderRight != 0, spriteData.borderBottom != 0, false, col);
				drawSliced(verts, uvs, cols, vl, vll, vrr, vr, 0f, vc, vtt, vt, sul, sull, surr, sur, 0f, svt, svbb, svb,
				           spriteData.borderLeft != 0, spriteData.borderRight != 0, false, spriteData.borderBottom != 0, col);
			} else {
				drawRect(verts, uvs, cols, vl, vr, vb, vc, sul, sur, svb, svtt, col);
				drawRect(verts, uvs, cols, vl, vr, vc, vt, sul, sur, svtt, svb, col);
			}
			break;
		case eSourcePosition.Top:
			if (mSliced) {
				drawSliced(verts, uvs, cols, vl, vll, vrr, vr, 0f, vc, vtt, vt, sul, sull, surr, sur, 0f, svbb, svtt, svt,
				           spriteData.borderLeft != 0, spriteData.borderRight != 0, false, spriteData.borderTop != 0, col);
				drawSliced(verts, uvs, cols, vl, vll, vrr, vr, vb, vbb, vc, 0f, sul, sull, surr, sur, svt, svtt, svbb, 0f,
				           spriteData.borderLeft != 0, spriteData.borderRight != 0, spriteData.borderTop != 0, false, col);
			} else {
				drawRect(verts, uvs, cols, vl, vr, vc, vt, sul, sur, svbb, svt, col);
				drawRect(verts, uvs, cols, vl, vr, vb, vc, sul, sur, svt, svbb, col);
			}
			break;
		case eSourcePosition.BottomLeft:
			if (mSliced) {
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, vb, vbb, vc, 0f, sul, sull, surr, 0f, svb, svbb, svtt, 0f,
				           spriteData.borderLeft != 0, false, spriteData.borderBottom != 0, false, col);
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, vb, vbb, vc, 0f, 0f, surr, sull, sul, svb, svbb, svtt, 0f,
				           false, spriteData.borderLeft != 0, spriteData.borderBottom != 0, false, col);
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, 0f, vc, vtt, vt, sul, sull, surr, 0f, 0f, svtt, svbb, svb,
				           spriteData.borderLeft != 0, false, false, spriteData.borderBottom != 0, col);
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, 0f, vc, vtt, vt, 0f, surr, sull, sul, 0f, svtt, svbb, svb,
				           false, spriteData.borderLeft != 0, false, spriteData.borderBottom != 0, col);
			} else {
				drawRect(verts, uvs, cols, vl, vm, vb, vc, sul, surr, svb, svtt, col);
				drawRect(verts, uvs, cols, vm, vr, vb, vc, surr, sul, svb, svtt, col);
				drawRect(verts, uvs, cols, vl, vm, vc, vt, sul, surr, svtt, svb, col);
				drawRect(verts, uvs, cols, vm, vr, vc, vt, surr, sul, svtt, svb, col);
			}
			break;
		case eSourcePosition.BottomRight:
			if (mSliced) {
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, vb, vbb, vc, 0f, 0f, sull, surr, sur, svb, svbb, svtt, 0f,
				           false, spriteData.borderRight != 0, spriteData.borderBottom != 0, false, col);
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, vb, vbb, vc, 0f, sur, surr, sull, 0f, svb, svbb, svtt, 0f,
				           spriteData.borderRight != 0, false, spriteData.borderBottom != 0, false, col);
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, 0f, vc, vtt, vt, 0f, sull, surr, sur, 0f, svtt, svbb, svb,
				           false, spriteData.borderRight != 0, false, spriteData.borderBottom != 0, col);
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, 0f, vc, vtt, vt, sur, surr, sull, 0f, 0f, svtt, svbb, svb,
				           spriteData.borderRight != 0, false, false, spriteData.borderBottom != 0, col);
			} else {
				drawRect(verts, uvs, cols, vm, vr, vb, vc, sull, sur, svb, svtt, col);
				drawRect(verts, uvs, cols, vl, vm, vb, vc, sur, sull, svb, svtt, col);
				drawRect(verts, uvs, cols, vm, vr, vc, vt, sull, sur, svtt, svb, col);
				drawRect(verts, uvs, cols, vl, vm, vc, vt, sur, sull, svtt, svb, col);
			}
			break;
		case eSourcePosition.TopLeft:
			if (mSliced) {
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, 0f, vc, vtt, vt, sul, sull, surr, 0f, 0f, svbb, svtt, svt,
				           spriteData.borderLeft != 0, false, false, spriteData.borderTop != 0, col);
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, 0f, vc, vtt, vt, 0f, surr, sull, sul, 0f, svbb, svtt, svt,
				           false, spriteData.borderLeft != 0, false, spriteData.borderTop != 0, col);
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, vb, vbb, vc, 0f, sul, sull, surr, 0f, svt, svtt, svbb, 0f,
				           spriteData.borderLeft != 0, false, spriteData.borderTop != 0, false, col);
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, vb, vbb, vc, 0f, 0f, surr, sull, sul, svt, svtt, svbb, 0f,
				           false, spriteData.borderLeft != 0, spriteData.borderTop != 0, false, col);
			} else {
				drawRect(verts, uvs, cols, vl, vm, vc, vt, sul, surr, svbb, svt, col);
				drawRect(verts, uvs, cols, vm, vr, vc, vt, surr, sul, svbb, svt, col);
				drawRect(verts, uvs, cols, vl, vm, vb, vc, sul, surr, svt, svbb, col);
				drawRect(verts, uvs, cols, vm, vr, vb, vc, surr, sul, svt, svbb, col);
			}
			break;
		case eSourcePosition.TopRight:
			if (mSliced) {
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, 0f, vc, vtt, vt, 0f, sull, surr, sur, 0f, svbb, svtt, svt,
				           false, spriteData.borderRight != 0, false, spriteData.borderTop != 0, col);
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, 0f, vc, vtt, vt, sur, surr, sull, 0f, 0f, svbb, svtt, svt,
				           spriteData.borderRight != 0, false, false, spriteData.borderTop != 0, col);
				drawSliced(verts, uvs, cols, 0f, vm, vrr, vr, vb, vbb, vc, 0f, 0f, sull, surr, sur, svt, svtt, svbb, 0f,
				           false, spriteData.borderRight != 0, spriteData.borderTop != 0, false, col);
				drawSliced(verts, uvs, cols, vl, vll, vm, 0f, vb, vbb, vc, 0f, sur, surr, sull, 0f, svt, svtt, svbb, 0f,
				           spriteData.borderRight != 0, false, spriteData.borderTop != 0, false, col);
			} else {
				drawRect(verts, uvs, cols, vm, vr, vc, vt, sull, sur, svbb, svt, col);
				drawRect(verts, uvs, cols, vl, vm, vc, vt, sur, sull, svbb, svt, col);
				drawRect(verts, uvs, cols, vm, vr, vb, vc, sull, sur, svt, svbb, col);
				drawRect(verts, uvs, cols, vl, vm, vb, vc, sur, sull, svt, svbb, col);
			}
			break;
		}
	}

	private void drawSliced(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols,
	                        float vl, float vll, float vrr, float vr, float vb, float vbb, float vtt, float vt,
	                        float uvl, float uvll, float uvrr, float uvr, float uvb, float uvbb, float uvtt, float uvt,
	                        bool left, bool right, bool bottom, bool top,
	                        Color32 col) {
		if (left) {
			if (bottom) {
				drawRect(verts, uvs, cols, vl, vll, vb, vbb, uvl, uvll, uvb, uvbb, col);
			}
			drawRect(verts, uvs, cols, vl, vll, vbb, vtt, uvl, uvll, uvbb, uvtt, col);
			if (top) {
				drawRect(verts, uvs, cols, vl, vll, vtt, vt, uvl, uvll, uvtt, uvt, col);
			}
		}
		if (bottom) {
			drawRect(verts, uvs, cols, vll, vrr, vb, vbb, uvll, uvrr, uvb, uvbb, col);
		}
		drawRect(verts, uvs, cols, vll, vrr, vbb, vtt, uvll, uvrr, uvbb, uvtt, col);
		if (top) {
			drawRect(verts, uvs, cols, vll, vrr, vtt, vt, uvll, uvrr, uvtt, uvt, col);
		}
		if (right) {
			if (bottom) {
				drawRect(verts, uvs, cols, vrr, vr, vb, vbb, uvrr, uvr, uvb, uvbb, col);
			}
			drawRect(verts, uvs, cols, vrr, vr, vbb, vtt, uvrr, uvr, uvbb, uvtt, col);
			if (top) {
				drawRect(verts, uvs, cols, vrr, vr, vtt, vt, uvrr, uvr, uvtt, uvt, col);
			}
		}
	}

	private void drawRect(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols,
	                      float vl,  float vr,  float vb,  float vt,
	                      float uvl, float uvr, float uvb, float uvt,
	                      Color32 col) {
		verts.Add(new Vector3(vl, vb, 0f));
		verts.Add(new Vector3(vl, vt, 0f));
		verts.Add(new Vector3(vr, vt, 0f));
		verts.Add(new Vector3(vr, vb, 0f));
		uvs.Add(new Vector2(uvl, uvb));
		uvs.Add(new Vector2(uvl, uvt));
		uvs.Add(new Vector2(uvr, uvt));
		uvs.Add(new Vector2(uvr, uvb));
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
	}
}
