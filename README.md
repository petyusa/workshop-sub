# Sonrisa Workspace Booking Application

A modern workspace booking application built with .NET 9+ API and Angular 20+ client, featuring desk and parking reservations with real-time availability tracking.

## 🚀 Quick Start

### Prerequisites
- .NET 9+ SDK
- Node.js 18+ 
- Visual Studio Code or Visual Studio 2022

### Run the Full Stack (Recommended)
```bash
# Install dependencies and start both API and client
npm run dev
```

### Run Services Individually

#### API (.NET)
```bash
cd api
dotnet run --project src/Workshop.Api
```
- API: http://localhost:5074
- Swagger UI: http://localhost:5074/swagger

#### Client (Angular)
```bash
cd client
npm install
npm start
```
- Client: http://localhost:4200

## 📁 Project Structure

```
workshopprep/
├── 📋 README.md                    # This file
├── 📦 package.json                 # Root workspace scripts
├── � .github/                     # Development guidelines
│   └── instructions/               # Coding standards & best practices
├── 🌐 api/                         # .NET 9+ API
│   ├── Workshop.sln
│   └── src/Workshop.Api/
└── 📱 client/                      # Angular 20+ SPA
    ├── src/app/
    └── package.json
```

## 🎯 Application Features

### Core Technologies
- **.NET 9+**: Minimal APIs, OpenAPI/Swagger documentation
- **Angular 20+**: Signals, Standalone Components, Modern Control Flow
- **Tailwind CSS**: Utility-first CSS framework for rapid styling
- **Sonrisa Branding**: Clean white design with signature green (#7dd13d) accents

### Key Features
- 🏢 **Workspace Booking**: Reserve desks and parking spots
- � **Real-time Dashboard**: Live availability and utilization stats
- 🎨 **Modern UI/UX**: Clean, professional interface with Sonrisa branding
- � **Responsive Design**: Works seamlessly on all devices
- ⚡ **Interactive Elements**: Real-time booking simulations
- 🔄 **Signal-based State**: Modern Angular reactive patterns

## 📖 Development Guidelines

- **API Development**: See `.github/instructions/api.instructions.md`
- **Client Development**: See `.github/instructions/client.instructions.md`

## 🛠️ Available Scripts

```bash
# Development
npm run dev              # Start both API and client
npm run dev:api          # Start API only
npm run dev:client       # Start client only
npm run start            # Alias for npm run dev

# Setup
npm run setup            # Install all dependencies
npm run install:all      # Install dependencies for both projects

# Building
npm run build            # Build both projects
npm run build:api        # Build API only
npm run build:client     # Build client only

# Testing
npm run test             # Run all tests
npm run test:api         # Run API tests
npm run test:client      # Run client tests

# Maintenance
npm run clean            # Clean all build artifacts
npm run clean:api        # Clean API build files
npm run clean:client     # Clean client build files and node_modules
```

## 🌟 Current Implementation

- ✅ **Sonrisa Branding**: Clean white design with signature green (#7dd13d)
- ✅ **Workspace Booking Interface**: Desk and parking reservation landing page
- ✅ **Real-time Stats**: Live dashboard showing availability and utilization
- ✅ **Modern Angular 20+**: Signals, computed properties, and OnPush change detection
- ✅ **API Foundation**: .NET 9+ with Swagger documentation
- ✅ **CORS Configuration**: Ready for client-API communication
- ✅ **Responsive Design**: Mobile-first approach with Tailwind CSS
- ✅ **Development Guidelines**: Comprehensive coding standards

##  Support & Development

- 📋 **Coding Standards**: Check `.github/instructions/` for development guidelines
- � **API Guidelines**: See `.github/instructions/api.instructions.md`
- 🎨 **Frontend Standards**: See `.github/instructions/client.instructions.md`
- 🐛 **Issues**: Report problems in the GitHub Issues section

---

## 🚀 Getting Started

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd workshopprep
   ```

2. **Install dependencies**
   ```bash
   npm run setup
   ```

3. **Start development**
   ```bash
   npm run dev
   ```

4. **Access the application**
   - Client: http://localhost:4200
   - API: http://localhost:5074
   - Swagger: http://localhost:5074/swagger

---

**Welcome to Sonrisa Workspace Solutions! 🏢✨**

*Building modern workplace experiences with cutting-edge technology.*