using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR     // 실행 파일에 포함되지 않음
using UnityEditor;

// ItemData용 커스텀 에디터를 작성한다는 표시
[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    ItemData itemData;

    // 선택한다 = 활성화된다
    private void OnEnable()
    {
        // 선택된 오브젝트(target)가 'as : ItemData로 캐스팅되면 참조값 아니면 null'
        itemData = target as ItemData;
    }

    // 재대로 안하면 Inspector창이 안보일 수 있다
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();      // 원래 Inspector창에 보여지는 것들

        if(itemData != null)
        {
            if(itemData.itemIcon != null)
            {
                Texture texture;
                EditorGUILayout.LabelField("Item Icon Preview");
                texture = AssetPreview.GetAssetPreview(itemData.itemIcon);              // itemData.itemIcon에서 texture가져오기
                if(texture != null)
                {
                    GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64));     // 그릴 영역 잡기
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);           // 그 영역에 그리기
                }
            }
        }
    }
}
#endif