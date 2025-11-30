package com.example.gungame.controller;

import com.example.gungame.model.Gun;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;

@RestController
@RequestMapping("/api")
@CrossOrigin(origins = "*")
public class GunController {

    private static List<Gun> gunList = new ArrayList<>();

    public GunController() {
    
        gunList.add(new Gun(UUID.randomUUID().toString(), "AK47", 1, 30));
        gunList.add(new Gun(UUID.randomUUID().toString(), "M4A1", 5, 120));
    }

    @GetMapping("/guns")
    public Map<String, List<Gun>> getAllGuns() {
        Map<String, List<Gun>> response = new HashMap<>();
        response.put("guns", gunList);
        return response;
    }

    @PostMapping("/guns")
    public Gun addGun(@RequestBody Gun newGun) {
        newGun.setId(UUID.randomUUID().toString());
        gunList.add(newGun);
        return newGun;
    }

    @PutMapping("/guns/{id}")
    public Gun updateGun(@PathVariable String id, @RequestBody Gun updatedData) {
        for (Gun gun : gunList) {
            if (gun.getId().equals(id)) {
                gun.setGunName(updatedData.getGunName());
                gun.setLevel(updatedData.getLevel());
                gun.setDamage(updatedData.getDamage());
                return gun;
            }
        }
        return null; 
    }

    // Xóa theo ID
    @DeleteMapping("/guns/{id}")
    public String deleteGun(@PathVariable String id) {
        boolean isRemoved = gunList.removeIf(gun -> gun.getId().equals(id));
        if (isRemoved) return "Đã xóa súng có ID: " + id;
        return "Không tìm thấy ID: " + id;
    }
}