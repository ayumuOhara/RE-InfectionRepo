using System.Collections;
using UnityEngine;

public interface Iinfection
{
    public bool IsInfectioning {  get; set; }

    public IEnumerator Infection(float infectionTime, float healthRate);
}
