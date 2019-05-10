using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalHandle : MonoBehaviour
{
    [System.Serializable]
    public class Element
    {
        [SerializeField] public Crystal crystal;
        [HideInInspector] public GameObject StateUI;
    }

    [SerializeField, Range(0f, 1f)] private float alpha;
    [SerializeField] private float distance = 65f;
    [SerializeField] private Color IsGetColor = Color.black;

    [SerializeField] private GameObject CrystalUI;
    [SerializeField] private GameObject CrystalUI_Image;

    [SerializeField] private Element[] Crystals;
   

    //保存されたデータと現在のデータとが正しいか
    public bool DataCheck(ref DataManager.PlanetData data)
    {
        return data.bGet.Length == Crystals.Length;
    }

    //現在のデータ分を保存する
    public void ReSetData(ref DataManager.PlanetData data)
    {
        data.bGet = new bool[Crystals.Length];
        for (int i = 0; i < Crystals.Length; i++)
            data.bGet[i] = Crystals[i].crystal.IsGet;
    }

    //データを適応してクリスタルを設定する
    public void Set(ref DataManager.PlanetData data)
    {
        //クリスタルのステータス設定
        for (int i = 0; i < Crystals.Length; i++)
        {
            Crystals[i].crystal.IsGet = data.bGet[i];
            Crystals[i].crystal.handle = this.GetComponent<CrystalHandle>();
        }

        for (int i = 0; i < Crystals.Length; i++)
        {
            Crystals[i].StateUI = Instantiate<GameObject>(CrystalUI_Image, CrystalUI.transform, false);
            Crystals[i].StateUI.transform.position += Crystals[i].StateUI.transform.right * (i * distance);
            Crystals[i].StateUI.SetActive(true);

            //既に取得済み
            if (!Crystals[i].crystal.IsGet) continue;

            Color color = Crystals[i].crystal.gameObject.GetComponent<Renderer>().material.color;
            Crystals[i].crystal.gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, alpha);

            Color UIColor = Crystals[i].StateUI.GetComponent<Image>().color;
            Crystals[i].StateUI.GetComponent<Image>().color = IsGetColor;
        }
    }

    //クリスタルと当たり判定をした
    public void HitCrystal(GameObject hit)
    {
        foreach(Element element in Crystals)
        {
            if (element.crystal.gameObject != hit) continue;

            Color color = element.crystal.gameObject.GetComponent<Renderer>().material.color;
            element.crystal.gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, alpha);

            Color UIColor = element.StateUI.GetComponent<Image>().color;
            element.StateUI.GetComponent<Image>().color = IsGetColor;
        }
    }
}
