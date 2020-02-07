using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

namespace App.Tests
{
    public class VirtualGamepadTest
    {
        [UnityTest]
        public IEnumerator VirtualButtonTest()
        {
            var go = new GameObject("VirtualButtonTest");
            var vb = go.AddComponent<App.VirtualButton>();

            yield return null;

            var esObj = GameObject.Find("EventSystem");
            var es = esObj.GetComponent<EventSystem>();
            var data = new PointerEventData(es);

            yield return null;

            Assert.IsFalse(vb.GetButton());
            Assert.IsFalse(vb.GetButtonDown());
            Assert.IsFalse(vb.GetButtonUp());

            yield return null;

            vb.OnPointerDown(data);
            Assert.IsTrue(vb.GetButton());
            Assert.IsTrue(vb.GetButtonDown());
            Assert.IsFalse(vb.GetButtonUp());

            yield return null;

            Assert.IsFalse(vb.GetButton());
            Assert.IsTrue(vb.GetButtonDown());
            Assert.IsFalse(vb.GetButtonUp());

            yield return null;

            vb.OnPointerUp(data);
            Assert.IsFalse(vb.GetButton());
            Assert.IsFalse(vb.GetButtonDown());
            Assert.IsTrue(vb.GetButtonUp());

            yield return null;

            Assert.IsFalse(vb.GetButton());
            Assert.IsFalse(vb.GetButtonDown());
            Assert.IsFalse(vb.GetButtonUp());

            yield return null;
        }
    }
}
