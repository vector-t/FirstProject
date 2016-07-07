using UnityEngine;

public class UITextureMirror : UITexture
{
    public enum eSourcePosition
    {
        Top, Bottom, Left, Right,
        TopLeft, TopRight, BottomLeft, BottomRight
    }

    [SerializeField] [HideInInspector]
    private eSourcePosition mSourcePosition = eSourcePosition.Left;

    public eSourcePosition SourcePosition
    {
        set
        {
            mSourcePosition = value; 
        }
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture tex = mainTexture;

        Color colF = color;
        colF.a = finalAlpha;
        Color32 col = NGUITools.ApplyPMA(colF);
        Vector4 v = drawingDimensions;

        float vl = v.x;
        float vr = v.z;
        float vm = 0.5f * (vl + vr);
        float vt = v.y;
        float vb = v.w;
        float vc = 0.5f * (vt + vb);

        float sul = 0f;
        float sur = 1f;
        float svt = 0;
        float svb = 1;
        float sull = 0f;
        float surr = 1f;
        float svtt = 0f;
        float svbb = 1;

        switch (mSourcePosition)
        {
            case eSourcePosition.Left:
                    drawRect(verts, uvs, cols, vl, vm, vb, vt, sul, surr, svb, svt, col);
                    drawRect(verts, uvs, cols, vm, vr, vb, vt, surr, sul, svb, svt, col);
                break;
            case eSourcePosition.Right:
                    drawRect(verts, uvs, cols, vm, vr, vb, vt, sull, sur, svb, svt, col);
                    drawRect(verts, uvs, cols, vl, vm, vb, vt, sur, sull, svb, svt, col);
                break;
            case eSourcePosition.Bottom:
                    drawRect(verts, uvs, cols, vl, vr, vb, vc, sul, sur, svb, svtt, col);
                    drawRect(verts, uvs, cols, vl, vr, vc, vt, sul, sur, svtt, svb, col);
                break;
            case eSourcePosition.Top:
                    drawRect(verts, uvs, cols, vl, vr, vc, vt, sul, sur, svbb, svt, col);
                    drawRect(verts, uvs, cols, vl, vr, vb, vc, sul, sur, svt, svbb, col);
                break;
            case eSourcePosition.BottomLeft:
                    drawRect(verts, uvs, cols, vl, vm, vb, vc, sul, surr, svb, svtt, col);
                    drawRect(verts, uvs, cols, vm, vr, vb, vc, surr, sul, svb, svtt, col);
                    drawRect(verts, uvs, cols, vl, vm, vc, vt, sul, surr, svtt, svb, col);
                    drawRect(verts, uvs, cols, vm, vr, vc, vt, surr, sul, svtt, svb, col);
                break;
            case eSourcePosition.BottomRight:
                    drawRect(verts, uvs, cols, vm, vr, vb, vc, sull, sur, svb, svtt, col);
                    drawRect(verts, uvs, cols, vl, vm, vb, vc, sur, sull, svb, svtt, col);
                    drawRect(verts, uvs, cols, vm, vr, vc, vt, sull, sur, svtt, svb, col);
                    drawRect(verts, uvs, cols, vl, vm, vc, vt, sur, sull, svtt, svb, col);
                break;
            case eSourcePosition.TopLeft:
                    drawRect(verts, uvs, cols, vl, vm, vc, vt, sul, surr, svbb, svt, col);
                    drawRect(verts, uvs, cols, vm, vr, vc, vt, surr, sul, svbb, svt, col);
                    drawRect(verts, uvs, cols, vl, vm, vb, vc, sul, surr, svt, svbb, col);
                    drawRect(verts, uvs, cols, vm, vr, vb, vc, surr, sul, svt, svbb, col);
                break;
            case eSourcePosition.TopRight:
                    drawRect(verts, uvs, cols, vm, vr, vc, vt, sull, sur, svbb, svt, col);
                    drawRect(verts, uvs, cols, vl, vm, vc, vt, sur, sull, svbb, svt, col);
                    drawRect(verts, uvs, cols, vm, vr, vb, vc, sull, sur, svt, svbb, col);
                    drawRect(verts, uvs, cols, vl, vm, vb, vc, sur, sull, svt, svbb, col);
                break;
        }
    }

    private void drawRect(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols,
                          float vl, float vr, float vb, float vt,
                          float uvl, float uvr, float uvb, float uvt,
                          Color32 col)
    {
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
