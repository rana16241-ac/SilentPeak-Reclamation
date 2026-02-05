# 🚀 Setup Guide - Silent Peak: Reclamation

## Prerequisites

### Required Software
1. **Unity Hub** (Latest version)
   - Download: https://unity.com/download

2. **Unity 2021.3 LTS or newer**
   - Install via Unity Hub
   - Include Android Build Support module
   - Include Android SDK & NDK Tools
   - Include OpenJDK

3. **Android Studio** (Optional, for advanced building)
   - Download: https://developer.android.com/studio

4. **Git** (for version control)
   - Download: https://git-scm.com/

### System Requirements
- **OS**: Windows 10/11, macOS 10.15+, or Linux
- **RAM**: 8GB minimum, 16GB recommended
- **Storage**: 10GB free space
- **Graphics**: DirectX 11 or Metal capable GPU

---

## 📥 Installation Steps

### Step 1: Clone the Repository

```bash
git clone https://github.com/rana16241-ac/SilentPeak-Reclamation.git
cd SilentPeak-Reclamation
```

### Step 2: Open in Unity

1. Open **Unity Hub**
2. Click **"Add"** button
3. Navigate to the cloned project folder
4. Select the folder and click **"Open"**
5. Unity will import all assets (this may take a few minutes)

### Step 3: Configure Project Settings

1. Go to **File → Build Settings**
2. Select **Android** platform
3. Click **"Switch Platform"** (wait for completion)
4. Click **"Player Settings"**
5. Configure the following:

#### Company & Product
- **Company Name**: Your Studio Name
- **Product Name**: Silent Peak Reclamation
- **Package Name**: com.yourstudio.silentpeak

#### Other Settings
- **Scripting Backend**: IL2CPP
- **API Compatibility Level**: .NET Standard 2.1
- **Target Architectures**: ARM64 ✓

#### Resolution and Presentation
- **Default Orientation**: Landscape Left
- **Allowed Orientations**: Landscape Left & Right only

---

## 🎮 Setting Up Scenes

### Main Menu Scene Setup

1. Open **Assets/Scenes/MainMenu.unity**
2. Create the following hierarchy:

```
Canvas (Screen Space - Overlay)
├── MainMenuPanel
│   ├── TopBar
│   │   ├── PlayerNameText
│   │   ├── CoinsText
│   │   ├── LevelMapButton
│   │   └── WatchAdButton
│   └── BottomButtons
│       ├── WeaponShopButton
│       ├── CustomizationButton
│       ├── PlayButton (Larger, centered)
│       ├── PremiumButton
│       └── CoinStoreButton
├── PlayerSetupPanel (Initially disabled)
│   ├── NameInputField
│   ├── AgeInputField
│   └── ConfirmButton
├── WeaponShopPanel (Initially disabled)
├── CustomizationPanel (Initially disabled)
├── PremiumPanel (Initially disabled)
├── CoinStorePanel (Initially disabled)
└── LevelSelectPanel (Initially disabled)

GameManager (Empty GameObject)
├── GameManager.cs
├── DataManager.cs
└── AudioManager.cs
```

3. Attach **MainMenuController.cs** to Canvas
4. Assign all UI references in the Inspector

### Gameplay Scene Setup

1. Create **Assets/Scenes/GamePlay.unity**
2. Create the following hierarchy:

```
Main Camera
├── SniperController.cs
└── Camera Target (Empty child)

Canvas (Screen Space - Overlay)
├── HUD
│   ├── EnemiesRemainingText
│   ├── CoinsText
│   ├── HeadshotsText
│   ├── MissionTimeText
│   ├── Crosshair
│   └── PauseButton
├── ScopeUI (Initially disabled)
│   ├── ScopeOverlay
│   ├── ZoomLevelText
│   └── BreathMeter
├── MissionCompletePanel (Initially disabled)
├── MissionFailedPanel (Initially disabled)
└── PausePanel (Initially disabled)

UIManager (Empty GameObject)
└── UIManager.cs

Level (Empty GameObject)
├── SniperPosition
├── BaseEnvironment
└── Enemies
    ├── Enemy1 (with EnemyBase.cs)
    ├── Enemy2 (with EnemyPatrol.cs)
    └── ...
```

3. Attach **UIManager.cs** to UIManager GameObject
4. Assign all UI references

---

## 🎨 Creating UI Elements

### Button Style
- **Size**: 200x60 pixels
- **Font**: Bold, 24pt
- **Colors**:
  - Normal: #2C3E50
  - Highlighted: #34495E
  - Pressed: #1A252F
  - Disabled: #7F8C8D

