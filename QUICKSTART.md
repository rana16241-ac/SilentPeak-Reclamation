# ⚡ Quick Start Guide - Silent Peak: Reclamation

Get up and running in 10 minutes!

---

## 🚀 Fast Setup (3 Steps)

### Step 1: Clone the Repository (1 min)
```bash
git clone https://github.com/rana16241-ac/SilentPeak-Reclamation.git
cd SilentPeak-Reclamation
```

### Step 2: Open in Unity (5 min)
1. Open **Unity Hub**
2. Click **"Add"** → Select project folder
3. Open with **Unity 2021.3 LTS** or newer
4. Wait for import to complete

### Step 3: Test in Editor (2 min)
1. Open **Assets/Scenes/MainMenu.unity**
2. Click **Play** button
3. Enter name and age
4. Explore the menu!

**Done! 🎉**

---

## 📱 Build for Android (5 min)

### Quick Build
```bash
# In Unity:
1. File → Build Settings
2. Select Android
3. Click "Switch Platform" (wait)
4. Click "Build"
5. Choose save location
6. Install APK on device
```

### Test on Device
```bash
adb install SilentPeak.apk
```

---

## 🎮 What You Can Test Now

### ✅ Working Features
- Main menu navigation
- Player profile creation
- Level selection UI
- Weapon shop system
- Coin store
- Touch controls (in gameplay scene)
- Enemy spawning logic
- Mission flow

### ⚠️ Needs Assets
- 3D models (enemies, environments)
- Textures and materials
- Sound effects and music
- Animations

---

## 📂 Key Files to Know

### Core Scripts
```
Assets/Scripts/Core/
├── GameManager.cs      # Main game logic
├── DataManager.cs      # Save/load system
└── AudioManager.cs     # Sound management
```

### UI Scripts
```
Assets/Scripts/UI/
├── MainMenuController.cs       # Main menu
├── WeaponShopController.cs     # Upgrades
├── CoinStoreController.cs      # IAP
└── LevelSelectController.cs    # Level selection
```

### Gameplay Scripts
```
Assets/Scripts/Player/
└── SniperController.cs         # Player controls

Assets/Scripts/Enemy/
├── EnemyBase.cs               # Enemy logic
├── EnemyPatrol.cs             # Patrol AI
└── EnemySpawner.cs            # Spawning
```

---

## 🔧 Common Issues

### Issue: Unity won't open project
**Fix**: Install Unity 2021.3 LTS or newer

### Issue: Scripts have errors
**Fix**: Wait for Unity to finish importing all assets

### Issue: Can't build for Android
**Fix**: Install Android Build Support via Unity Hub

### Issue: Touch controls don't work in editor
**Fix**: Use mouse to simulate touch, or build to device

---

## 📚 Next Steps

1. **Read Full Documentation**
   - [SETUP_GUIDE.md](SETUP_GUIDE.md) - Detailed setup
   - [DEVELOPMENT_ROADMAP.md](DEVELOPMENT_ROADMAP.md) - Development plan
   - [PROJECT_SUMMARY.md](PROJECT_SUMMARY.md) - Complete overview

2. **Start Contributing**
   - [CONTRIBUTING.md](CONTRIBUTING.md) - Contribution guidelines
   - Check [Issues](https://github.com/rana16241-ac/SilentPeak-Reclamation/issues)
   - Join discussions

3. **Add Assets**
   - Create placeholder 3D models
   - Add temporary textures
   - Import free sound effects

---

## 💡 Pro Tips

### For Developers
- Use `Debug.Log()` to test functionality
- Check Console for errors
- Test on actual Android device, not just editor
- Use Git branches for new features

### For Artists
- Keep poly counts low (< 5000 per model)
- Use power-of-2 textures (512, 1024, 2048)
- Optimize for mobile performance
- Test on low-end devices

### For Testers
- Report bugs with reproduction steps
- Test on multiple devices
- Check performance (FPS, battery)
- Provide constructive feedback

---

## 🎯 Your First Task

### Option 1: Test Existing Code
1. Open MainMenu scene
2. Play through the UI
3. Report any bugs you find

### Option 2: Add Placeholder Assets
1. Create simple 3D cube as enemy
2. Add to Enemy prefabs
3. Test spawning in gameplay scene

### Option 3: Improve Documentation
1. Read through docs
2. Find unclear sections
3. Submit improvements

---

## 📞 Need Help?

- **Email**: rana.16241.ac@iqra.edu.pk
- **Issues**: [GitHub Issues](https://github.com/rana16241-ac/SilentPeak-Reclamation/issues)
- **Discussions**: [GitHub Discussions](https://github.com/rana16241-ac/SilentPeak-Reclamation/discussions)

---

## ✅ Checklist

- [ ] Cloned repository
- [ ] Opened in Unity
- [ ] Tested in editor
- [ ] Read documentation
- [ ] Joined discussions
- [ ] Ready to contribute!

---

**Welcome to the team! Let's build an amazing game together! 🎮🚀**
