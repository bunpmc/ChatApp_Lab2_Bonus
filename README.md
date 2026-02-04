# SignalR Premium Chat Client

A high-performance chat client built with ASP.NET Core Razor Pages and SignalR.

## Features
- **Real-time Messaging**: Powered by SignalR.
- **Rich Emoji Support**: Integrated emoji picker.
- **Large File Uploads**: Support for files up to 2GB.
- **File Previews**: Automatic preview for images and videos directly in the chat.
- **Premium UI**: Modern dark aesthetics with glassmorphism.

## How to Run

1. **Prerequisites**: Ensure .NET 9.0 SDK is installed.
2. **Run the Application**:
   ```bash
   dotnet run
   ```
3. **Open Browser**: Navigate to `http://localhost:5000` (or the URL shown in terminal).

## Implementation Details
- **Hub**: `ChatHub` inherits from `Microsoft.AspNetCore.SignalR.Hub`.
- **Large Files**: Configured Kestrel and FormOptions to allow 2GB body size.
- **Uploads**: Files are uploaded via AJAX to a Razor Page handler, then the link is broadcasted via SignalR to all clients.

## Notes
- Ensure you have write permissions to the `wwwroot/uploads` folder (it will be created automatically).
- For local network access, configure `appsettings.json` or use `--urls`.
