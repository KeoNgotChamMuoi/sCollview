package com.example.gungame.model;

public class Gun {
    private String id;
    private String gunName;
    private int level;
    private int damage;

    public Gun() {
    }

    public Gun(String id, String gunName, int level, int damage) {
        this.id = id;
        this.gunName = gunName;
        this.level = level;
        this.damage = damage;
    }

    // Getter & Setter
    public String getId() { return id; }
    public void setId(String id) { this.id = id; }

    public String getGunName() { return gunName; }
    public void setGunName(String gunName) { this.gunName = gunName; }

    public int getLevel() { return level; }
    public void setLevel(int level) { this.level = level; }
    
    public int getDamage() { return damage; }
    public void setDamage(int damage) { this.damage = damage; }
}