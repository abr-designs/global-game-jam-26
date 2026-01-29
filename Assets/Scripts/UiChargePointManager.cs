using Prototype.Alex.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiChargePointManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float tickInterval = 1f;

    [Header("Indicator Pool")]
    [SerializeField] private Transform container;
    [SerializeField] private int indicatorCount = 3;
    [SerializeField] private UiChargePointIndicatorOverlayCanvas indicatorPrefab;

    [Header("Colors")]
    [SerializeField] private Color regularColor = Color.green;
    [SerializeField] private Color superColor = new Color(1f, 0.84f, 0f);

    private readonly List<ChargeWaypoint> chargePoints = new();
    private readonly List<UiChargePointIndicatorOverlayCanvas> indicators = new();

    private void Awake()
    {
        chargePoints.AddRange(FindObjectsByType<ChargeWaypoint>(FindObjectsSortMode.None));

        for (int i = 0; i < indicatorCount; i++)
        {
            var indicator = Instantiate(indicatorPrefab, container);
            indicator.Unassign();
            indicators.Add(indicator);
        }
    }

    private void Start()
    {
        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        var wait = new WaitForSeconds(tickInterval);

        while (true)
        {
            UpdateAssignments();
            yield return wait;
        }
    }

    private void UpdateAssignments()
    {
        // Build sorted list of valid charge points
        List<ChargeWaypoint> valid = new();

        foreach (var cp in chargePoints)
        {
            if (!cp.Activated)
                valid.Add(cp);
        }

        valid.Sort((a, b) =>
        {
            float da = Vector3.SqrMagnitude(a.transform.position - player.position);
            float db = Vector3.SqrMagnitude(b.transform.position - player.position);
            return da.CompareTo(db);
        });

        // Assign indicators
        for (int i = 0; i < indicators.Count; i++)
        {
            if (i >= valid.Count)
            {
                indicators[i].Unassign();
                continue;
            }

            var cp = valid[i];
            var color = cp.ChargePointType == CHARGE_POINT_TYPE.SUPER ? superColor : regularColor;

            indicators[i].Assign(
                cp.transform,
                color
            );
        }
    }
}
