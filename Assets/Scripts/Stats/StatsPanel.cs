using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Omnis.Playdough
{
    public class StatsPanel : MonoBehaviour
    {
        #region Serialized fields
        [SerializeField] protected Text perfect;
        [SerializeField] protected Text qualified;
        [SerializeField] protected Text unqualified;
        [SerializeField] protected Text tooMuch;
        [SerializeField] protected Text miss;
        [SerializeField] protected Text total;
        [SerializeField] protected Text accuracy;
        [SerializeField] protected Text avgTime;
        [SerializeField] protected Text playedTime;
        #endregion

        #region Functions
        public void ShowPanel()
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(IShowPanel());
        }
        private IEnumerator IShowPanel()
        {
            transform.localScale = new(1f, 0f, 1f);
            var scaleY = 0f;
            while (scaleY < 1f)
            {
                transform.localScale = new(1f, scaleY, 1f);
                scaleY += 10f * Time.deltaTime;
                yield return 0;
            }
            transform.localScale = Vector3.one;
        }
        public void HidePanel()
        {
            StopAllCoroutines();
            StartCoroutine(IHidePanel());
        }
        private IEnumerator IHidePanel()
        {
            transform.localScale = Vector3.one;
            var scaleY = 1f;
            while (scaleY > 0f)
            {
                transform.localScale = new(1f, scaleY, 1f);
                scaleY -= 10f * Time.deltaTime;
                yield return 0;
            }
            transform.localScale = new(1f, 0f, 1f);
            gameObject.SetActive(false);
        }
        #endregion
    }
}
