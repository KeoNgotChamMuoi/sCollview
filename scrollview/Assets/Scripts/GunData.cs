using System;

[Serializable]
public class GunData
{
    public string id;
    public string gunName;
    public int level;
    public int damage;
    public GunData() { }

    public GunData(GunData source)
    {
        this.id = source.id;
        this.gunName = source.gunName;
        this.level = source.level;
        this.damage = source.damage;
    }
}


[Serializable]
public class GunListWrapper
{
    public GunData[] guns;
}