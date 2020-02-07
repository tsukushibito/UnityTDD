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

            var es = EventSystem.current;
            Assert.IsNotNull(es);
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

            Assert.IsTrue(vb.GetButton());
            Assert.IsFalse(vb.GetButtonDown());
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

        [UnityTest]
        public IEnumerator VirtualStickTest()
        {
            var go = new GameObject("VirtualStickTest");

            yield return null;

            var vgObj = GameObject.Find("VirtualGamepad");
            Assert.IsNotNull(vgObj);
            var vg = vgObj.GetComponent<VirtualGamepad>();
            Assert.IsNotNull(vg);
            var vs = vgObj.GetComponentInChildren<VirtualStick>();
            Assert.IsNotNull(vs);

            var es = EventSystem.current;
            Assert.IsNotNull(es);
            var data = new PointerEventData(es);

            yield return null;

            var h = vs.GetHolizontal();
            Assert.AreEqual(0.0f, h);

            var v = vs.GetVertical();
            Assert.AreEqual(0.0f, v);

            yield return null;
        }
    }
}
