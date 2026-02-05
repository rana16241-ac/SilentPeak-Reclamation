# 🤝 Contributing to Silent Peak: Reclamation

Thank you for your interest in contributing to Silent Peak: Reclamation! This document provides guidelines and instructions for contributing to the project.

---

## 📋 Table of Contents

1. [Code of Conduct](#code-of-conduct)
2. [Getting Started](#getting-started)
3. [Development Workflow](#development-workflow)
4. [Coding Standards](#coding-standards)
5. [Commit Guidelines](#commit-guidelines)
6. [Pull Request Process](#pull-request-process)
7. [Bug Reports](#bug-reports)
8. [Feature Requests](#feature-requests)

---

## 📜 Code of Conduct

### Our Pledge
We are committed to providing a welcoming and inspiring community for all. Please be respectful and constructive in all interactions.

### Expected Behavior
- Use welcoming and inclusive language
- Be respectful of differing viewpoints
- Accept constructive criticism gracefully
- Focus on what is best for the community
- Show empathy towards other community members

### Unacceptable Behavior
- Harassment or discriminatory language
- Trolling or insulting comments
- Public or private harassment
- Publishing others' private information
- Other conduct which could reasonably be considered inappropriate

---

## 🚀 Getting Started

### Prerequisites
1. Unity 2021.3 LTS or newer
2. Git installed and configured
3. GitHub account
4. Basic knowledge of C# and Unity

### Fork and Clone
```bash
# Fork the repository on GitHub
# Then clone your fork
git clone https://github.com/YOUR_USERNAME/SilentPeak-Reclamation.git
cd SilentPeak-Reclamation

# Add upstream remote
git remote add upstream https://github.com/rana16241-ac/SilentPeak-Reclamation.git
```

### Setup Development Environment
```bash
# Fetch latest changes
git fetch upstream

# Create a new branch for your feature
git checkout -b feature/your-feature-name upstream/main
```

---

## 🔄 Development Workflow

### 1. Create a Branch
Always create a new branch for your work:
```bash
git checkout -b feature/amazing-feature
# or
git checkout -b bugfix/fix-issue-123
# or
git checkout -b docs/update-readme
```

### Branch Naming Convention
- `feature/` - New features
- `bugfix/` - Bug fixes
- `hotfix/` - Critical fixes
- `docs/` - Documentation updates
- `refactor/` - Code refactoring
- `test/` - Adding tests
- `style/` - Code style changes

### 2. Make Changes
- Write clean, readable code
- Follow the coding standards (see below)
- Test your changes thoroughly
- Update documentation if needed

### 3. Commit Changes
```bash
git add .
git commit -m "feat: add amazing feature"
```

### 4. Push to Your Fork
```bash
git push origin feature/amazing-feature
```

### 5. Create Pull Request
- Go to GitHub and create a Pull Request
- Fill out the PR template
- Link related issues
- Request review

---

## 💻 Coding Standards

### C# Style Guide

#### Naming Conventions
```csharp
// Classes: PascalCase
public class GameManager { }

// Methods: PascalCase
public void StartGame() { }

// Private fields: camelCase with underscore
private int _playerHealth;

// Public properties: PascalCase
public int PlayerHealth { get; set; }

// Constants: UPPER_CASE
private const int MAX_HEALTH = 100;

// Local variables: camelCase
int enemyCount = 10;
```

#### Code Organization
```csharp
using UnityEngine;
using System.Collections.Generic;

namespace SilentPeak.Core
{
    /// <summary>
    /// Brief description of the class
    /// </summary>
    public class ExampleClass : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Settings")]
        [SerializeField] private int maxValue = 100;
        #endregion

        #region Private Fields
        private int currentValue;
        #endregion

        #region Properties
        public int CurrentValue => currentValue;
        #endregion

        #region Unity Lifecycle
        private void Awake() { }
        private void Start() { }
        private void Update() { }
        #endregion

        #region Public Methods
        public void DoSomething() { }
        #endregion

        #region Private Methods
        private void HelperMethod() { }
        #endregion
    }
}
```

#### Comments and Documentation
```csharp
/// <summary>
/// Calculates damage based on weapon type and upgrades
/// </summary>
/// <param name="weaponType">Type of weapon used</param>
/// <param name="isHeadshot">Whether the shot was a headshot</param>
/// <returns>Total damage value</returns>
public int CalculateDamage(WeaponType weaponType, bool isHeadshot)
{
    // Implementation
}
```

### Unity Best Practices

#### Performance
- Use object pooling for frequently instantiated objects
- Cache component references in Awake/Start
- Avoid using Find methods in Update
- Use coroutines instead of Update when possible

```csharp
// Good
private Transform _transform;
private void Awake()
{
    _transform = transform;
}

// Bad
private void Update()
{
    transform.position = Vector3.zero; // Calls GetComponent every frame
}
```

#### Memory Management
- Unsubscribe from events in OnDestroy
- Dispose of resources properly
- Avoid memory leaks

```csharp
private void OnEnable()
{
    EventManager.OnGameOver += HandleGameOver;
}

private void OnDisable()
{
    EventManager.OnGameOver -= HandleGameOver;
}
```

---

## 📝 Commit Guidelines

### Commit Message Format
```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding tests
- `chore`: Maintenance tasks

### Examples
```bash
feat(player): add breath control mechanic

Implemented breath holding system that stabilizes aim.
- Added breath meter UI
- Breath depletes over time when held
- Recovers when released

Closes #42

---

fix(enemy): correct patrol waypoint navigation

Fixed issue where patrol enemies would get stuck at waypoints.

Fixes #38

---

docs(readme): update installation instructions

Added detailed steps for Android Studio setup.
```

### Commit Best Practices
- Use present tense ("add feature" not "added feature")
- Use imperative mood ("move cursor to..." not "moves cursor to...")
- Limit first line to 72 characters
- Reference issues and pull requests

---

## 🔀 Pull Request Process

### Before Submitting
- [ ] Code follows the style guidelines
- [ ] Self-review of code completed
- [ ] Comments added for complex code
- [ ] Documentation updated
- [ ] No new warnings generated
- [ ] Tests added/updated
- [ ] All tests pass
- [ ] Tested on Android device

### PR Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
Describe testing performed

## Screenshots (if applicable)
Add screenshots

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-reviewed code
- [ ] Commented complex code
- [ ] Updated documentation
- [ ] No new warnings
- [ ] Added tests
- [ ] Tests pass
- [ ] Tested on device
```

### Review Process
1. Maintainer reviews code
2. Feedback provided if needed
3. Make requested changes
4. Re-request review
5. Approved and merged

---

## 🐛 Bug Reports

### Before Submitting
- Check existing issues
- Test on latest version
- Gather reproduction steps

### Bug Report Template
```markdown
**Describe the Bug**
Clear description of the bug

**To Reproduce**
Steps to reproduce:
1. Go to '...'
2. Click on '...'
3. See error

**Expected Behavior**
What should happen

**Screenshots**
If applicable

**Device Info**
- Device: [e.g. Samsung S21]
- OS: [e.g. Android 12]
- Game Version: [e.g. 1.0.0]

**Additional Context**
Any other information
```

---

## 💡 Feature Requests

### Feature Request Template
```markdown
**Is your feature request related to a problem?**
Clear description of the problem

**Describe the solution you'd like**
Clear description of desired solution

**Describe alternatives you've considered**
Alternative solutions considered

**Additional context**
Any other information, mockups, etc.
```

---

## 🎨 Asset Contributions

### 3D Models
- Format: FBX or OBJ
- Poly count: < 5000 for mobile
- Textures: Power of 2 (512x512, 1024x1024)
- Include normal maps if applicable

### 2D Assets
- Format: PNG with transparency
- Resolution: Appropriate for mobile
- Organized in folders

### Audio
- Format: WAV or OGG
- Sample rate: 44.1kHz
- Bit depth: 16-bit
- Normalized audio levels

---

## 📞 Communication

### Channels
- **GitHub Issues**: Bug reports, feature requests
- **GitHub Discussions**: General questions, ideas
- **Email**: rana.16241.ac@iqra.edu.pk

### Response Times
- Issues: Within 48 hours
- Pull Requests: Within 1 week
- Questions: Within 72 hours

---

## 🏆 Recognition

Contributors will be:
- Listed in CONTRIBUTORS.md
- Credited in game credits
- Mentioned in release notes

---

## 📄 License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

## ❓ Questions?

Don't hesitate to ask! We're here to help.

**Thank you for contributing to Silent Peak: Reclamation! 🎮**
