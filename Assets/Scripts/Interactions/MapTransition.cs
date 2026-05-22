using Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapTransition : MonoBehaviour
{

    [SerializeField] PolygonCollider2D mapBoundry;
    CinemachineConfiner confiner;
    [SerializeField] Direction direction;
    [SerializeField] Transform teleportTargetPos;
    [SerializeField] float additivePos;
    [SerializeField] AudioClip areaMusic;

    [SerializeField] Room currRoom;

    enum Direction {Up, Down, Left, Right, Teleport}

    private void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner>();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FadeTransition(collision.gameObject);

            currRoom?.ResetEnemies();

            MapController_Manual.Instance?.HighlightArea(mapBoundry.name);
            MapController_Dynamic.Instance?.UpdateCurrentArea(mapBoundry.name);
            BgMusicManager.instance.PlayMusic(areaMusic);
        }
    }

    async void FadeTransition(GameObject player)
    {
        PauseController.SetPause(true);

        await ScreenFader.Instance.FadeOut();
        confiner.m_BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        await ScreenFader.Instance.FadeIn();

        PauseController.SetPause(false);
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        if (direction.Equals(Direction.Teleport)) {
            player.transform.position = teleportTargetPos.position;
            return;
        }


        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += additivePos;
                break;
            case Direction.Down:
                newPos.y -= additivePos;
                break;
            case Direction.Left:
                newPos.x += additivePos;
                break;
            case Direction.Right:
                newPos.y -= additivePos;
                break;

        }

    player.transform.position = newPos;
    }


    void ResetEnemies()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            enemy.ResetPosition();
        }

    }

}
