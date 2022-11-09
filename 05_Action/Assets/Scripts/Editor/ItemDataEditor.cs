using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

// ItemData용 커스텀 에디터를 작성한다는 표시
[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    // 선택된 ItemData를 저장할 변수
    ItemData itemData;

    // 선택한다 = 활성화된다
    private void OnEnable()
    {
        // target은 에디터에서 선택한 오브젝트
        // target은 ItemData로 캐스팅 시도. 성공하면 null이 아닌 참조값. 실패하면 null
        itemData = target as ItemData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();      // 원래 인스펙터 창에서 그려지는 것들

        if (itemData != null)       // itemData가 있는지 확인
        {
            if (itemData.itemIcon != null)
            {
                Texture2D texture;
                EditorGUILayout.LabelField("Item Icon Preview");                    // 제목 출력
                texture = AssetPreview.GetAssetPreview(itemData.itemIcon);          // itemData.itemIcon에서 texture가져오기
                if (texture != null)
                {
                    GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64)); // 64 * 64 크기의 영역을 잡고
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       // 위에 잡는 영역에 텍스쳐를 그린다.
                }
            }
        }
    }
}
#endif