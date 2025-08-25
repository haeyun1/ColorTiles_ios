using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDetector : MonoBehaviour
{
    [SerializeField] public Grid targetGrid;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float detectionZDepth = -1f;
    [SerializeField] private GameObject mark;
    [SerializeField] float markLifetime = 0.1f;
    private readonly Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    WaitForSeconds markWait;

    void Awake()
    {
        markWait = new WaitForSeconds(markLifetime);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.instance.isFinish == false)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z + detectionZDepth);
            Vector3 worldClickPosition = mainCamera.ScreenToWorldPoint(mouseScreenPos);

            // 클릭된 월드 좌표를 그리드 셀 좌표로 변환
            Vector3Int clickedCell = targetGrid.WorldToCell(new Vector3(worldClickPosition.x, worldClickPosition.y, detectionZDepth));
            // 클릭된 셀의 월드 중심 좌표 (마크 경로의 시작점)
            Vector3 clickCellCenterWorld = targetGrid.GetCellCenterWorld(clickedCell);
            Vector2 origin = new(clickCellCenterWorld.x, clickCellCenterWorld.y); // 레이캐스트 원점

            if (Physics2D.OverlapPoint(origin) != null)
            {
                // Debug.Log("클릭한 칸이 비어있지 않습니다.");
                return;
            }

            List<GameObject> hitObjects = new List<GameObject>();

            foreach (Vector2 dir in directions)
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, dir);
                if (hit.collider != null)
                {
                    hitObjects.Add(hit.collider.gameObject);
                }
            }

            // 4개 중 아무것도 없으면 종료 (매칭될 타일이 2개 이상 필요)
            if (hitObjects.Count < 2)
            {
                // Debug.Log("타일이 하나 이하입니다. 매치되는 타일이 없습니다.");
                return;
            }

            // tag 기준으로 그룹화 (tag가 같으면 같은 종류)
            Dictionary<string, List<GameObject>> groups = new();
            foreach (GameObject obj in hitObjects)
            {
                if (!groups.ContainsKey(obj.tag))
                    groups[obj.tag] = new List<GameObject>();
                groups[obj.tag].Add(obj);
            }

            // 두 개 이상 모인 그룹 찾아서 처리
            int destroyedCount = 0;
            List<GameObject> objectsToDestroy = new();

            foreach (var pair in groups)
            {
                if (pair.Value.Count >= 2) // 같은 종류의 타일이 2개 이상 발견되면
                {
                    foreach (GameObject obj in pair.Value)
                    {
                        Vector3 objCellCenterWorld = targetGrid.GetCellCenterWorld(targetGrid.WorldToCell(obj.transform.position));

                        StartCoroutine(MakeMarkGridPath(clickCellCenterWorld, objCellCenterWorld));
                        objectsToDestroy.Add(obj); // 파괴할 오브젝트 리스트에 추가
                        destroyedCount++;
                    }
                }
            }

            // 매치되는 타일이 없었을 경우
            if (destroyedCount == 0)
            {
                // Debug.Log("매치되는 타일이 존재하지 않습니다.");
                GameManager.instance.SetTime(-10f);
                return;
            }
            else
            {
                foreach (GameObject obj in objectsToDestroy)
                {
                    if (obj != null)
                    {
                        obj.GetComponent<Collider2D>().enabled = false;
                        Destroy(obj, markLifetime);
                    }
                }
                GameManager.instance.AddScore(destroyedCount);
                // Debug.Log($"점수 +{destroyedCount}");
            }
        }
    }

    IEnumerator MakeMarkGridPath(Vector3 startWorldPos, Vector3 endWorldPos)
    {
        Vector3Int startCell = targetGrid.WorldToCell(startWorldPos);
        Vector3Int endCell = targetGrid.WorldToCell(endWorldPos);

        Vector3Int currentCell = startCell;

        bool isHorizontal = startCell.y == endCell.y;

        while (currentCell != endCell)
        {
            // 마크 생성
            Vector3 cellWorldPos = targetGrid.GetCellCenterWorld(currentCell);
            cellWorldPos.z = detectionZDepth;
            GameObject m = Instantiate(mark, cellWorldPos, Quaternion.identity, transform);
            Destroy(m, markLifetime);

            // 다음 셀로 이동
            if (isHorizontal)
                currentCell.x += (endCell.x > currentCell.x) ? 1 : -1;
            else
                currentCell.y += (endCell.y > currentCell.y) ? 1 : -1;
        }

        // 마크가 사라지는 시간 기다리기
        yield return markWait;
    }

}