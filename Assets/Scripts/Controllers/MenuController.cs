using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;
    [SerializeField] TabController tabController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseController.IsDialogOpen || PauseController.IsGamePaused) return;
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
            {
                return;
            }

            SoundEffectManager.Play("OpenMenu");

            bool newState = !menuCanvas.activeSelf;
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            PauseController.SetMenuOpen(menuCanvas.activeSelf);

            if (newState)
            {
                // Re-opened menu ? reactivate current tab
                tabController.ActivateTab(tabController.activeTab);
            }
        }

    }
}
