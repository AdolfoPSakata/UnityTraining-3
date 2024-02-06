using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHUDController : MonoBehaviour
{
    [Header("UI Images")]
    [SerializeField] private Image mainWeapon;
    [SerializeField] private Image secondaryWeapon;
    [SerializeField] private Image cooldownFiller;
    [SerializeField] private Image healthBar;

    [Header("UI Texts")]
    [SerializeField] private TMP_Text boltsAmount;
    [SerializeField] private TMP_Text ammoAmount;
    [SerializeField] private TMP_Text killAmount;

    private EventSubscriber OnHealthChange;
    private EventSubscriber onBoltChange;
    private EventSubscriber onAmmoChange;
    private EventSubscriber onKillChange;
    private EventSubscriber onShot;

    private Coroutine cooldown;
    private void Start()
    {
        OnHealthChange = new EventSubscriber(EventBus.healthChangeEvent, UpdateHealth);
        onBoltChange = new EventSubscriber(EventBus.boltChangeEvent, UpdateBolts);
        onAmmoChange = new EventSubscriber(EventBus.ammoChangeEvent, UpdateAmmo);
        onKillChange = new EventSubscriber(EventBus.killChangeEvent, UpdateKills);
        onShot = new EventSubscriber(EventBus.startCooldownEvent, StartCooldown);
    }

    private void UpdateHealth(EventArgs args)
    {
        UpdateUI(healthBar, args.Value);
    }
    private void UpdateBolts(EventArgs args)
    {
        Debug.Log(args.Value);
        UpdateUI(boltsAmount, args.Value);
    }
    private void UpdateAmmo(EventArgs args)
    {
        UpdateUI(ammoAmount, args.Value);
    }
    private void UpdateKills(EventArgs args)
    {
        UpdateUI(killAmount, args.Value);
    }

    private void UpdateUI(TMP_Text component, float value)
    {
        string text = value.ToString();
        component.text = text;
    }

    private void UpdateUI(Image image, float value)
    {
        image.fillAmount = value;
    }
    private void StartCooldown(EventArgs args)
    {
        if (cooldown != null)
            cooldown = null;
        cooldown = StartCoroutine(ShowCooldown(args));
    }

    private IEnumerator ShowCooldown(EventArgs args)
    {
        cooldownFiller.fillAmount = 0;
        float cooldownTime = args.Value;
        float fillValue = 0;
        const float FILL_PERCENT = 1f;

        while (FILL_PERCENT > fillValue)
        {
            yield return new WaitForEndOfFrame();

            fillValue += Time.deltaTime;
            cooldownFiller.fillAmount = fillValue / cooldownTime;
        }
        cooldownFiller.fillAmount = 0;
    }
}
