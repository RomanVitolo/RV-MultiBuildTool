# RV-MultiBuildTool

## Project Overview
**RV-MultiBuildTool** is a Unity Editor extension designed to streamline the build process for Unity projects. It allows developers to configure multiple build targets (platforms/configurations) and execute all of them with a single action. This tool saves time by eliminating the need to manually switch build targets and settings one by one. With RV-MultiBuildTool, you can set up various build configurations (e.g., different platforms, development/production builds, scene selections) and build them in batch, making multi-platform deployment much easier.

## Installation Instructions

### Installing via Unity Package Manager (UPM)
RV-MultiBuildTool is distributed as a Unity Package Manager (UPM) package on the **UPM branch** of the repository. To install it in your Unity project:

1. **Prerequisite**: Make sure you have **Git** installed on your system, as Unity can fetch UPM packages directly from Git repositories.
2. **Open Unity Package Manager**: In Unity, go to `Window > Package Manager`, then click the **➕ (plus)** button and choose **"Add package from git URL..."**.
3. **Enter Git URL**: Paste the following URL (which targets the UPM branch) and press **Enter**:
   ```
   https://github.com/RomanVitolo/RV-MultiBuildTool.git#upm
   ```
   Unity will download the package from the UPM branch.
4. **Confirm Installation**: After a moment, Unity should display "RV-MultiBuildTool" in the Package Manager list, ready to use in your project.

*Alternatively*, you can add the package directly via your `Packages/manifest.json`:
```json
{
  "dependencies": {
    "com.romanvitolo.multibuildtool": "https://github.com/RomanVitolo/RV-MultiBuildTool.git#upm"
  }
}
```
After saving the manifest, Unity will automatically download and add the package.

## Usage Guide

### Opening the Build Tool Window
Navigate to **`Tools > RV-MultiBuildTool > Builds Tools`** in the Unity Editor to open the tool's window.

### Configuring Build Targets
1. **Add a Build Target**: Click **"Add Target"** (`+` button) to create a new build entry.
2. **Select Platform**: Choose a platform (Windows, macOS, Android, iOS, etc.).
3. A folder called **Builds** will be created inside the project and will host the different builds.
4. **Configure Options**: Enable development build, compression settings, and select scenes.
5. **Run Build**: Click **"Build Platforms"** to build multiple targets in one go.

### Example Usage
For a project requiring Windows and Android builds:
- Add **Windows 64-bit** and **Android** as targets.
- A folder called **Builds** will be created inside the project and will host the different builds..
- Enable **Development Build** for Android.
- Click **Build All** to process both builds sequentially.

## Editing and Contribution

### Cloning the Repository (Main Branch)
If you want to modify or extend RV-MultiBuildTool, clone the **main branch** of the repository:
```sh
git clone https://github.com/RomanVitolo/RV-MultiBuildTool.git
```
This branch contains the full source code. Open the project in Unity to start making changes.

### Contributing Back
1. **Fork & Create a Branch**: Develop features on a new branch.
2. **Implement and Test**: Ensure stability and functionality.
3. **Submit a Pull Request**: Open a PR to merge changes into the main repository.

## Sitaxis
RV-MultiBuildTool follows a simple sitaxis (syntax structure) for managing builds:
- **Add Target:** Define a new build configuration.
- **Select Platform:** Choose which platform to build for.
- **Set Output Path:** A folder called **Builds** will be created inside the project and will host the different builds.
- **Enable Options:** Adjust settings like Development Build or Compression.
- **Build Platforms:** Execute all configured builds in sequence.

The tool is designed to be intuitive, following Unity’s standard workflow, making it accessible to both beginner and advanced developers.

## Links
- **Main Branch (Development)** – [GitHub Main](https://github.com/RomanVitolo/RV-MultiBuildTool/tree/main)
- **UPM Branch (Release)** – [GitHub UPM](https://github.com/RomanVitolo/RV-MultiBuildTool/tree/upm)

## License and Credits
**License**: [MIT License](https://opensource.org/licenses/MIT). See [`LICENSE`](https://github.com/RomanVitolo/RV-MultiBuildTool/blob/main/LICENSE) for details.

**Credits**:
- **Roman Vitolo** – Creator and Maintainer
- **Contributors** – Community members who help improve the tool

If you find this tool useful, consider giving the repository a ⭐ on GitHub. Feedback and contributions are always welcome!

