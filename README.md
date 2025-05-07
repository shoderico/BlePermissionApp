# BlePermissionApp

A .NET MAUI application demonstrating Bluetooth Low Energy (BLE) permission handling and scanning using the `Plugin.BLE` library. This app checks Bluetooth status, requests necessary permissions, and guides users to enable Bluetooth or configure settings on iOS and Android platforms.

## Features
- Checks Bluetooth adapter status (on/off).
- Requests and verifies BLE-related permissions for iOS and Android.
- Displays user-friendly messages to guide users to enable Bluetooth or open settings.
- Platform-specific implementations using Dependency Injection and Facade Pattern.
- Abstract base class to minimize empty platform-specific implementations.
- Clean architecture for maintainability and scalability.

## Technologies Used
- **.NET MAUI**: Cross-platform framework for building iOS, Android, and other platform apps.
- **Plugin.BLE**: Library for BLE communication.
- **C#**: Primary programming language.
- **Dependency Injection**: For managing platform-specific services.
- **Facade Pattern**: Simplifies platform-specific code integration.

## Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or later (with .NET MAUI workload)
- Xcode (for iOS development on macOS)
- Android SDK (for Android development)
- Physical devices recommended for BLE testing (emulators may not support BLE).

## Setup
1. **Clone the repository**:
   ```bash
   git clone https://github.com/shoderico/BlePermissionApp.git
   cd BlePermissionApp
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Configure platform-specific permissions**:
   - **iOS**:
     - Add `NSBluetoothAlwaysUsageDescription` to `Platforms/iOS/Info.plist` for Bluetooth permission.
     - Example:
       ```xml
       <key>NSBluetoothAlwaysUsageDescription</key>
       <string>This app requires Bluetooth to scan for nearby devices.</string>
       ```
   - **Android**:
     - Ensure `BLUETOOTH`, `BLUETOOTH_ADMIN`, `BLUETOOTH_SCAN`, `BLUETOOTH_CONNECT`, and `ACCESS_FINE_LOCATION` permissions are declared in `Platforms/Android/AndroidManifest.xml`.
     - Example:
       ```xml
       <uses-permission android:name="android.permission.BLUETOOTH" />
       <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />
       <uses-permission android:name="android.permission.BLUETOOTH_SCAN" />
       <uses-permission android:name="android.permission.BLUETOOTH_CONNECT" />
       <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
       ```

4. **Build and run**:
   - Open the solution (`BlePermissionApp.sln`) in Visual Studio.
   - Select the target platform (iOS, Android, etc.).
   - Build and deploy to a physical device.

## Usage
1. Launch the app on your device.
2. The app checks Bluetooth status and permissions.
3. Follow on-screen prompts to:
   - Enable Bluetooth if disabled.
   - Grant necessary permissions.
   - Open settings if permissions are denied.
4. Once permissions are granted, the app can scan for nearby BLE devices.

## Project Structure
The project is organized to separate platform-specific code, shared services, and UI components, leveraging dependency injection and the facade pattern for clean architecture. Below is the folder structure based on the repository:

- **`BlePermissionApp/`**: Root directory for the MAUI project.
  - `MauiProgram.cs`: Entry point for dependency injection setup, registering services and platform-specific implementations.
  - `MainPage.xaml`/`MainPage.xaml.cs`: Main UI displaying Bluetooth status, permission prompts, and scan controls.
- **`Platforms/`**: Platform-specific implementations.
  - `Android/`:
    - `PlatformService.cs`: Android-specific BLE permission handling and Bluetooth adapter checks.
    - `AndroidManifest.xml`: Declares Android permissions (e.g., BLUETOOTH, ACCESS_FINE_LOCATION).
  - `iOS/`:
    - `PlatformService.cs`: iOS-specific BLE permission requests using CoreBluetooth.
    - `Info.plist`: Configures Bluetooth usage description.
- **`Shared/Services/`**: Shared services for BLE and permission management.
  - `IPlatformService.cs`: Interface for platform-specific BLE and permission operations.
  - `PlatformServiceBase.cs`: Abstract base class providing default (empty) implementations for platform services.
  - `AppServices.cs`: Facade that coordinates BLE scanning, permission checks, and platform-specific logic via dependency injection.

This structure ensures modularity, with platform-specific code isolated and shared logic centralized in `Shared/Services/`.

## Contributing
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -m 'Add your feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Open a Pull Request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments
- [Plugin.BLE](https://github.com/xabre/xamarin-bluetooth-le) for BLE functionality.
- .NET MAUI community for documentation and support.

## Contact
For questions or feedback, open an issue or reach out to [@shoderico](https://github.com/shoderico) on GitHub.