### Text Style
- **Font**: Arial or similar sans-serif
- **Color**: White (#FFFFFF)
- **Outline**: Black, 2px

### Panel Style
- **Background**: Semi-transparent black (#000000, Alpha: 180)
- **Border**: 2px white outline

---

## 🔧 Script Setup

### GameManager Setup
1. Create empty GameObject named "GameManager"
2. Add **GameManager.cs** component
3. Mark as **DontDestroyOnLoad**

### DataManager Setup
1. Add **DataManager.cs** to GameManager
2. No additional setup required (auto-initializes)

### AudioManager Setup
1. Add **AudioManager.cs** to GameManager
2. Create child GameObjects:
   - MusicSource (with AudioSource component)
   - SFXSource (with AudioSource component)
   - AmbienceSource (with AudioSource component)
3. Assign AudioSource references in Inspector

---

## 📱 Building for Android

### Method 1: Direct APK Build

1. **File → Build Settings**
2. Ensure Android is selected
3. Click **"Build"**
4. Choose save location
5. Wait for build to complete
6. Install APK on Android device

### Method 2: Export to Android Studio

1. **File → Build Settings**
2. Check **"Export Project"**
3. Click **"Export"**
4. Choose export location
5. Open exported project in Android Studio
6. Build using Gradle:
   ```bash
   ./gradlew assembleDebug
   ```

### Testing on Device

```bash
# Connect device via USB
adb devices

# Install APK
adb install path/to/SilentPeak.apk

# View logs
adb logcat -s Unity
```

---

## 🎯 Testing Checklist

### Main Menu
- [ ] Player setup appears on first launch
- [ ] Name and age validation works
- [ ] All buttons navigate correctly
- [ ] Coins display updates
- [ ] Watch ad button adds 100 coins

### Weapon Shop
- [ ] All upgrades display correctly
- [ ] Purchase system works
- [ ] Insufficient coins shows error
- [ ] Max level disables button

### Level Selection
- [ ] Locked levels are grayed out
- [ ] Level details show correctly
- [ ] Stars display properly
- [ ] Play button starts mission

### Gameplay
- [ ] Touch controls work smoothly
- [ ] Scope zoom functions
- [ ] Shooting mechanics work
- [ ] Enemy hit detection accurate
- [ ] Mission complete/fail triggers correctly
- [ ] Coins awarded properly

### Performance
- [ ] Maintains 60 FPS
- [ ] No memory leaks
- [ ] Smooth UI transitions
- [ ] Quick load times

---

## 🐛 Troubleshooting

### Issue: Unity won't open project
**Solution**: Ensure Unity version is 2021.3 LTS or newer

### Issue: Android build fails
**Solution**: 
1. Check Android SDK is installed
2. Verify JDK is configured
3. Ensure IL2CPP is selected

### Issue: Touch input not working
**Solution**: 
1. Check EventSystem exists in scene
2. Verify Canvas Raycaster is attached
3. Test on actual device, not just editor

### Issue: Scripts have errors
**Solution**:
1. Ensure all scripts are in correct folders
2. Check namespace declarations
3. Reimport all scripts

### Issue: Performance is poor
**Solution**:
1. Reduce shadow quality
2. Lower texture resolution
3. Disable post-processing effects
4. Optimize mesh poly count

---

## 📚 Additional Resources

### Unity Documentation
- [Unity Manual](https://docs.unity3d.com/Manual/index.html)
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/)
- [Android Development](https://docs.unity3d.com/Manual/android.html)

### Tutorials
- [Unity Learn](https://learn.unity.com/)
- [Brackeys YouTube](https://www.youtube.com/user/Brackeys)
- [Unity Mobile Optimization](https://learn.unity.com/tutorial/mobile-optimization-practical-guide)

### Community
- [Unity Forum](https://forum.unity.com/)
- [Unity Discord](https://discord.gg/unity)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/unity3d)

---

## 📞 Support

For issues or questions:
- **Email**: rana.16241.ac@iqra.edu.pk
- **GitHub Issues**: [Create an issue](https://github.com/rana16241-ac/SilentPeak-Reclamation/issues)

---

## ✅ Next Steps

After setup is complete:
1. Test all core systems
2. Create placeholder art assets
3. Add sound effects and music
4. Implement remaining enemy types
5. Design and build all 20 bases
6. Integrate ads and IAP
7. Optimize performance
8. Beta testing
9. Release!

**Happy Developing! 🎮**
