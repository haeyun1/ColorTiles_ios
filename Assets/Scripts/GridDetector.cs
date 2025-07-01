using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDetector : MonoBehaviour
{
    [SerializeField] private Grid targetGrid;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float detectionZDepth = -1f;
    [SerializeField] private float raycastDistance = 23f;
    [SerializeField] private LayerMask targetLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mainCamera != null && GameManager.Instance.isFinish == false)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z + detectionZDepth);
            Vector3 worldClickPosition = mainCamera.ScreenToWorldPoint(mouseScreenPos);

            Vector3Int clickedCell = targetGrid.WorldToCell(new Vector3(worldClickPosition.x, worldClickPosition.y, detectionZDepth));
            Vector3 cellCenterWorld = targetGrid.GetCellCenterWorld(clickedCell);
            Vector2 origin = new Vector2(cellCenterWorld.x, cellCenterWorld.y);

            // 먼저 클릭한 곳에 오브젝트가 있는지 확인
            Collider2D centerCollider = Physics2D.OverlapPoint(origin, targetLayer);
            if (centerCollider != null)
            {
                Debug.Log("클릭한 칸이 비어있지 않습니다.");
                return;
            }

            // 상하좌우 탐색
            Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            List<GameObject> hitObjects = new List<GameObject>();

            foreach (Vector2 dir in directions)
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, dir, raycastDistance, targetLayer);
                if (hit.collider != null)
                {
                    hitObjects.Add(hit.collider.gameObject);
                }
            }

            // 4개 중 아무것도 없으면 종료
            if (hitObjects.Count < 2) return;

            // tag 기준으로 그룹화 (tag가 같으면 같은 종류)
            Dictionary<string, List<GameObject>> groups = new Dictionary<string, List<GameObject>>();
            foreach (GameObject obj in hitObjects)
            {
                string tag = obj.tag;
                if (!groups.ContainsKey(tag))
                    groups[tag] = new List<GameObject>();
                groups[tag].Add(obj);
            }

            // 두 개 이상 모인 그룹 찾아서 제거
            int destroyedCount = 0;
            foreach (var pair in groups)
            {
                if (pair.Value.Count >= 2)
                {
                    foreach (GameObject obj in pair.Value)
                    {
                        Destroy(obj);
                        destroyedCount++;
                    }
                }
            }

            if (destroyedCount > 0)
            {
                ScoreManager.Instance.AddScore(destroyedCount);
                Debug.Log($"오브젝트 {destroyedCount}개 제거, 점수 +{destroyedCount}");
            }
        }
    }
}
