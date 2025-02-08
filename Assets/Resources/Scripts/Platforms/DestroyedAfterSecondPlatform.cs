using System.Collections;
using UnityEngine;

public class DestroyedAfterSecondPlatform : PlatformBase
{
    public override void OnJump()
    {
        StartCoroutine(DestroyAfter());
    }

    protected override void SpecialAction()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
