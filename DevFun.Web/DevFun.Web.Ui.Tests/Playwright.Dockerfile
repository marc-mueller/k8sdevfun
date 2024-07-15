# Dockerfile
FROM mcr.microsoft.com/playwright:v1.44.1-jammy

WORKDIR /app

# Install Playwright and necessary dependencies
RUN npm install -g playwright@1.44

ARG DEBIAN_FRONTEND=noninteractive

RUN apt-get update && \
    apt-get install -y x11vnc xvfb fluxbox && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

RUN mkdir ~/.vnc && \
    x11vnc -storepasswd secret ~/.vnc/passwd

# Expose necessary ports
EXPOSE 3000
EXPOSE 5900

# Start VNC server, window manager, and Playwright server
CMD ["sh", "-c", "Xvfb :99 -screen 0 1920x1080x24 & export DISPLAY=:99 && fluxbox & x11vnc -usepw -display :99 -forever -shared & npx playwright run-server --port 3000"]
