# C# Console Chat Application

A simple chat application built using C# 12.0, following the **Client-Server Architecture**. This project allows multiple clients to connect to a central server, exchange messages, and interact in real-time using a console interface.

## Features

- **Client-Server Communication**: The server manages multiple clients and broadcasts messages between them.
- **Real-Time Chat**: Clients can send and receive messages in real-time.
- **Nickname Feature**: Each client can choose a nickname upon connection, which will be displayed with every message.
- **Console-Based Interface**: Simple console-based user interaction for both the server and clients.

## Architecture

The project is divided into three parts:

1. **ChatStream (Class Library)**: Contains the logic for sending and receiving messages, including creating and parsing message packets.
2. **Server (Console App)**: Handles multiple client connections and broadcasts messages to all connected clients.
3. **Client (Console App)**: Allows users to connect to the server, choose a nickname, and send/receive messages.

## Getting Started

### Prerequisites

- .NET SDK installed on your machine.

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Youssef-Remah/Csharp-Chat-App.git
   ```
2. Restore the necessary NuGet packages:
   ```bash
   dotnet restore
   ```
### Running the Application
1. **Start the Server**: Run the Server project first. It will listen for incoming client connections.
    ```bash
    dotnet run --project Server
    ```
2. **Run Clients**: Open multiple terminal windows and run the Client project in each one. The clients will connect to the server and prompt for a nickname.
    ```bash
    dotnet run --project Client
    ```
3. **Start Chatting**: Once the clients are connected, type messages in any client terminal, and the server will broadcast the message to all other clients.
