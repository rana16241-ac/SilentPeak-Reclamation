# 🎯 Silent Peak: Reclamation

![Unity](https://img.shields.io/badge/Unity-2021.3%2B-black?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Android-green?logo=android)
![License](https://img.shields.io/badge/License-MIT-blue)

## 📖 Game Overview

**Silent Peak: Reclamation** is a tactical third-person sniper stealth game for Android mobile devices. Play as an elite sniper tasked with reclaiming American military bases from rebellious terrorist forces. Every shot counts - one miss triggers the alarm and fails the mission!

### 🎮 Key Features

- **17-20 Unique Military Bases** with 7-8 sub-levels each
- **Tactical Stealth Gameplay** - Silent elimination from long distance
- **Progressive Difficulty** - Enemy count increases: 3 → 7 → 10 → 15 → 20 → 24 → 27 → 30
- **6 Enemy Types** - Guards, Patrols, Heavy Soldiers, Snipers, Commanders, Elite Rebels
- **Weapon Upgrade System** - Scope, Stability, Reload Speed, Damage, Silencer
- **Character Customization** - Hair styles/colors, uniforms, gear
- **Offline Playable** - No internet required
- **Landscape Mode** - Optimized for horizontal gameplay

## 🎯 Game Mechanics

### Core Gameplay
- **One Shot, One Kill** - Precision is everything
- **No Misses Allowed** - Missing alerts enemies and fails the mission
- **Scope Zoom Levels** - Multiple zoom options for different ranges
- **Breath Control** - Steady your aim for the perfect shot
- **Bullet Physics** - Advanced levels include bullet drop and wind

### Mission Structure
1. Cinematic intro showing sniper approach
2. Setup at hidden vantage point
3. Eliminate all enemies silently
4. Mission complete or alarm triggered

## 💰 Economy System

### Earn Coins
- Mission completion rewards
- Headshot bonuses
- Speed completion bonuses
- Watch ads: **100 coins per ad**

### Coin Store
| Price | Coins |
|-------|-------|
| $2    | 3,000 |
| $5    | 7,000 |
| $10   | 15,000 |
| $15   | 20,000 |
| $20   | 30,000 |
| $25   | 40,000 |
| $30   | 50,000 |

## 🛠️ Technical Details

### Platform Requirements
- **Unity Version**: 2021.3 LTS or newer
- **Platform**: Android
- **Minimum API**: Android 7.0 (API 24)
- **Target API**: Android 13 (API 33)
- **Orientation**: Landscape
- **Scripting Backend**: IL2CPP

### Project Structure
```
Assets/
├── Scenes/
│   ├── MainMenu.unity
│   ├── GamePlay.unity
│   └── Loading.unity
├── Scripts/
│   ├── Core/
│   ├── UI/
│   ├── Player/
│   ├── Enemy/
│   ├── Weapons/
│   └── Economy/
├── Prefabs/
├── Materials/
├── Textures/
├── Models/
└── Audio/
```

## 🚀 Getting Started

### Prerequisites
- Unity 2021.3 LTS or newer
- Android SDK
- JDK 11 or newer

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/rana16241-ac/SilentPeak-Reclamation.git
   cd SilentPeak-Reclamation
   ```

2. **Open in Unity**
   - Open Unity Hub
   - Click "Add" and select the project folder
   - Open the project

3. **Build for Android**
   - File → Build Settings
   - Select Android platform
   - Switch Platform
   - Build APK or Export to Android Studio

### Android Studio Integration

1. **Export Project**
   - In Unity: File → Build Settings → Export Project
   - Choose export location

2. **Open in Android Studio**
   ```bash
   cd exported-project
   ./gradlew assembleDebug
   ```

3. **Install on Device**
   ```bash
   adb install app/build/outputs/apk/debug/app-debug.apk
   ```

## 📱 Controls

- **Tap & Drag** - Aim the sniper scope
- **Pinch** - Zoom in/out
- **Tap Fire Button** - Shoot
- **Hold Breath Button** - Stabilize aim

## 🎨 Customization

### Weapons
- 5 upgrade categories per weapon
- Multiple sniper rifle variants
- Premium weapon skins (cosmetic only)

### Character
- **Hair**: 3 styles × 12 colors = 36 combinations
- **Body**: Height & weight presets
- **Gear**: Uniforms, shoes, gloves, helmets
- **Premium Characters**: Special cosmetic characters

## 🏆 Progression System

- **17-20 Bases** to reclaim
- **7-8 Sub-levels** per base
- **Star Rating System** (0-3 stars per level)
- **Unlock System** - Complete levels to unlock next
- **Achievement Tracking** - Headshots, perfect missions, speed runs

## 🎵 Audio

- Realistic sniper rifle sounds
- Alarm siren system
- Ambient military base sounds
- Cinematic music tracks
- UI sound effects

## 🔒 Privacy & Data

- **No Login Required** - Play immediately
- **Local Storage Only** - All data stored on device
- **Offline Playable** - No internet connection needed
- **Optional Ads** - Watch for bonus coins

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Developer

**RANA MUHAMMAD AWAIS**
- Email: rana.16241.ac@iqra.edu.pk
- GitHub: [@rana16241-ac](https://github.com/rana16241-ac)

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 Development Roadmap

- [x] Core game architecture
- [x] Main menu system
- [x] Player data management
- [x] Economy system
- [ ] Sniper gameplay mechanics
- [ ] Enemy AI system
- [ ] Level generation
- [ ] Weapon upgrade system
- [ ] Character customization
- [ ] Audio integration
- [ ] Ad integration
- [ ] In-app purchases
- [ ] Performance optimization
- [ ] Beta testing
- [ ] Release v1.0

## 🎮 Gameplay Screenshots

*Coming soon...*

## 📞 Support

For support, email rana.16241.ac@iqra.edu.pk or open an issue in this repository.

---

**Made with ❤️ for mobile gaming enthusiasts**
