using System.Collections;

public interface IDissolveMenu
{
    IEnumerator DissolveInCoroutine();
    IEnumerator DissolveOutCoroutine();
}
